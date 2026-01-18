using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.BLL.Abstractions.Internals.Services;
using Identity.BLL.Exceptions.Commons;
using Identity.BLL.Exceptions.Users;
using Identity.BLL.Helpers;
using Identity.DTO.Accounts;
using Identity.Entity.Enums;
using Identity.Entity.Models;

namespace Identity.BLL.ServiceImplementation
{
    public sealed class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserVerificationRepository _userVerificationRepo;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUserRepository userRepo, IUserVerificationRepository userVerificationRepo, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _userRepo = userRepo;
            _userVerificationRepo = userVerificationRepo;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ConfirmEmail(ConfirmEmailDto dto)
        {
            UserVerification? existVerification = await _userVerificationRepo.GetUserVerificationByType(dto.userId, VerificationType.EmailConfirm);
            if (existVerification is null)
                throw new VerificationNotFoundException(ExceptionMessage.VerificationNotFoundMessage);
            if (existVerification.Status is VerificationStatus.Success)
                throw new VerificationAlreadyConfirmed(ExceptionMessage.VerificationConfirmedMessage);
            if (existVerification.Code != dto.code)
                throw new InvalidVerificationCodeException(ExceptionMessage.InvalidVerificationCodeMessage);
            if (existVerification.ExpiresAt <= DateTimeOffset.UtcNow)
            {
                existVerification.Status = VerificationStatus.Expired;
                throw new VerificationCodeExpiredException(ExceptionMessage.InvalidExpiresTimeMessage);
            }
            existVerification.Status = VerificationStatus.Success;
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task ForgetPassword(ForgetPasswordDto dto)
        {
            User? existUser = await _userRepo.FindByEmailAsync(dto.Email);
            if (existUser is null)
                return;
            bool hasActiveCode = await _userVerificationRepo.CheckActiveVerificationCodeAsync(existUser.Id, VerificationType.PasswordReset);
            if (hasActiveCode)
                throw new VerificationCodeExpiredException(ExceptionMessage.ValidVerificationCode);
            string resetCode = SecurityService.GenerateVerificationCode();
            UserVerification newVerification = new()
            {
                Code = resetCode,
                Type = VerificationType.PasswordReset,
                UserId = existUser.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                Status = VerificationStatus.Active,
            };
            await _userVerificationRepo.Add(newVerification);
            await _emailService.SendAsync(dto.Email, resetCode, "Reset verification code");
            await _unitOfWork.SaveAsync();
            return;
        }

        public async Task<bool> SendEmailVerificationCode(Guid id)
        {
            User? existUser = await _userRepo.GetByIdAsync(id);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            bool hasCode = await _userVerificationRepo.CheckActiveVerificationCodeAsync(id, VerificationType.EmailConfirm);
            if (hasCode)
                throw new InvalidAccountException(ExceptionMessage.InvalidVerificationCodeMessage);
            string newCode = SecurityService.GenerateVerificationCode();
            await _emailService.SendAsync(existUser.Email, newCode, "Verification Code");
            UserVerification newVerification = new UserVerification
            {
                Code = newCode,
                UserId = id,
                Type = VerificationType.EmailConfirm,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5),
                Status = VerificationStatus.Active
            };
            await _userVerificationRepo.Add(newVerification);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> UserActive(Guid id)
        {
            User? existUser = await _userRepo.GetByIdAsync(id, true);
            if (existUser is null) throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if (existUser.IsActive)
                return true;
            existUser.IsActive = true;
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> UserDeactive(Guid id)
        {
            User? existUser = await _userRepo.GetByIdAsync(id, true);
            if (existUser is null) throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if (!existUser.IsActive)
                return true;
            existUser.IsActive = false;
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}
