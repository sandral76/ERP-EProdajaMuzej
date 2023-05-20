using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUlaznicaRepository
    {
        Task<Ulaznica> GetUlaznicaByID(int ulaznicaID);
        Task<IReadOnlyList<Ulaznica>> GetUlaznicas();

    }
}