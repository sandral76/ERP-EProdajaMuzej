using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{   [ApiController]
    [Route("api/[controller]")]
    public class MuzejController : ControllerBase
    {
        private readonly IGenericRepository<Muzej> muzejRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;   

        public MuzejController(IGenericRepository<Muzej> muzejRepo, IMapper mapper,EProdajaMuzejContext dbContext)
        {
                this.muzejRepo=muzejRepo;
                this.mapper=mapper;
                this.dbContext=dbContext;
        }
        [HttpGet]
        [EnableCors("AllowOrigin")] 
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<IReadOnlyList<MuzejDTO>>> GetMuzej()
        {
            var muzeji= await muzejRepo.ListAllAsync();
            return Ok(mapper.Map<IReadOnlyList<Muzej>,IReadOnlyList<MuzejDTO>>(muzeji));
        }
        [HttpGet("{muzejID}")]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<MuzejDTO>> GetMuzejByID(int muzejID){
            var muzej=await muzejRepo.GetByIdAsync(muzejID);
            return mapper.Map<Muzej,MuzejDTO>(muzej);
        }

        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<MuzejDTO>> AddMuzej(Muzej addMuzejRequest)
        {
            
            muzejRepo.Add(addMuzejRequest);
            var muzej= await muzejRepo.GetByIdAsync(addMuzejRequest.MuzejId);
            return mapper.Map<Muzej,MuzejDTO>(muzej);

        } 
        [HttpPut("{muzejID}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<MuzejDTO>> UpdateMuzej(int muzejID, Muzej updateMuzejRequest)
        {
            var updateMuzej=await muzejRepo.Update(updateMuzejRequest,muzejID,(existingMuzej,newMuzej)=>{
                existingMuzej.Naziv=newMuzej.Naziv;
                existingMuzej.Grad=newMuzej.Grad;
                existingMuzej.Ulica=newMuzej.Ulica;
                existingMuzej.BrojTelefona=newMuzej.BrojTelefona;
                existingMuzej.Direktor=newMuzej.Direktor;
                existingMuzej.VebSajt=newMuzej.VebSajt;
                return existingMuzej;
            });
            var muzej= await muzejRepo.GetByIdAsync(muzejID);
            return mapper.Map<Muzej,MuzejDTO>(muzej);
        }
        [HttpDelete("{muzejID}")]
        [Authorize(Roles="Admin")]
        public async Task<OkResult> DeleteMuzej(int muzejID)
        {
            muzejRepo.Delete(muzejID);
            return Ok();
        }
        [HttpGet("grad/{grad?}")]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        [EnableCors("AllowOrigin")] 
        public async Task<ActionResult<IReadOnlyList<MuzejGradDTO>>> GetMuzejByGrad(string? grad)
        {
            var muzeji = await (from m in dbContext.Muzejs
                         where m.Grad== grad
                         select  m).ToListAsync();

            if (grad==null)
            {
                //var muzejs1= await muzejRepo.ListAllAsync();
                var result = await dbContext.Muzejs.GroupBy(m => m.Grad).Select(g => g.First()).ToListAsync();

                return Ok(mapper.Map<IReadOnlyList<Muzej>,IReadOnlyList<MuzejGradDTO>>(result));
            }
            else
            return Ok(mapper.Map<IReadOnlyList<Muzej>,IReadOnlyList<MuzejGradDTO>>(muzeji));
        }
    }
}
        