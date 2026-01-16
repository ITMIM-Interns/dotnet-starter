using MediatR;

namespace MiniApp.BLL.Features.Commands.Accounts.SendEmailVerificationCode
{
    public sealed record SendEmailVerificationCodeCommand(Guid UserId):IRequest<bool>;
  
}
