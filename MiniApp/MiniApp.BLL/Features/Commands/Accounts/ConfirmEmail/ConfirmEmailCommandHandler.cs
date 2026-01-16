using MediatR;
using Microsoft.Extensions.Logging;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.BLL.Features.Commands.Accounts.ConfirmEmail;
using MiniApp.Models.Enums;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Features.Commands.Accounts.ChangeVerify
{
    public sealed class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IUserVerificationReadRepository _userVerificationRead;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(IUserVerificationReadRepository userVerificationRead, IUnitOfWork unitOfWork, ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userVerificationRead = userVerificationRead;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            UserVerification existVerification = await _userVerificationRead.GetUserVerificationByType(request.userId,VerificationType.Email);
            if (existVerification is null)
                throw new VerificationNotFoundException(ExceptionMessage.VerificationNotFoundMessage);
            if (existVerification.IsConfirm)
                throw new VerificationAlreadyConfirmed(ExceptionMessage.VerificationConfirmedMessage);
            if (existVerification.Code !=request.code)
                throw new InvalidVerificationCodeException(ExceptionMessage.InvalidVerificationCodeMessage);
            if (existVerification.ExpiresAt <= DateTimeOffset.UtcNow)
                throw new VerificationCodeExpiredException(ExceptionMessage.InvalidExpiresTimeMessage);
            existVerification.IsConfirm = true;
            existVerification.IsUsed = true;
            return await _unitOfWork.SaveAsync()>0;
        }
    }
}
