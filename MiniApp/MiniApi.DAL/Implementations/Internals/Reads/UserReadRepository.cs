using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Models;

namespace MiniApp.DAL.Implementations.Internals.Reads
{
    public sealed class UserReadRepository : GenericReadRepository<User,Guid>,IUserReadRepository
    {
        public UserReadRepository(AppDbContext context) : base(context) { }
    }
}
