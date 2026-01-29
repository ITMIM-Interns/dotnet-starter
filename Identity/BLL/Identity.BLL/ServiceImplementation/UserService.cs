using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.BLL.Abstractions.Internals.Services;
using Identity.BLL.Exceptions.Commons;
using Identity.BLL.Exceptions.Users;
using Identity.BLL.Helpers;
using Identity.DTO.Users;
using Identity.Entity.Models;

namespace Identity.BLL.ServiceImplementation
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        public UserService(IUserRepository userRepo, IFileService fileService,
                                        IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _userRepo = userRepo;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
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
