using System.Linq.Expressions;
using Core.Interfaces;
using Core.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly EProdajaMuzejContext dbContext;
        private readonly DbSet<T> dbSet;

        public GenericRepository(EProdajaMuzejContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>().AsQueryable(), spec);
        }

        public void Add(T addRequest)
        {
            dbContext.Set<T>().Add(addRequest);
            dbContext.SaveChanges();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = dbContext.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public async Task<T> Update(T updateRequest, int id, Func<T, T, T> updateEntity)
        {
            var existingEntity = await dbContext.Set<T>().FindAsync(id);
            var updatedEntity = updateEntity(existingEntity, updateRequest);
            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            await dbContext.SaveChangesAsync();
            return updatedEntity;
        }

        public void Delete(int id)
        {
            var enitityForDelete = dbContext.Set<T>().Find(id);
            dbContext.Set<T>().Remove(enitityForDelete);
            dbContext.SaveChanges();
        }

        public async Task<T> Update(T updateRequest, int id, int id2, Func<T, T, T> updateEntity)
        {
            var existingEntity = await dbContext.Set<T>().FindAsync(id, id2);
            var updatedEntity = updateEntity(existingEntity, updateRequest);
            dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            await dbContext.SaveChangesAsync();
            return updatedEntity;
        }

        public void Delete(int id, int id2)
        {
            var enitityForDelete = dbContext.Set<T>().Find(id, id2);
            dbContext.Set<T>().Remove(enitityForDelete);
            dbContext.SaveChanges();
        }

        public async Task<T> Update(T updateRequest)
        {
            dbContext.Set<T>().Attach(updateRequest);
            dbContext.Entry(updateRequest).State = EntityState.Modified;
            dbContext.SaveChanges();
            return updateRequest;
        }
    }
}