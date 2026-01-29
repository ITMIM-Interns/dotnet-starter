using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.BLL.Abstractions.Internals.Services;
using Identity.BLL.Exceptions.Commons;
using Identity.BLL.Exceptions.Users;
using Identity.BLL.Helpers;
using Identity.DTO.Accounts;
using Identity.DTO.Users;
using Identity.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Identity.BLL.ServiceImplementation
{
    public sealed class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        public AccountService(IUserRepository userRepo, IEmailService emailService, IUnitOfWork unitOfWork, ITokenService tokenService, ICacheService cacheService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IFileService fileService)
        {
            _userRepo = userRepo;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _cacheService = cacheService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _fileService = fileService;
        }

        public async Task<Guid> Register(CreateUserDto request)
        {
            if (await _userRepo.EmailExistsAsync(request.Email))
                throw new ExistUserFieldException(ExceptionMessage.ExistUserEmailMessage);
            if (await _userRepo.UserNameExistsAsync(request.Username))
                throw new ExistUserFieldException(ExceptionMessage.ExistUsernameMessage);
            byte[] salt = SecurityService.GenerateRandomNumber(16);
            string hashedPassword = SecurityService.PasswordHash(request.Password, salt);
            User newUser = new User()
            {
                Username = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                Salt = Convert.ToBase64String(salt),
                ContactNumber = request.ContactNumber,
                IsActive = false,
                IsConfirmed = false,
            };
            if (request.Image is not null)
                newUser.Image = await _fileService.UploadFileAsync(request.Image, "user-image");
            await _userRepo.Add(newUser);
            await _unitOfWork.SaveAsync();
            string code = SecurityService.GenerateVerificationCode();
            await _cacheService.SetAsync($"user-email-verification-{newUser.Id}", code, TimeSpan.FromMinutes(2));
            await _emailService.SendAsync(request.Email, code, "Email confrimation");
            return newUser.Id;
        }
        public async Task<bool> ConfirmEmail(ConfirmEmailDto dto)
        {
            string? code = await _cacheService.GetAsync<string>($"user-email-verification-{dto.UserId}");
            if (code is null)
                throw new VerificationNotFoundException(ExceptionMessage.VerificationNotFoundMessage);
            if (code != dto.Code)
                throw new InvalidVerificationCodeException(ExceptionMessage.InvalidVerificationCodeMessage);
            User? existUser = await _userRepo.GetByIdAsync(dto.UserId, true);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            existUser.IsActive = true;
            existUser.IsConfirmed = true;
            int rowCount = await _unitOfWork.SaveAsync();
            await _cacheService.RemoveAsync($"user-email-verification-{existUser.Id}");
            return rowCount > 0;
        }
        public async Task<bool> LoginAsync(LoginDto dto)
        {
            User? existUser = await _userRepo.FindByEmailAsync(dto.Email);
            if (existUser is null)
                throw new InvalidAccountException(ExceptionMessage.InvalidLoginMessage);
            if (existUser.IsActive is false)
                throw new InvalidAccountException(ExceptionMessage.AccountNotActiveMessage);
            bool isCorrect = await _userRepo.CheckUserPasswordAsync(existUser, dto.Password);
            if (!isCorrect)
                throw new InvalidAccountException(ExceptionMessage.InvalidLoginMessage);
            if (await _cacheService.GetAsync<string>("refresh-token") is not null)
                await _cacheService.RemoveAsync("refresh-token");
            await _cacheService.SetAsync("refresh-token", Convert.ToBase64String(SecurityService.GenerateRandomNumber(64)), TimeSpan.FromDays(_configuration.GetValue<int>("JwtSettings:RefreshTokenExpireDay")));
            string token = _tokenService.CreateAccessToken(existUser);
             _httpContextAccessor.HttpContext?.Response.Cookies.Append("access-token", token, new CookieOptions
             {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:ExpireAt")),
             });
            return true;
        }
        public async Task<bool> LogoutAsync()
        {
            await _cacheService.RemoveAsync("refresh-token");
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("access-token");
            return true;
        }
        public async Task<bool> ForgetPassword(ForgetPasswordDto dto)
        {
            User? existUser = await _userRepo.FindByEmailAsync(dto.Email);
            if (existUser is null || !existUser.IsConfirmed || !existUser.IsActive)
                return true;
            string? hasActiveCode = await _cacheService.GetAsync<string>($"forget-password-{existUser.Id}");
            if (hasActiveCode is not null)
                throw new InvalidAccountException(ExceptionMessage.ValidVerificationCode);
            string code = SecurityService.GenerateVerificationCode();
            await _cacheService.SetAsync($"forget-password-{existUser.Id}", code, TimeSpan.FromMinutes(2));
            await _emailService.SendAsync(dto.Email, code, "Reset verification code");
            return true;
        }
        public async Task<Guid> SendEmailVerificationCode(string email)
        {
            User? existUser = await _userRepo.FindByEmailAsync(email);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            string? code = await _cacheService.GetAsync<string>($"user-email-verification-{existUser.Id}");
            if (code is not null)
                throw new InvalidAccountException(ExceptionMessage.ValidVerificationCode);
            string newCode = SecurityService.GenerateVerificationCode();
            await _cacheService.SetAsync($"user-email-verification-{existUser.Id}", newCode, TimeSpan.FromMinutes(2));
            await _emailService.SendAsync(existUser.Email, newCode, "Verification Code");
            return existUser.Id;
        }
      
        public async Task<bool> UserActive(Guid id)
        {
            User? existUser = await _userRepo.GetByIdAsync(id, true);
            if (existUser is null) throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if (existUser.IsActive)
                return true;
            existUser.IsActive = true;
            await _cacheService.RemoveAsync($"DeactivatedUser:{id}");
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<bool> UserDeactive(Guid id)
        {
            User? existUser = await _userRepo.GetByIdAsync(id, true);
            if (existUser is null) throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if (!existUser.IsActive)
                return true;
            await _cacheService.RemoveAsync($"RefreshToken:{id}");
            await _cacheService.SetAsync($"DeactivatedUser:{id}", true, TimeSpan.FromMinutes(_configuration.GetValue<int>("JwtSettings:ExpireAt")+1));
            existUser.IsActive = false;
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}
