using MediatR;
using MiniApp.BLL.Abstractions.Externals.Files;
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
        private readonly IFileService _fileService;
        public CreateUserCommandHandler(IUserWriteRepository userWrite, IUserReadRepository userRead, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _userWrite = userWrite;
            _userRead = userRead;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRead.EmailExistsAsync(request.Email))
                throw new ExistUserFieldException(ExceptionMessage.ExistUserEmailMessage);
            if (await _userRead.UserNameExistsAsync(request.Username))
                throw new ExistUserFieldException(ExceptionMessage.ExistUsernameMessage);
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
                newUser.Image = await _fileService.UploadFileAsync(request.Image,"user-image");
            await _userWrite.Add(newUser);
            await _unitOfWork.SaveAsync();
            return newUser.Id;
        }
    }
}
