using MediatR;
using Microsoft.AspNetCore.Http;

namespace MiniApp.BLL.Features.Commands.Users.Create
{
    public sealed record CreateUserCommand(
        string Username,
        string Email,
        string Password,
        string ContactNumber,
        IFormFile Image
        ) : IRequest<Guid>;
   
}
