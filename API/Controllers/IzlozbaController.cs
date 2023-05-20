using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using AutoMapper;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IzlozbaController : ControllerBase
    {
        private readonly IGenericRepository<Izlozba> izlozbaRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;   

        public IzlozbaController(IGenericRepository<Izlozba> izlozbaRepo,IMapper mapper,EProdajaMuzejContext dbContext)
        {
                this.izlozbaRepo=izlozbaRepo;
                this.mapper=mapper;
                this.dbContext=dbContext;
        }
        [HttpGet]
        [EnableCors("AllowOrigin")] 
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<IReadOnlyList<IzlozbaDTO>>> GetIzlozba(){

            var izlozbe=await izlozbaRepo.ListAllAsync();
            return Ok(mapper.Map<IReadOnlyList<Izlozba>,IReadOnlyList<IzlozbaDTO>>(izlozbe));
            
        }  
        [HttpGet("{izlozbaID}")]
        [Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<IzlozbaDTO>> GetIzlozbaByID(int izlozbaID){
            var izlozba=await izlozbaRepo.GetByIdAsync(izlozbaID);
            return mapper.Map<Izlozba,IzlozbaDTO>(izlozba);
        }
        [HttpPost]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<IzlozbaDTO>> AddIzlozba(Izlozba addIzlozbaRequest)
        {
            
            izlozbaRepo.Add(addIzlozbaRequest);
            var izlozba= await izlozbaRepo.GetByIdAsync(addIzlozbaRequest.IzlozbaId);
            return mapper.Map<Izlozba,IzlozbaDTO>(izlozba);

        } 
        [HttpPut("{izlozbaID}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult<IzlozbaDTO>> UpdateIzlozba(int izlozbaID, Izlozba updateIzlozbaRequest)
        {
            var updateIzlozba=await izlozbaRepo.Update(updateIzlozbaRequest,izlozbaID,(existingIzlozba,newIzlozba)=>{
                existingIzlozba.Umetnik=newIzlozba.Umetnik;
                existingIzlozba.NazivIzlozbe=newIzlozba.NazivIzlozbe;
                return existingIzlozba;
            });
            var izlozba= await izlozbaRepo.GetByIdAsync(izlozbaID);
            return mapper.Map<Izlozba,IzlozbaDTO>(izlozba);
        }
        [HttpDelete("{izlozbaID}")]
        [Authorize(Roles="Admin")]
        public async Task<OkResult> DeleteIzlozba(int izlozbaID)
        {
            izlozbaRepo.Delete(izlozbaID);
            return Ok();
        }
    }
}