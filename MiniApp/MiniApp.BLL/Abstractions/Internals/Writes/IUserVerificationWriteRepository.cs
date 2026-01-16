using MiniApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.BLL.Abstractions.Internals.Writes
{
    public interface IUserVerificationWriteRepository : IGenericWriteRepository<UserVerification,Guid>
    {
    }
}
