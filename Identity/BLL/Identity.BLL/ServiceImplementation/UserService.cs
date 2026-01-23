using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.BLL.Abstractions.Internals.Services;
using Identity.BLL.Exceptions.Commons;
using Identity.BLL.Exceptions.Users;
using Identity.BLL.Helpers;
using Identity.DTO.Users;
using Identity.Entity.Enums;
using Identity.Entity.Models;

namespace Identity.BLL.ServiceImplementation
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IUserVerificationRepository _userVerificationRepo;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepo, IFileService fileService,
                                        IEmailService emailService, IUserVerificationRepository userVerificationRepo, IUnitOfWork unitOfWork)
        {
            _userRepo = userRepo;
            _fileService = fileService;
            _emailService = emailService;
            _userVerificationRepo = userVerificationRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Add(CreateUserDto request)
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
            };
            UserVerification userVerification = new UserVerification()
            {
                User = newUser,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5),
                Code = SecurityService.GenerateVerificationCode(),
                Status = VerificationStatus.Active,
                Type = VerificationType.EmailConfirm,
            };
            if (request.Image is not null)
                newUser.Image = await _fileService.UploadFileAsync(request.Image, "user-image");
            await _userVerificationRepo.Add(userVerification);
            await _userRepo.Add(newUser);
            await _unitOfWork.SaveAsync();
            await _emailService.SendAsync(request.Email, userVerification.Code, "Email confrimation");
            return newUser.Id;
        }
        public async Task Remove(Guid id)
        {
            User? existUser = await _userRepo.GetByIdAsync(id, true);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if (existUser.Image is not null)
                await _fileService.RemoveFileAsync(ImageService.ExtractKeyFromUrl(existUser.Image));
            await _userRepo.Remove(existUser);
            await _unitOfWork.SaveAsync();
            return;
        }
      
        public async Task<User?> GetByIdAsync(Guid id, bool hasTracked = false)=> await _userRepo.GetByIdAsync(id, hasTracked);
        public async Task<UserDto> GetUserDetailByIdAsync(Guid id)
        {
            UserDto? user = await _userRepo.GetUserDetailByIdAsync(id);
            if (user is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            return user;
        }
        public async Task Update(UpdateUserDto request)
        {
            User? existUser = await _userRepo.GetByIdAsync(request.Id, true);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if (request.Username is not null)
            {
                bool hasUser = await _userRepo.UserNameExistAsyncForUpdate(request.Username, existUser.Id);
                if (hasUser)
                    throw new ExistUserFieldException(ExceptionMessage.ExistUsernameMessage);
                existUser.Username = request.Username is null ? existUser.Username : request.Username;
            }

            if (request.Image is not null)
            {
                string imageUrl;
                if (existUser.Image is not null)
                {
                    string fileKey = ImageService.ExtractKeyFromUrl(existUser.Image);
                    imageUrl = await _fileService.UpdateFileAsync(request.Image, fileKey, "user-image");
                }
                else
                {
                    imageUrl = await _fileService.UploadFileAsync(request.Image, "user-image");
                }

                existUser.Image = imageUrl;
            }
            await _unitOfWork.SaveAsync();
            return;
        }
    }
}
