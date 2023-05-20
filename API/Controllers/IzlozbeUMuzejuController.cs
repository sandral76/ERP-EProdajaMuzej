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
    public class IzlozbeUMuzejuController : ControllerBase
    {
        private readonly IGenericRepository<IzlozbaUMuzeju> izlozbaUMuzejuRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;

        public IzlozbeUMuzejuController(IGenericRepository<IzlozbaUMuzeju> izlozbaUMuzejuRepo, IMapper mapper,EProdajaMuzejContext dbContext)
        {
                this.izlozbaUMuzejuRepo=izlozbaUMuzejuRepo;
                this.mapper=mapper;
                this.dbContext=dbContext;
        }

        [HttpGet]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        
        public async Task<ActionResult<IReadOnlyList<IzlozbaUMuzejuDTO>>> GetIzlozbaUMuzeju()
        {
            var spec = new IzlozbeUMuzeju();
            var izlozbeUMuzeju= await izlozbaUMuzejuRepo.ListAsync(spec);
            return Ok(mapper.Map<IReadOnlyList<IzlozbaUMuzeju>,IReadOnlyList<IzlozbaUMuzejuDTO>>(izlozbeUMuzeju));
        }
        [HttpGet("{muzejID}/{izlozbaID}")]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<IzlozbaUMuzejuDTO>> GetIzlozbaUMuzejuByID([FromRoute]int muzejID,[FromRoute]int izlozbaID)
        {   
            var spec = new IzlozbeUMuzeju(muzejID,izlozbaID);
            var izlozbaUMuzeju= await izlozbaUMuzejuRepo.GetEntityWithSpec(spec);
            return mapper.Map<IzlozbaUMuzeju,IzlozbaUMuzejuDTO>(izlozbaUMuzeju);
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<IzlozbaUMuzejuDTO>> AddIzlozbaUMuzeju(IzlozbaUMuzeju addIzlozbaUMuzejuRequest)
        {
            
            izlozbaUMuzejuRepo.Add(addIzlozbaUMuzejuRequest);
            var spec = new IzlozbeUMuzeju(addIzlozbaUMuzejuRequest.MuzejId,addIzlozbaUMuzejuRequest.IzlozbaId);
            var izlozbaUMuzeju = await izlozbaUMuzejuRepo.GetEntityWithSpec(spec);
            return mapper.Map<IzlozbaUMuzeju,IzlozbaUMuzejuDTO>(izlozbaUMuzeju);

        }
        [HttpPut("{muzejID}/{izlozbaID}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<IzlozbaUMuzejuDTO>> UpdateIzlozbaUMuzeju([FromRoute]int muzejID,[FromRoute] int izlozbaID, IzlozbaUMuzeju updateIzlozbaUMuzejuRequest)
        {
            var updateizlozbaUMuzeju=await izlozbaUMuzejuRepo.Update(updateIzlozbaUMuzejuRequest,muzejID,izlozbaID,(existingizlozbaUMuzeju,newizlozbaUMuzeju)=>{
                existingizlozbaUMuzeju.DatumOdrzavanja=newizlozbaUMuzeju.DatumOdrzavanja;
                existingizlozbaUMuzeju.Galerija=newizlozbaUMuzeju.Galerija;
                return existingizlozbaUMuzeju;
            });
            var spec = new IzlozbeUMuzeju(muzejID,izlozbaID);
            var izlozbaUMuzeju = await izlozbaUMuzejuRepo.GetEntityWithSpec(spec);
            return mapper.Map<IzlozbaUMuzeju,IzlozbaUMuzejuDTO>(izlozbaUMuzeju);
        }

        [HttpDelete("{muzejID}/{izlozbaID}")]
        [Authorize(Roles="Admin")]
        public async Task<OkResult> DeleteUlaznica(int muzejID, int izlozbaID)
        {
            izlozbaUMuzejuRepo.Delete(muzejID,izlozbaID);
            return Ok();
        }
    }
}