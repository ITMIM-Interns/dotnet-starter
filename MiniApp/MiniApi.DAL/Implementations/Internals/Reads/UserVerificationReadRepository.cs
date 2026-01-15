using Microsoft.EntityFrameworkCore;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Enums;
using MiniApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.DAL.Implementations.Internals.Reads
{
    public sealed class UserVerificationReadRepository : GenericReadRepository<UserVerification, Guid>, IUserVerificationReadRepository
    {
        public UserVerificationReadRepository(AppDbContext context) : base(context) { }

        public  async Task<UserVerification> GetUserVerificationByType(Guid userId, VerificationType type)
        {
            UserVerification data = await _context.UserVerifications.OrderByDescending(uv => uv.CreatedDate).FirstOrDefaultAsync(uv => uv.Type == type && uv.UserId == userId);
            return data;
        }
    }
}
