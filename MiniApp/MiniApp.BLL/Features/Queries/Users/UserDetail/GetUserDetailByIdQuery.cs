using MediatR;
using MiniApp.DTOs.Users;

namespace MiniApp.BLL.Features.Queries.Users.GetById
{
    public sealed record GetUserDetailByIdQuery(Guid Id) : IRequest<UserDto>;
    
}
