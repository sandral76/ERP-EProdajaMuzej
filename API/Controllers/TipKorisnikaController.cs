using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipKorisnikaController : ControllerBase
    {
        private readonly IGenericRepository<TipKorisnika> tipKorisnikaRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;

        public TipKorisnikaController(IGenericRepository<TipKorisnika> tipKorisnikaRepo, IMapper mapper, EProdajaMuzejContext dbContext)
        {
            this.tipKorisnikaRepo = tipKorisnikaRepo;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }
        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<IReadOnlyList<TipKorisnikaDTO>>> GetTipKorisnika()
        {
                var tipoviKorisnika = await tipKorisnikaRepo.ListAllAsync();
                return Ok(mapper.Map<IReadOnlyList<TipKorisnika>, IReadOnlyList<TipKorisnikaDTO>>(tipoviKorisnika));

        }
        [HttpGet("{tipKorisnikaID}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<TipKorisnikaDTO>> GetTipKorisnikaByID(int tipKorisnikaID)
        {
            
            var tipKorisnika = await tipKorisnikaRepo.GetByIdAsync(tipKorisnikaID);
            return mapper.Map<TipKorisnika, TipKorisnikaDTO>(tipKorisnika);
          
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<TipKorisnikaDTO>> AddTipKorisnika(TipKorisnika addTipKorisnikaRequest)
        {

            var isExistingTipKorisnika= dbContext.TipKorisnikas.Where(u=> u.TipKorisnikaId == addTipKorisnikaRequest.TipKorisnikaId).FirstOrDefault();
            var korisnikUloga= dbContext.TipKorisnikas.Where(u=>u.Uloga == addTipKorisnikaRequest.Uloga).FirstOrDefault();

            if(korisnikUloga.Uloga == addTipKorisnikaRequest.Uloga)
                return BadRequest("Tip korisnika koji pokusavate da dodate vec postoji.");
            if((isExistingTipKorisnika.TipKorisnikaId == addTipKorisnikaRequest.TipKorisnikaId) &&(isExistingTipKorisnika.Uloga == addTipKorisnikaRequest.Uloga))
                return BadRequest("Tip korisnika koji pokusavate da dodate vec postoji.");
            
            tipKorisnikaRepo.Add(addTipKorisnikaRequest);
            var tipKorisnika = await tipKorisnikaRepo.GetByIdAsync(addTipKorisnikaRequest.TipKorisnikaId);
            return mapper.Map<TipKorisnika, TipKorisnikaDTO>(tipKorisnika);

        }
        [HttpPut("{tipKorisnikaID}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<TipKorisnikaDTO>> UpdateTipKorisnika(int tipKorisnikaID, TipKorisnika updateTipKorisnikaRequest)
        {
            var isExistingTipKorisnika= dbContext.TipKorisnikas.Where(u=> u.TipKorisnikaId == updateTipKorisnikaRequest.TipKorisnikaId).FirstOrDefault();
            var korisnikUloga= dbContext.TipKorisnikas.Where(u=>u.Uloga == updateTipKorisnikaRequest.Uloga).FirstOrDefault();

            if(korisnikUloga.Uloga == updateTipKorisnikaRequest.Uloga)
                return BadRequest("Tip korisnika koji pokusavate da dodate vec postoji.");
            if((isExistingTipKorisnika.TipKorisnikaId == updateTipKorisnikaRequest.TipKorisnikaId) &&(isExistingTipKorisnika.Uloga == updateTipKorisnikaRequest.Uloga))
                return BadRequest("Tip korisnika koji pokusavate da dodate vec postoji.");
            var updateTipKorisnika = await tipKorisnikaRepo.Update(updateTipKorisnikaRequest, tipKorisnikaID, (existingTipKorisnika, newTipKorisnika) =>
            {
                existingTipKorisnika.Uloga = newTipKorisnika.Uloga;
                return existingTipKorisnika;
            });
            var tipKorisnika = await tipKorisnikaRepo.GetByIdAsync(tipKorisnikaID);
            return mapper.Map<TipKorisnika, TipKorisnikaDTO>(tipKorisnika);
        }
        [HttpDelete("{tipKorisnikaID}")]
        [Authorize(Roles="Admin")]
        public async Task<OkResult> DeleteTipKorisnika(int tipKorisnikaID)
        {
            tipKorisnikaRepo.Delete(tipKorisnikaID);
            return Ok();
        }
    }
}