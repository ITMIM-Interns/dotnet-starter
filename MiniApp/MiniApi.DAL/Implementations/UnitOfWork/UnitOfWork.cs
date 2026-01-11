using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.DAL.Implementations.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
