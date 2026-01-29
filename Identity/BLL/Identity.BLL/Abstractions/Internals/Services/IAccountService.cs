using Identity.DTO.Accounts;
using Identity.DTO.Users;

namespace Identity.BLL.Abstractions.Internals.Services
{
    public interface IAccountService
    {
        Task<bool> ForgetPassword(ForgetPasswordDto dto);
        Task<bool> ConfirmEmail(ConfirmEmailDto dto);
        Task<Guid> SendEmailVerificationCode(string email);
        Task<bool> UserActive(Guid id);
        Task<bool> UserDeactive(Guid id);
        Task<Guid> Register(CreateUserDto request);
        Task<bool> LoginAsync(LoginDto dto);
        Task<bool> LogoutAsync();
    }
}
