using MediatR;
using Microsoft.AspNetCore.Http;

namespace MiniApp.BLL.Features.Commands.Users.Update
{
    public sealed record UpdateUserCommand
    (
        Guid Id,
        string Username,
        IFormFile Image
    ):IRequest;
}
