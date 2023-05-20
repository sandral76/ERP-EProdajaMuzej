using Core.Entities;
using Core.Specification;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        void Add(T addRequest);
        Task<T> Update(T updateRequest,int id,Func<T,T,T> update);
        Task<T> Update(T updateRequest,int id,int id2,Func<T,T,T> update);
        void Delete(int id);
        void Delete(int id,int id2);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}