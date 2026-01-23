using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.DAL.Data;
using Identity.Entity.Commons;
using Microsoft.EntityFrameworkCore;

namespace Identity.DAL.Internals
{
    public class GenericRepository<TEntity, TKey> :IGenericRepository<TEntity,TKey> where TEntity  : BaseEntity<TKey>,new() where TKey : struct
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, bool hasTracked = false)
        {
            TEntity? entity = new();
            if (hasTracked)
                entity = await _context.Set<TEntity>().FindAsync(id);
            else
                entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
            return entity;
        }
        public Task Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return Task.CompletedTask;
        }

        public Task Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}
