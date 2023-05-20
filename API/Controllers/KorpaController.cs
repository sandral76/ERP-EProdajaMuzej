using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KorpaController : ControllerBase
    {
        private readonly IGenericRepository<Korpa> korpaRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;   

        public KorpaController(IGenericRepository<Korpa> korpaRepo, IMapper mapper,EProdajaMuzejContext dbContext)
        {
                this.korpaRepo=korpaRepo;
                this.mapper=mapper;
                this.dbContext=dbContext;
        }
        [HttpGet]
        //[Authorize(Roles="Admin")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IReadOnlyList<KorpaDTO>>> GetKorpa()
        {
            /*var korpe= await korpaRepo.ListAllAsync();
            return Ok(mapper.Map<IReadOnlyList<Korpa>,IReadOnlyList<KorpaDTO>>(korpe));*/
            var spec = new KorpaWithStavkaPorudzbine();
            var korpe= await korpaRepo.ListAsync(spec);
            //return Ok(mapper.Map<IReadOnlyList<Korpa>,IReadOnlyList<KorpaDTO>>(korpe));
            //return Ok(korpe);
             
             /*var korpeDto = korpe.Select(k => new KorpaDTO
             {
                KorpaId = k.KorpaId,
                BrojUlaznica = k.BrojUlaznica,
                
                StavkaPorudzbines = k.StavkaPorudzbines.Select(sp => 
                new StavkaPorudzbineDTO {
                    StavkaPorudzbineId = sp.StavkaPorudzbineId,
                    CenaStavka = sp.CenaStavka,
                }
                
                ).ToList(),
            }).ToList();*/
            
            return Ok(mapper.Map<IReadOnlyList<Korpa>,IReadOnlyList<KorpaDTO>>(korpe));
            
        }
        [HttpGet("{korpaID}")]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")] 
        public async Task<ActionResult<KorpaDTO>> GetKorpaByID(int korpaID){
            var spec = new KorpaWithStavkaPorudzbine(korpaID);
            var korpa=await korpaRepo.GetEntityWithSpec(spec);
            return mapper.Map<Korpa,KorpaDTO>(korpa);
        }
        [HttpPost]
        //[Authorize(Roles="Admin")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<KorpaDTO>> AddKorpa(Korpa addKorpaRequest)
        {
            
            korpaRepo.Add(addKorpaRequest);
            var korpa= await korpaRepo.GetByIdAsync(addKorpaRequest.KorpaId);
            return mapper.Map<Korpa,KorpaDTO>(korpa);

        } 
        [HttpPut("{korpaID}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<KorpaDTO>> UpdateKorpa(int korpaID, Korpa updateKorpaRequest)
        {
            var updateKorpa=await korpaRepo.Update(updateKorpaRequest,korpaID,(existingKorpa,newKorpa)=>{
                existingKorpa.BrojUlaznica=newKorpa.BrojUlaznica;
                existingKorpa.UkupanIznos=newKorpa.UkupanIznos;;
                return existingKorpa;
            });
            var korpa= await korpaRepo.GetByIdAsync(korpaID);
            return mapper.Map<Korpa,KorpaDTO>(korpa);
        }
        [HttpDelete("{korpaID}")]
        //[Authorize(Roles="Admin")]
        [EnableCors("AllowOrigin")]
        public async Task<OkResult> DeleteKorpa(int korpaID)
        {
            korpaRepo.Delete(korpaID);
            return Ok();
        }


        [HttpGet("brojUlaznica/{korpaID}")]
        [EnableCors("AllowOrigin")]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<KorpaBrojUlaznicaDTO>> GetKorpasBrojUlaznica(int korpaID){
            var spec = new KorpaWithStavkaPorudzbine(korpaID);
            var korpa=await korpaRepo.GetEntityWithSpec(spec);
            return mapper.Map<Korpa,KorpaBrojUlaznicaDTO>(korpa);
        }
    }
}