using MediatR;

namespace MiniApp.BLL.Features.Commands.Accounts.UserDeactive
{
    public sealed record UserDeactiveCommand(Guid UserId):IRequest<bool>;
   
}
