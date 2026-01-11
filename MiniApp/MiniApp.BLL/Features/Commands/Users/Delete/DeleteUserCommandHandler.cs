using MediatR;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.BLL.Features.Commands.Users.Delete
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserReadRepository _readUser;
        private readonly IUserWriteRepository _writeUser;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserReadRepository readUser, IUserWriteRepository writeUser)
        {
            _unitOfWork = unitOfWork;
            _readUser = readUser;
            _writeUser = writeUser;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User? existUser = await _readUser.GetByIdAsync(request.Id,true);
            if (existUser is null)
                throw new UserNotFoundException();
            await _writeUser.Remove(existUser);
            return Unit.Value;
        }
    }
}
