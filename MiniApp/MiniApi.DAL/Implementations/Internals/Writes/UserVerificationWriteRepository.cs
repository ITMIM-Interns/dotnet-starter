using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.DAL.Implementations.Internals.Writes
{
    public sealed class UserVerificationWriteRepository : GenericWriteRepository<UserVerification, Guid>, IUserVerificationWriteRepository
    {
        public UserVerificationWriteRepository(AppDbContext context) : base(context) { }
        
    }
}
