using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.DataAccess.Data;
using Identity.Entity.Enums;
using Identity.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.DAL.Internals
{
    public sealed class UserVerificationRepository : GenericRepository<UserVerification, Guid>, IUserVerificationRepository
    {
        public UserVerificationRepository(AppDbContext context) : base(context) { }

        public async Task<bool> CheckActiveVerificationCodeAsync(Guid userId, VerificationType type) =>
            await _context.UserVerifications.AnyAsync(x => x.UserId == userId && x.Status == VerificationStatus.Active && x.Type == type && x.ExpiresAt > DateTimeOffset.UtcNow);


        public async Task<UserVerification?> GetUserVerificationByType(Guid userId, VerificationType type)=>
            await _context.UserVerifications.OrderByDescending(uv => uv.CreatedDate).FirstOrDefaultAsync(uv => uv.Type == type && uv.UserId == userId);
         
    }
}
