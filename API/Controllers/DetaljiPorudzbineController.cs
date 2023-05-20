using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetaljiPorudzbineController : ControllerBase
    {
        private readonly IGenericRepository<DetaljiPorudzbine> detaljiPorudzbineRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;
        private readonly KorisnikController korisnikController;

        public DetaljiPorudzbineController(IGenericRepository<DetaljiPorudzbine> detaljiPorudzbineRepo, IMapper mapper, EProdajaMuzejContext dbContext, KorisnikController korisnikController)
        {
            this.detaljiPorudzbineRepo = detaljiPorudzbineRepo;
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.korisnikController = korisnikController;
        }
        [HttpGet]
        //[Authorize(Roles="Admin")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IReadOnlyList<DetaljiPorudzbineDTO>>> GetDetaljiPorudzbine()
        {
            var dp = await detaljiPorudzbineRepo.ListAllAsync();
            return Ok(mapper.Map<IReadOnlyList<DetaljiPorudzbine>, IReadOnlyList<DetaljiPorudzbineDTO>>(dp));
        }
        [HttpGet("{dostavaID}")]
        [Authorize(Roles = "Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<DetaljiPorudzbineDTO>> GetDetaljiPorudzbineByID(int dostavaID)
        {

            var dp = await detaljiPorudzbineRepo.GetByIdAsync(dostavaID);
            return mapper.Map<DetaljiPorudzbine, DetaljiPorudzbineDTO>(dp);
        }
        [HttpPost]
        //[Authorize(Roles="Admin")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<DetaljiPorudzbineDTO>> AddDetaljiPorudzbine(DetaljiPorudzbine addDetaljiPorudzbineRequest)
        {

            detaljiPorudzbineRepo.Add(addDetaljiPorudzbineRequest);
            var dp = await detaljiPorudzbineRepo.GetByIdAsync(addDetaljiPorudzbineRequest.DostavaId);
            return mapper.Map<DetaljiPorudzbine, DetaljiPorudzbineDTO>(dp);

        }
        [HttpPut("{dostavaID}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DetaljiPorudzbineDTO>> UpdateDetaljiPorudzbine(int dostavaID, DetaljiPorudzbine updateDetaljiPorudzbineRequest)
        {
            var UpdateDetaljiPorudzbine = await detaljiPorudzbineRepo.Update(updateDetaljiPorudzbineRequest, dostavaID, (existingDetaljiPorudzbine, newDetaljiPorudzbine) =>
            {
                existingDetaljiPorudzbine.EmailDostave = newDetaljiPorudzbine.EmailDostave;
                existingDetaljiPorudzbine.Ime = newDetaljiPorudzbine.Ime;
                existingDetaljiPorudzbine.Prezime = newDetaljiPorudzbine.Prezime;
                existingDetaljiPorudzbine.KontaktTelefon = newDetaljiPorudzbine.KontaktTelefon;
                return existingDetaljiPorudzbine;
            });
            var dp = await detaljiPorudzbineRepo.GetByIdAsync(dostavaID);
            return mapper.Map<DetaljiPorudzbine, DetaljiPorudzbineDTO>(dp);
        }
        [HttpDelete("{dostavaID}")]
        [Authorize(Roles = "Admin")]
        public async Task<OkResult> DeleteDetaljiPorudzbine(int dostavaID)
        {
            detaljiPorudzbineRepo.Delete(dostavaID);
            return Ok();
        }
        /*[HttpGet("dostava")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<DetaljiPorudzbineDTO>> GetKorisnikByDeltaljiPorudzbine(string emailDostave, string ime, string prezime)
        {

            var korisnik = korisnikController.GetKorisnikByEmailImePrezime(emailDostave,ime,prezime);

            if (korisnik != null)
            {
                DetaljiPorudzbineDTO detalji = new DetaljiPorudzbineDTO
                {
                    EmailDostave = korisnik.Email,
                    Ime = korisnik.Ime,
                    Prezime = korisnik.Prezime,
                    KontaktTelefon = korisnik.BrojTel
                };

                return Ok(detalji);
            }
            else
            return NotFound();
        }*/
    }

}