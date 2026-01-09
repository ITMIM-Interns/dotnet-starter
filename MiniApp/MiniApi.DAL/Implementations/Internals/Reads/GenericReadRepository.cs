using Microsoft.EntityFrameworkCore;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Commons;

namespace MiniApp.DAL.Implementations.Internals.Reads
{
    public class GenericReadRepository<TEntity, Tkey> : IGenericReadRepository<TEntity, Tkey>
                                                where TEntity : BaseEntity<Tkey>,new() where Tkey : struct
    {
        protected readonly AppDbContext _context;
        public GenericReadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id, bool hasTracked=false)
        {
            TEntity entity=new();
            if (hasTracked)
                entity = await _context.Set<TEntity>().FindAsync(id);
            else
                entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e=>e.Id.Equals(id));
            return entity;
        }
    }
}
