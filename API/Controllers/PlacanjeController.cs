using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Entities;
using AutoMapper;
using API.DTOs;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class PlacanjeController : ControllerBase
    {
        private readonly IGenericRepository<Placanje> placanjeRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;
        public PlacanjeController(IGenericRepository<Placanje> placanjeRepo, IMapper mapper,EProdajaMuzejContext dbContext)
        {
                this.placanjeRepo=placanjeRepo;
                this.mapper=mapper;
                this.dbContext=dbContext;
        }
        [HttpGet]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<IReadOnlyList<PlacanjeDTO>>> GetPlacanje()
        {
            var spec = new PlacanjeWithIznos();
            var placanja= await placanjeRepo.ListAsync(spec);
            return Ok(mapper.Map<IReadOnlyList<Placanje>,IReadOnlyList<PlacanjeDTO>>(placanja));
        }
        [HttpGet("{placanjeID}")]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<PlacanjeDTO>> GetPlacanjeByID(int placanjeID)
        {   
            var spec = new PlacanjeWithIznos(placanjeID);
            var placanja= await placanjeRepo.GetEntityWithSpec(spec);
            return mapper.Map<Placanje,PlacanjeDTO>(placanja);
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<PlacanjeDTO>> AddPlacanje(Placanje addPlacanjeRequest)
        {
            
            placanjeRepo.Add(addPlacanjeRequest);
            var spec = new PlacanjeWithIznos(addPlacanjeRequest.PlacanjeId);
            var placanje = await placanjeRepo.GetEntityWithSpec(spec);
            return mapper.Map<Placanje,PlacanjeDTO>(placanje);
        }
        [HttpPut("{placanjeID}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<PlacanjeDTO>> UpdatePlacanje(int placanjeID, Placanje updatePlacanjeRequest)
        {
            var updatePlacanje=await placanjeRepo.Update(updatePlacanjeRequest,placanjeID,(existingPlacanje,newPlacanje)=>{
                existingPlacanje.DatumPlacanja=newPlacanje.DatumPlacanja;
                existingPlacanje.TipPlacanja=newPlacanje.TipPlacanja;
                existingPlacanje.BrojRacuna=newPlacanje.BrojRacuna;
                existingPlacanje.VlasnikKartice=newPlacanje.VlasnikKartice;
                existingPlacanje.PorudzbinaId=newPlacanje.PorudzbinaId;
                
                return existingPlacanje;
            });
            var spec = new PlacanjeWithIznos(placanjeID);
            var placanje = await placanjeRepo.GetEntityWithSpec(spec);
            return mapper.Map<Placanje,PlacanjeDTO>(placanje);
        }
        [HttpDelete("{placanjeID}")]
        [Authorize(Roles="Admin")]
        public async Task<OkResult> DeletePlacanje(int placanjeID)
        {
            placanjeRepo.Delete(placanjeID);
            return Ok();
        }
    }
}