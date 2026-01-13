using MediatR;
using MiniApp.BLL.Abstractions.Externals.Files;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.BLL.Helpers;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Features.Commands.Users.Delete
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserReadRepository _readUser;
        private readonly IUserWriteRepository _writeUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserReadRepository readUser, IUserWriteRepository writeUser, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _readUser = readUser;
            _writeUser = writeUser;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User? existUser = await _readUser.GetByIdAsync(request.Id,true);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if(existUser.Image is not null)
                await _fileService.RemoveFileAsync(ImageService.ExtractKeyFromUrl(existUser.Image));
            await _writeUser.Remove(existUser);
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
