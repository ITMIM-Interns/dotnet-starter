using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DeleteUserCommandHandler> _logger;

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
            {
                _logger.LogInformation("User image is deleting");
                await _fileService.RemoveFileAsync(ImageService.ExtractKeyFromUrl(existUser.Image));
                _logger.LogInformation("User image deleted");
            }
            await _writeUser.Remove(existUser);
            _logger.LogInformation("User is deleting");
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Deleted user");
            return Unit.Value;
        }
    }
}
