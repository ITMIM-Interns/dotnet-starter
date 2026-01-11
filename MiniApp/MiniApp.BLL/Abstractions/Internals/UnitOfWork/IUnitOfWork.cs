using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.BLL.Abstractions.Internals.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}
