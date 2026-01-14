using MediatR;
using MiniApp.BLL.Abstractions.Externals;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.BLL.Helpers;
using MiniApp.Models.Models;
using System.Runtime.Intrinsics.X86;

namespace MiniApp.BLL.Features.Commands.Users.Update
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserReadRepository _userRead;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateUserCommandHandler(IUserReadRepository userRead, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _userRead = userRead;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User existUser = await _userRead.GetByIdAsync(request.Id,true);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if(request.Username is not null)
            {
                bool hasUser = await _userRead.UserNameExistAsyncForUpdate(request.Username, existUser.Id);
                if (hasUser)
                    throw new ExistUserFieldException(ExceptionMessage.ExistUsernameMessage);
                existUser.Username = request.Username is null ? existUser.Username : request.Username;
            }
          
            if (request.Image is not null)
            {
                string fileKey = ImageService.ExtractKeyFromUrl(existUser.Image);
                string imageUrl = await _fileService.UpdateFileAsync(request.Image, fileKey, "user-image");
                existUser.Image = imageUrl;
            }
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
