using MediatR;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Features.Commands.Accounts.ToggleUserStatus
{
    public sealed class UserActiveCommandHandler : IRequestHandler<UserActiveCommand, bool>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserActiveCommandHandler(IUserReadRepository userReadRepository, IUnitOfWork unitOfWork)
        {
            _userReadRepository = userReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UserActiveCommand request, CancellationToken cancellationToken)
        {
            User existUser=await _userReadRepository.GetByIdAsync(request.UserId,true);
            if (existUser is null) throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            if (existUser.IsActive)
                return true;
            existUser.IsActive=true;
            return await _unitOfWork.SaveAsync()>0;
        }
    }
}
