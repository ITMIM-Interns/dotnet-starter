using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.DAL.Data;

namespace Identity.DAL.Internals
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context) => _context = context;


        public async Task<int> SaveAsync()=>await _context.SaveChangesAsync();
       
    }
}
