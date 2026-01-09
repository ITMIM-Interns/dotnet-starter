using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Commons;

namespace MiniApp.DAL.Implementations.Internals.Writes
{
    public class GenericWriteRepository<TEntity, TKey> : IGenericWriteRepository<TEntity, TKey> where TEntity : BaseEntity<TKey> where TKey : struct
    {
        protected readonly AppDbContext _context;

        public GenericWriteRepository(AppDbContext context)
        {
            _context = context;
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
