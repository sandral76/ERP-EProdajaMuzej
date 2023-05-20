using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using API.Errors;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EProdajaMuzejContext dbContext;
        private readonly IConfiguration configuration;
        private readonly IGenericRepository<Korisnik> korisnikRepo;
        private readonly IGenericRepository<Porudzbina> porudzbinaRepo;
        private readonly IGenericRepository<StavkaPorudzbine> stavkaRepo;
        private readonly IGenericRepository<Korpa> korpaRepo;
        private readonly IMapper mapper;
        private readonly PasswordHasher<Korisnik> passwordHasher;

        public AuthController(EProdajaMuzejContext dbContext, IConfiguration configuration, IGenericRepository<Korisnik> korisnikRepo, IMapper mapper,
        IGenericRepository<Porudzbina> porudzbinaRepo, IGenericRepository<StavkaPorudzbine> stavkaRepo, IGenericRepository<Korpa> korpaRepo)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.korisnikRepo = korisnikRepo;
            this.mapper = mapper;
            passwordHasher = new PasswordHasher<Korisnik>();
            this.porudzbinaRepo = porudzbinaRepo;
            this.stavkaRepo = stavkaRepo;
            this.korpaRepo = korpaRepo;

        }

        [Authorize]
        [HttpGet]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<LoginWithTokenDTO>> GetCurrentKorisnik()
        {

            var korisnickoImeClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(korisnickoImeClaim))
            {
                var korisnik = dbContext.Korisniks.FirstOrDefault(u => u.KorisnickoIme == korisnickoImeClaim);
                var loggedKorisnik = new LoginWithTokenDTO { KorisnickoIme = korisnik.KorisnickoIme, Lozinka = korisnik.Lozinka, Token = token };

                return loggedKorisnik;
            }

            return Unauthorized(new ApiResponses(401));
        }


        [HttpPost]
        [Route("register")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<KorisnikDTO>> Register([FromBody] RegisterKorisnikDTO registerKorisnik)
        {

            var isExistingKorisnik = dbContext.Korisniks.Where(u => u.KorisnickoIme == registerKorisnik.KorisnickoIme).FirstOrDefault();
            if (isExistingKorisnik != null)
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Korisnicko ime je zauzeto" } });
            var noviKorisnik = new Korisnik
            {

                KorisnickoIme = registerKorisnik.KorisnickoIme,
                Lozinka = passwordHasher.HashPassword(null, registerKorisnik.Lozinka),
                Ime = registerKorisnik.Ime,
                Prezime = registerKorisnik.Prezime,
                BrojTel = registerKorisnik.BrojTel,
                Email = registerKorisnik.Email,
                TipKorisnikaId = 2 // da svi budu registrovani korisnici
            };
            korisnikRepo.Add(noviKorisnik);
            var spec = new KorisnikWithTip(noviKorisnik.KorisnikId);
            var korisnik = await korisnikRepo.GetEntityWithSpec(spec);
            return mapper.Map<Korisnik, KorisnikDTO>(noviKorisnik);

        }

        [HttpPost]
        [Route("login")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<LoginWithTokenDTO>> Login(LoginDTO loginRequest)
        {
            var isExistingKorisnik = dbContext.Korisniks.Where(u => u.KorisnickoIme == loginRequest.KorisnickoIme).FirstOrDefault();
            if (isExistingKorisnik == null)
                //return Unauthorized("Korisnicko ime je neispravno."); 
                return Unauthorized(new ApiResponses(401));

            var result = passwordHasher.VerifyHashedPassword(isExistingKorisnik, isExistingKorisnik.Lozinka, loginRequest.Lozinka);
            if (result != PasswordVerificationResult.Success)
                //return Unauthorized("Lozinka je neispravna.");
                return Unauthorized(new ApiResponses(401));

            string role;
            switch (isExistingKorisnik.TipKorisnikaId)
            {
                case 1:
                    role = "Admin";
                    break;
                case 2:
                    role = "Registrovani korisnik";
                    break;
                default:
                    role = "Super korisnik";
                    break;
            }

            var authClaims = new List<Claim>{  //cleaim-'tvrdanja' o identitetu korisniku koji se prijavljuje u sistem
                new Claim(ClaimTypes.Name,isExistingKorisnik.KorisnickoIme),
                new Claim("JWTID",Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role),
            };

            var token = GenerateNewJsonWebToken(authClaims);
            var korisnik = new LoginWithTokenDTO { KorisnickoIme = isExistingKorisnik.KorisnickoIme, Lozinka = isExistingKorisnik.Lozinka, Token = token };
            return Ok(korisnik);

        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {

            var authSeceret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var tokenObject = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],    //ko izdaje token
                audience: configuration["JWT:ValidAudience"],   //kome je namewnjen
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authSeceret, SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }

        [HttpPost]
        [Route("make-admin")]
        [EnableCors("AllowOrigin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDTO updatePermissionRequest)
        {

            var isExistingKorisnik = dbContext.Korisniks.Where(u => u.KorisnickoIme == updatePermissionRequest.KorisnickoIme).FirstOrDefault();
            if (isExistingKorisnik == null)
                return BadRequest("Neispravno korisnicko ime.");

            isExistingKorisnik.TipKorisnikaId = 1;
            dbContext.Korisniks.Update(isExistingKorisnik);
            dbContext.SaveChanges();

            return Ok("Korisnik je sada admin.");
        }

        [HttpGet("korisnickoImeexists")]
        public bool CheckKorisnickoImeExists([FromQuery] string korisnickoIme)
        {
            var isExistingKorisnik = dbContext.Korisniks.Where(u => u.KorisnickoIme == korisnickoIme).FirstOrDefault();
            if (isExistingKorisnik == null)
                return false;
            else return true;
        }
        [HttpGet("dostava")]
        [EnableCors("AllowOrigin")]
        [Authorize]
        public async Task<ActionResult<DetaljiPorudzbineDTO>> GetKorisnikAdresa()
        {
            var korisnickoImeClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(korisnickoImeClaim))
            {
                var korisnik = dbContext.Korisniks.FirstOrDefault(u => u.KorisnickoIme == korisnickoImeClaim);
                return mapper.Map<Korisnik, DetaljiPorudzbineDTO>(korisnik);
            }
            else return Unauthorized(new ApiResponses(401));
        }
        [HttpPut("dostava")]
        [EnableCors("AllowOrigin")]
        [Authorize]
        public async Task<ActionResult<DetaljiPorudzbineDTO>> UpdateKorisnikAdresa(DetaljiPorudzbineDTO detaljiPorudzbineUpdate)
        {
            var korisnickoImeClaim = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var korisnik = dbContext.Korisniks.FirstOrDefault(u => u.KorisnickoIme == korisnickoImeClaim);
            if (korisnik != null)
            {
                korisnik.BrojTel = detaljiPorudzbineUpdate.KontaktTelefon;
                korisnik.Email = detaljiPorudzbineUpdate.EmailDostave;
                korisnik.Ime = detaljiPorudzbineUpdate.Ime;
                korisnik.Prezime = detaljiPorudzbineUpdate.Ime;

                dbContext.Korisniks.Update(korisnik);
                await dbContext.SaveChangesAsync();
                return mapper.Map<Korisnik, DetaljiPorudzbineDTO>(korisnik);
            }
            else return Unauthorized(new ApiResponses(401));

        }


        /*public async Task<Porudzbina> CreatePorudzbinaAsync(string korisnickoIme, int korpaId, DetaljiPorudzbineDTO dostava)
        {

            var spec1 = new KorpaWithStavkaPorudzbine(korpaId);
            var korpa = await korpaRepo.GetEntityWithSpec(spec1);

            var stavke = new List<StavkaPorudzbine>();
            foreach (var stavka in korpa.StavkaPorudzbines)
            {
                var spec2 = new StavkaPorudzbinaWithDatumKreiranja(stavka.StavkaPorudzbineId,stavka.UlaznicaId);
                var stavkaPorudzbine= await stavkaRepo.GetEntityWithSpec(spec2);
                stavke.Add(stavkaPorudzbine);
            }
            var ukupanIznos=stavke.Sum(stavka=>stavka.CenaStavka);

            var porudzbina=new Porudzbina()
        }*/
    }
}



































/*generiše JWT (JSON Web Token) na osnovu tvrdnji o identitetu korisnika (claims) i drugih parametara koje je neophodno definisati za JWT.

Prvi red koda kreira objekat SymmetricSecurityKey koji predstavlja ključ za potpisivanje JWT tokena. Ključ se kreira iz UTF-8 enkodiranog niza bajtova koji se dobijaju iz konfiguracionog fajla (appsettings.json) u kojem se nalazi tajna vrednost ključa ("JWT:Secret").
Drugi red koda kreira novi JWT token (JwtSecurityToken) koji se sastoji od sledećih parametara:
"issuer": identifikuje ko je izdao JWT token, a vrednost se takođe dobija iz konfiguracionog fajla ("JWT:ValidIssuer").
"audience": identifikuje kome je JWT token namenjen, a vrednost se takođe dobija iz konfiguracionog fajla ("JWT:ValidAudience").
"expires": definiše kada će JWT token isteći. U ovom slučaju, token će isteći za jedan sat od trenutka kreiranja JWT tokena (DateTime.Now.AddHours(1)).
"claims": predstavlja tvrdnje o identitetu korisnika, koje su prethodno definisane u listi "claims".
"signingCredentials": definiše algoritam koji se koristi za potpisivanje JWT tokena. U ovom slučaju se koristi HMACSHA256 algoritam za potpisivanje JWT tokena, a koristi se SymmetricSecurityKey koji je kreiran u prvom redu koda.
Treći red koda kreira JWT token kao string pomoću JwtSecurityTokenHandler klase.
Četvrti red koda vraća JWT token kao rezultat.
Ukratko, ovaj kod generiše JWT token na osnovu tvrdnji o identitetu korisnika, tajne vrednosti ključa za potpisivanje, i drugih parametara koji definišu svojstva JWT tokena.*/