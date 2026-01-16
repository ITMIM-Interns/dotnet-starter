using MediatR;

namespace MiniApp.BLL.Features.Commands.Accounts.ToggleUserStatus
{
    public sealed record UserActiveCommand(Guid UserId) : IRequest<bool>;
    
}
