using Microsoft.EntityFrameworkCore;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.BLL.Helpers;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Enums;
using MiniApp.Models.Models;

namespace MiniApp.DAL.Implementations.Internals.Reads
{
    public sealed class UserVerificationReadRepository : GenericReadRepository<UserVerification, Guid>, IUserVerificationReadRepository
    {
        public UserVerificationReadRepository(AppDbContext context) : base(context) { }

        public async Task<bool> CheckActiveVerificationCodeAsync(Guid userId, VerificationType type)=>
            await _context.UserVerifications.AnyAsync(x =>x.UserId == userId && !x.IsUsed && x.Type == type && x.ExpiresAt > DateTimeOffset.UtcNow);
            

        public  async Task<UserVerification> GetUserVerificationByType(Guid userId, VerificationType type)
        {
            UserVerification data = await _context.UserVerifications.OrderByDescending(uv => uv.CreatedDate).FirstOrDefaultAsync(uv => uv.Type == type && uv.UserId == userId);
            return data;
        }

       
    }
}
