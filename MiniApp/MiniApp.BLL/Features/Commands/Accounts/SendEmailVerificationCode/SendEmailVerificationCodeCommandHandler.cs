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

namespace MiniApp.BLL.Features.Commands.Accounts.SendEmailVerificationCode
{
    public sealed class SendEmailVerificationCodeCommandHandler : IRequestHandler<SendEmailVerificationCodeCommand, bool>
    {
        private readonly IUserVerificationReadRepository _userVerificationRead;
        private readonly IUserVerificationWriteRepository _userVerificationWrite;
        private readonly IUserReadRepository _userRead;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public SendEmailVerificationCodeCommandHandler(IUserVerificationReadRepository userVerificationRead, IUserVerificationWriteRepository userVerificationWrite, IUnitOfWork unitOfWork, IEmailService emailService, IUserReadRepository userRead)
        {
            _userVerificationRead = userVerificationRead;
            _userVerificationWrite = userVerificationWrite;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userRead = userRead;
        }

        public async Task<bool> Handle(SendEmailVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            User existUser = await _userRead.GetByIdAsync(request.UserId);
            if (existUser is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            bool hasCode = await _userVerificationRead.CheckActiveVerificationCodeAsync(request.UserId,VerificationType.Email);
            if (hasCode)
                throw new InvalidAccountException(ExceptionMessage.InvalidVerificationCodeMessage);
            string newCode = SecurityService.GenerateVerificationCode();
            await _emailService.SendAsync(existUser.Email, newCode, "Verification Code");
            UserVerification newVerification = new UserVerification
            {
                Code = newCode,
                UserId = request.UserId,
                Type = VerificationType.Email,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(5),
                IsConfirm = false,
                IsUsed = false
            };
            await _userVerificationWrite.Add(newVerification);
            return await _unitOfWork.SaveAsync()>0; 
        }
    }
}
