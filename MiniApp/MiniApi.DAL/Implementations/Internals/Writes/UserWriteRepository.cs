using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Models;

namespace MiniApp.DAL.Implementations.Internals.Writes
{
    public sealed class UserWriteRepository : GenericWriteRepository<User, Guid>, IUserWriteRepository
    {
        public UserWriteRepository(AppDbContext context) : base(context) { }
    }
}
