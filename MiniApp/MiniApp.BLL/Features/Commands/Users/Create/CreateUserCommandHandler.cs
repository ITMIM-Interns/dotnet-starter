using MediatR;
using MiniApp.BLL.Abstractions.Externals;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.BLL.Helpers;
using MiniApp.Models.Enums;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Features.Commands.Users.Create
{
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand,Guid>
    {
        private readonly IUserWriteRepository _userWrite;
        private readonly IUserReadRepository _userRead;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IUserVerificationWriteRepository _userVerificationWrite;
        public CreateUserCommandHandler(IUserWriteRepository userWrite, IUserReadRepository userRead, IUnitOfWork unitOfWork, IFileService fileService, IEmailService emailService, IUserVerificationWriteRepository userVerificationWrite)
        {
            _userWrite = userWrite;
            _userRead = userRead;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _emailService = emailService;
            _userVerificationWrite = userVerificationWrite;
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
                IsActive = true,
            };
            UserVerification userVerification = new UserVerification()
            {
                User=newUser,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5),
                Code = SecurityService.GenerateVerificationCode(),
                IsConfirm = false,
                Type = VerificationType.Email,
                IsUsed=false
            };
            if (request.Image is not null)
                newUser.Image = await _fileService.UploadFileAsync(request.Image,"user-image");
            await _userVerificationWrite.Add(userVerification);
            await _userWrite.Add(newUser);
            await _unitOfWork.SaveAsync();
            await _emailService.SendAsync(request.Email,userVerification.Code,"Email confrimation");
            return newUser.Id;
        }
    }
}
