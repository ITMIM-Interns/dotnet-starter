using Identity.Entity.Enums;
using Identity.Entity.Models;

namespace Identity.BLL.Abstractions.Internals.Repositories
{
    public interface IUserVerificationRepository : IGenericRepository<UserVerification,Guid>
    {
        Task<UserVerification?> GetUserVerificationByType(Guid userId, VerificationType type);
        Task<bool> CheckActiveVerificationCodeAsync(Guid userId,VerificationType type);
    }
}
