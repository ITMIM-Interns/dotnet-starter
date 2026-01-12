using MediatR;

namespace MiniApp.BLL.Features.Commands.Users.Delete
{
    public sealed record DeleteUserCommand(Guid Id):IRequest;
   
}
