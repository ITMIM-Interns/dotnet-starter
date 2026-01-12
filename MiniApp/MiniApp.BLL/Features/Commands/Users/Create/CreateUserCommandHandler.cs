using MediatR;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.BLL.Helpers;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Features.Commands.Users.Create
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,Guid>
    {
        private readonly IUserWriteRepository _userWrite;
        private readonly IUserReadRepository _userRead;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUserWriteRepository userWrite, IUserReadRepository userRead, IUnitOfWork unitOfWork)
        {
            _userWrite = userWrite;
            _userRead = userRead;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRead.EmailExistsAsync(request.Email))
                throw new UserNotFoundException(ExceptionMessage.ExistUserEmailMessage);
            if (await _userRead.UserNameExistsAsync(request.Username))
                throw new UserNotFoundException(ExceptionMessage.ExistUsernameMessage);
            byte[] salt = SecurityService.GenerateSalt();
            string hashedPassword = SecurityService.PasswordHash(request.Password,salt);
            Console.WriteLine(hashedPassword.Length);
            User newUser = new User()
            {
                Username = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                Salt = Convert.ToBase64String(salt),
                ContactNumber = request.ContactNumber,
                LastVerificationCode=SecurityService.GenerateVerificationCode()
            };
            if (request.Image is not null)
                newUser.Image = request.Image.ToString();
            if(request.ContactNumber is not null)
                newUser.ContactNumber = request.ContactNumber;
            await _userWrite.Add(newUser);
            await _unitOfWork.SaveAsync();
            return newUser.Id;
        }
    }
}
