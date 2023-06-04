using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Entities;
using AutoMapper;
using API.DTOs;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StavkaPorudzbineController : ControllerBase
    {
        private readonly IGenericRepository<StavkaPorudzbine> stavkaPorudzbineRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;
        public StavkaPorudzbineController(IGenericRepository<StavkaPorudzbine> stavkaPorudzbineRepo, IMapper mapper,EProdajaMuzejContext dbContext)
        {
                this.stavkaPorudzbineRepo=stavkaPorudzbineRepo;
                this.mapper=mapper;
                this.dbContext=dbContext;
        }
        [HttpGet]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IReadOnlyList<StavkaPorudzbineDTO>>> GetStavkaPorudzbine()
        {
            var spec = new StavkaPorudzbinaWithDatumKreiranja();
            var stavkePorudzbine= await stavkaPorudzbineRepo.ListAsync(spec);
            return Ok(mapper.Map<IReadOnlyList<StavkaPorudzbine>,IReadOnlyList<StavkaPorudzbineDTO>>(stavkePorudzbine));
        }
        [HttpGet("{stavkaPorudzbineID}/{ulaznicaID}")]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<StavkaPorudzbineDTO>> GetStavkaaPorudzbineByID(int stavkaPorudzbineID,int ulaznicaID)
        {   
            var spec = new StavkaPorudzbinaWithDatumKreiranja(stavkaPorudzbineID,ulaznicaID);
            var stavkaPorudzbine= await stavkaPorudzbineRepo.GetEntityWithSpec(spec);
            return mapper.Map<StavkaPorudzbine,StavkaPorudzbineDTO>(stavkaPorudzbine);
        }
        [HttpPost]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<StavkaPorudzbineDTO>> AddStavkaPorudzbine(StavkaPorudzbine addStavkaPorudzbineRequest)
        {
            
            stavkaPorudzbineRepo.Add(addStavkaPorudzbineRequest);
            var spec = new StavkaPorudzbinaWithDatumKreiranja(addStavkaPorudzbineRequest.StavkaPorudzbineId,addStavkaPorudzbineRequest.UlaznicaId);
            var stavkaPorudzbine = await stavkaPorudzbineRepo.GetEntityWithSpec(spec);
            return mapper.Map<StavkaPorudzbine,StavkaPorudzbineDTO>(stavkaPorudzbine);
        }
        [HttpPut("{stavkaPorudzbineID}/{ulaznicaID}")]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<StavkaPorudzbineDTO>> UpdateStavkaPorudzbine(int stavkaPorudzbineID,int ulaznicaID, StavkaPorudzbine updateStavkaPorudzbineRequest)
        {
            var updateStavkaPorudzbine=await stavkaPorudzbineRepo.Update(updateStavkaPorudzbineRequest,stavkaPorudzbineID,ulaznicaID,(existingStavkaPorudzbine,newStavkaPorudzbine)=>{
                existingStavkaPorudzbine.CenaStavka=newStavkaPorudzbine.CenaStavka;
                existingStavkaPorudzbine.PopustStavka=newStavkaPorudzbine.PopustStavka;
                existingStavkaPorudzbine.UlaznicaId=newStavkaPorudzbine.UlaznicaId;
                existingStavkaPorudzbine.KorpaId=newStavkaPorudzbine.KorpaId;
                existingStavkaPorudzbine.PorudzbinaId=newStavkaPorudzbine.PorudzbinaId;
                return existingStavkaPorudzbine;
            });
            var spec = new StavkaPorudzbinaWithDatumKreiranja(stavkaPorudzbineID,ulaznicaID);
            var stavkaPorudzbine = await stavkaPorudzbineRepo.GetEntityWithSpec(spec);
            return mapper.Map<StavkaPorudzbine,StavkaPorudzbineDTO>(stavkaPorudzbine);
        }
        [HttpDelete("{stavkaPorudzbineID}/{ulaznicaID}")]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")]
        public async Task<OkResult> DeleteStavkaPorudzbine(int stavkaPorudzbineID,int ulaznicaID)
        {
            stavkaPorudzbineRepo.Delete(stavkaPorudzbineID,ulaznicaID);
            return Ok();
        }
    }
}