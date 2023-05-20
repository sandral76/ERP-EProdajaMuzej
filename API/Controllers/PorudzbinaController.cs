using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Entities;
using AutoMapper;
using API.DTOs;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using API.Errors;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PorudzbinaController : ControllerBase
    {
        private readonly IGenericRepository<Porudzbina> porudzbinaRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;

        public PorudzbinaController(IGenericRepository<Porudzbina> porudzbinaRepo, IMapper mapper, EProdajaMuzejContext dbContext)
        {
            this.porudzbinaRepo = porudzbinaRepo;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }
        [HttpGet]
        //[Authorize(Roles="Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IReadOnlyList<PorudzbinaDTO>>> GetPorudzbina()
        {
            var p=dbContext.Porudzbinas.ToList();
            //return Ok(p);
            /*var spec = new PorudzbinaWithDostavaAndKorisnik();
            var porudzbine= await porudzbinaRepo.ListAsync(spec);*/
            return Ok(mapper.Map<IReadOnlyList<Porudzbina>,IReadOnlyList<PorudzbinaDTO>>(p));
        }
        [HttpGet("{porudzbinaID}")]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<PorudzbinaDTO>> GetPorudzbinaByID(int porudzbinaID)
        {
            var spec = new PorudzbinaWithDostavaAndKorisnik(porudzbinaID);
            var porudzbina = await porudzbinaRepo.GetEntityWithSpec(spec);
            return mapper.Map<Porudzbina, PorudzbinaDTO>(porudzbina);
        }
        [HttpPost]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<PorudzbinaDTO>> AddPorudzbina(Porudzbina addPorudzbinaRequest)
        {

            porudzbinaRepo.Add(addPorudzbinaRequest);
            //var spec = new PorudzbinaWithDostavaAndKorisnik(addPorudzbinaRequest.PorudzbinaId);
            //var porudzbina = await porudzbinaRepo.GetEntityWithSpec(spec);
            return Ok(mapper.Map<Porudzbina, PorudzbinaDTO>(addPorudzbinaRequest));
        }
        [HttpPut("{porudzbinaID}")]
        [Authorize(Roles = "Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<PorudzbinaDTO>> UpdatePorudzbina(int porudzbinaID, Porudzbina updatePorudzbinaRequest)
        {
            var updatePorudzbina = await porudzbinaRepo.Update(updatePorudzbinaRequest, porudzbinaID, (existingPorudzbina, newPorudzbina) =>
            {
                existingPorudzbina.DatumKreiranja = newPorudzbina.DatumKreiranja;
                existingPorudzbina.StatusPorudzbine = newPorudzbina.StatusPorudzbine;
                existingPorudzbina.IznosPorudzbine = newPorudzbina.IznosPorudzbine;
                existingPorudzbina.PopustNaPorudzbinu = newPorudzbina.PopustNaPorudzbinu;
                existingPorudzbina.DatumAzuriranja = newPorudzbina.DatumAzuriranja;
                existingPorudzbina.DostavaId = newPorudzbina.DostavaId;
                existingPorudzbina.KorisnikId = newPorudzbina.KorisnikId;
                return existingPorudzbina;
            });
            var spec = new PorudzbinaWithDostavaAndKorisnik(porudzbinaID);
            var porudzbina = await porudzbinaRepo.GetEntityWithSpec(spec);
            return mapper.Map<Porudzbina, PorudzbinaDTO>(porudzbina);
        }
        [HttpDelete("{porudzbinaID}")]
        [Authorize(Roles = "Admin,Registrovani korisnik,Super korisnik")]
        public async Task<OkResult> DeleteUlaznica(int porudzbinaID)
        {
            porudzbinaRepo.Delete(porudzbinaID);
            return Ok();
        }
    }
}