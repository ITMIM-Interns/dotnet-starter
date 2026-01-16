using MediatR;

namespace MiniApp.BLL.Features.Commands.Accounts.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(Guid userId,string code):IRequest<bool>;

}
