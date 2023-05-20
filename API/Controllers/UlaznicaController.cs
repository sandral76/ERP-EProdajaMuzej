using Infrastructure.Data;
using Core.Entities;
//using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specification;
using API.DTOs;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using API.Helepers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UlaznicaController : ControllerBase
    {
        private readonly IGenericRepository<Ulaznica> ulaznicaRepo;
        private readonly IMapper mapper;
        private readonly EProdajaMuzejContext dbContext;
        /*public UlaznicaController(IUlaznicaRepository repo)  //primer DI
        {
            this.repo=repo;
        }*/
        public UlaznicaController(IGenericRepository<Ulaznica> ulaznicaRepo, IMapper mapper, EProdajaMuzejContext dbContext)
        {
            this.ulaznicaRepo = ulaznicaRepo;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        [HttpGet]
        //[Route("GetAllAuthor")]  
        [EnableCors("AllowOrigin")]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<Pagination<UlaznicaDTO>>> GetUlaznice([FromQuery] UlazniceSpecParams parameters)
        {
            var spec = new UlazniceOnIzlozbas(parameters);
            var countSpec = new UlazniceWithFiltersForCountSpecification(parameters);
            var totalItems = await ulaznicaRepo.CountAsync(countSpec);

            var ulaznice = await ulaznicaRepo.ListAsync(spec);
            var data = mapper.Map<IReadOnlyList<Ulaznica>, IReadOnlyList<UlaznicaDTO>>(ulaznice);
            //return Ok(mapper.Map<IReadOnlyList<Ulaznica>,IReadOnlyList<UlaznicaDTO>>(ulaznice));
            return Ok(new Pagination<UlaznicaDTO>(parameters.PageIndex, parameters.PageSize, totalItems, data));

        }

        [HttpGet("{ulaznicaID}")]
        //[Authorize(Roles="Admin,Registrovani korisnik,Super korisnik")]
        public async Task<ActionResult<UlaznicaDTO>> GetUlaznicaByID(int ulaznicaID)
        {
            var spec = new UlazniceOnIzlozbas(ulaznicaID);
            var ulaznica = await ulaznicaRepo.GetEntityWithSpec(spec);
            if (ulaznica == null) return NotFound(new ApiResponses(404));
            return mapper.Map<Ulaznica, UlaznicaDTO>(ulaznica);
        }

        [HttpPost]
        //[Authorize(Roles="Admin")]
        public async Task<ActionResult<UlaznicaDTO>> AddUlaznica(Ulaznica addUlaznicaRequest)
        {
            try
            {
                ulaznicaRepo.Add(addUlaznicaRequest);
                var spec = new UlazniceOnIzlozbas(addUlaznicaRequest.UlaznicaId);
                var ulaznica = await ulaznicaRepo.GetEntityWithSpec(spec);
                return mapper.Map<Ulaznica, UlaznicaDTO>(ulaznica);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }


        }

        [HttpPut("{ulaznicaID}")]
        //[Authorize(Roles="Admin")]
        public async Task<ActionResult<UlaznicaDTO>> UpdateUlaznica(int ulaznicaID, Ulaznica updateUlaznicaRequest)
        {
            var updateUlaznica = await ulaznicaRepo.Update(updateUlaznicaRequest, ulaznicaID, (existingUlaznica, newUlaznica) =>
            {
                existingUlaznica.CenaUlaznice = newUlaznica.CenaUlaznice;
                existingUlaznica.IzlozbaId = newUlaznica.IzlozbaId;
                return existingUlaznica;
            });
            var spec = new UlazniceOnIzlozbas(ulaznicaID);
            var ulaznica = await ulaznicaRepo.GetEntityWithSpec(spec);
            return mapper.Map<Ulaznica, UlaznicaDTO>(ulaznica);
        }

        [HttpDelete("{ulaznicaID}")]
        //[Authorize(Roles="Admin")]
        [EnableCors("AllowOrigin")]
        public async Task<OkResult> DeleteUlaznica(int ulaznicaID)
        {
            ulaznicaRepo.Delete(ulaznicaID);
            return Ok();
        }
        [HttpGet("grad/{g?}")]
        [EnableCors("AllowOrigin")]
        public async Task<ActionResult<IReadOnlyList<UlaznicaDTO>>> GetUlaznicaByGradMuzeja(string? g, [FromQuery] UlazniceSpecParams? parameters)
        {   
            
            var result = await (from u in dbContext.Ulaznicas
                                join im in dbContext.IzlozbaUMuzejus on u.IzlozbaId equals im.IzlozbaId
                                join m in dbContext.Muzejs on im.MuzejId equals m.MuzejId
                                where m.Grad == g
                                select u).ToListAsync();
            if (g == null)
            {
                var spec = new UlazniceOnIzlozbas(parameters);
                var countSpec = new UlazniceWithFiltersForCountSpecification(parameters);
                var totalItems = await ulaznicaRepo.CountAsync(countSpec);

                var ulaznice = await ulaznicaRepo.ListAsync(spec);
                var data = mapper.Map<IReadOnlyList<Ulaznica>, IReadOnlyList<UlaznicaDTO>>(ulaznice);

                //return Ok(mapper.Map<IReadOnlyList<Ulaznica>, IReadOnlyList<UlaznicaDTO>>(ulaznice));
                return Ok(new Pagination<UlaznicaDTO>(parameters.PageIndex, parameters.PageSize, totalItems, data));
            }
            else
            {
                var spec = new UlazniceOnIzlozbas(result,g,parameters);
                int totalItems=result.Count();
                var u = await ulaznicaRepo.ListAsync(spec);
                
                var data = mapper.Map<IReadOnlyList<Ulaznica>, IReadOnlyList<UlaznicaDTO>>(u);
                //return Ok(mapper.Map<IReadOnlyList<Ulaznica>, IReadOnlyList<UlaznicaDTO>>(u));
                return Ok(new Pagination<UlaznicaDTO>(parameters.PageIndex, parameters.PageSize, totalItems, data));
            }

        }
    }
}