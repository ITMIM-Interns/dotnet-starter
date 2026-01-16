using MiniApp.Models.Enums;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Abstractions.Internals.Reads
{
    public interface IUserVerificationReadRepository : IGenericReadRepository<UserVerification,Guid>
    {
        Task<UserVerification> GetUserVerificationByType(Guid userId, VerificationType type);
        Task<bool> CheckActiveVerificationCodeAsync(Guid userId,VerificationType type);
    }
}
