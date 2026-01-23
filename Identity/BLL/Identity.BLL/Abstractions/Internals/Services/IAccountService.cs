using Identity.DTO.Accounts;

namespace Identity.BLL.Abstractions.Internals.Services
{
    public interface IAccountService
    {
        Task ForgetPassword(ForgetPasswordDto dto);
        Task<bool> ConfirmEmail(ConfirmEmailDto dto);
        Task<bool> SendEmailVerificationCode(Guid id);
        Task<bool> UserActive(Guid id);
        Task<bool> UserDeactive(Guid id);
        Task<string> LoginAsync(LoginDto dto);
    }
}
