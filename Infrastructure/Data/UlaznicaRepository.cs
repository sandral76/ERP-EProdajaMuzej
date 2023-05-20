using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UlaznicaRepository : IUlaznicaRepository
    {
        private readonly EProdajaMuzejContext dbContext;

        public UlaznicaRepository(EProdajaMuzejContext dbContext)
        {
            this.dbContext=dbContext;
        }

        public async Task<Ulaznica> GetUlaznicaByID(int ulaznicaID)
        {
            return await dbContext.Ulaznicas.FindAsync();
        }

        public async Task<IReadOnlyList<Ulaznica>> GetUlaznicas()
        {
        
            //return await dbContext.Ulaznicas.ToListAsync();
            return await dbContext.Ulaznicas.Include(i=>i.Izlozba)
            .ToListAsync();
        }
    }
}