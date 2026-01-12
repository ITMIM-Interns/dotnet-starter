using MediatR;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Features.Commands.Users.Update
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserReadRepository _userRead;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUserReadRepository userRead, IUnitOfWork unitOfWork)
        {
            _userRead = userRead;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User existUser = await _userRead.GetByIdAsync(request.Id,true);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            existUser.Username = request.Username is null? existUser.Username:request.Username;
            existUser.Image = request.Image is null? existUser.Image:request.Image.FileName;
            await _unitOfWork.SaveAsync();
            return Unit.Value;
        }
    }
}
