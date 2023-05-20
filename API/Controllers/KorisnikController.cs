using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using API.DTOs;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KorisnikController : ControllerBase
    {
        private readonly IGenericRepository<Korisnik> korisnikRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;
        private readonly PasswordHasher<Korisnik> passwordHasher;

        public KorisnikController(IGenericRepository<Korisnik> korisnikRepo, IMapper mapper, EProdajaMuzejContext dbContext)
        {
            this.korisnikRepo = korisnikRepo;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.passwordHasher = new PasswordHasher<Korisnik>();
        }
        [HttpGet]
        //[Authorize(Roles="Admin")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IReadOnlyList<KorisnikDTO>>> GetKorisnik()
        {
            /*var korisnici = dbContext.Korisniks
            .Include(x => x.TipKorisnika)
            .Include(y => y.Porudzbinas)
            .ThenInclude(p => p.StavkaPorudzbines).ToList();*/
            var spec = new KorisnikWithTip();
            var korisnici = await korisnikRepo.ListAsync(spec);
            return Ok(mapper.Map<IReadOnlyList<Korisnik>, IReadOnlyList<KorisnikDTO>>(korisnici));

        }
        [HttpGet("{korisnikID}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<KorisnikDTO>> GetKorisnikByID(int korisnikID)
        {
            var korisnik = dbContext.Korisniks
            .Include(x => x.TipKorisnika)
            .Include(y => y.Porudzbinas)
            .ThenInclude(p => p.StavkaPorudzbines)
            .FirstOrDefault(k => k.KorisnikId == korisnikID);
            //var spec = new KorisnikWithTip(korisnikID);
            //var korisnik = await korisnikRepo.GetEntityWithSpec(spec);
            return mapper.Map<Korisnik, KorisnikDTO>(korisnik);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<KorisnikDTO>> AddKorisnik(Korisnik addKorisnikRequest)
        {

            var isExistingKorisnik = dbContext.Korisniks.Where(u => u.KorisnickoIme == addKorisnikRequest.KorisnickoIme).FirstOrDefault();
            if (isExistingKorisnik != null)
                return BadRequest("Korisnicko ime je zauzeto");
            var noviKorisnik = new Korisnik
            {

                KorisnickoIme = addKorisnikRequest.KorisnickoIme,
                Lozinka = passwordHasher.HashPassword(null, addKorisnikRequest.Lozinka),
                Ime = addKorisnikRequest.Ime,
                Prezime = addKorisnikRequest.Prezime,
                BrojTel = addKorisnikRequest.BrojTel,
                Email = addKorisnikRequest.Email,
                TipKorisnikaId = addKorisnikRequest.TipKorisnikaId
            };
            korisnikRepo.Add(noviKorisnik);
            var spec = new KorisnikWithTip(noviKorisnik.KorisnikId);
            var korisnik = await korisnikRepo.GetEntityWithSpec(spec);
            return mapper.Map<Korisnik, KorisnikDTO>(noviKorisnik);
        }

        [HttpPut("{korisnikID}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<KorisnikDTO>> UpdateKorisnika(int korisnikID, Korisnik updateKorisnikaRequest)
        {
            var isExistingKorisnik = dbContext.Korisniks.Where(u => u.KorisnickoIme == updateKorisnikaRequest.KorisnickoIme).FirstOrDefault();
            if (isExistingKorisnik != null)
                return BadRequest("Korisnicko ime je zauzeto");

            var updateKorisnik = await korisnikRepo.Update(updateKorisnikaRequest, korisnikID, (existingKorisnik, newKorisnik) =>
            {
                existingKorisnik.KorisnickoIme = newKorisnik.KorisnickoIme;
                existingKorisnik.Lozinka = passwordHasher.HashPassword(null, updateKorisnikaRequest.Lozinka);
                existingKorisnik.BrojTel = newKorisnik.BrojTel;
                existingKorisnik.Email = newKorisnik.Email;
                existingKorisnik.TipKorisnikaId = newKorisnik.TipKorisnikaId;
                return existingKorisnik;
            });
            var spec = new KorisnikWithTip(korisnikID);
            var korisnik = await korisnikRepo.GetEntityWithSpec(spec);
            return mapper.Map<Korisnik, KorisnikDTO>(korisnik);
        }
        [HttpDelete("{korisnikID}")]
        [Authorize(Roles = "Admin")]
        public async Task<OkResult> DeleteKorisnik(int korisnikID)
        {
            korisnikRepo.Delete(korisnikID);
            return Ok();
        }

        [HttpGet("dostava")]
        [EnableCors("AllowOrigin")]
        public DetaljiPorudzbineDTO GetKorisnikByEmailImePrezime([FromQuery] string email, string ime, string prezime)
        {
            var isExistingKorisnik = dbContext.Korisniks.Where(u => u.Email == email && u.Ime == ime && u.Prezime == prezime).FirstOrDefault();
            if (isExistingKorisnik == null)
                return null;
            else return mapper.Map<Korisnik, DetaljiPorudzbineDTO>(isExistingKorisnik);
        }
    }
}