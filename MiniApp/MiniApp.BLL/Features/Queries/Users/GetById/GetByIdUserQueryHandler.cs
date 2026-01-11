using MediatR;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.DTOs.Users;
using MiniApp.Models.Models;

namespace MiniApp.BLL.Features.Queries.Users.GetById
{
    public sealed class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, UserDto>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetByIdUserQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<UserDto> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            User? user = await _userReadRepository.GetByIdAsync(request.Id);
            if (user is null)
                throw new UserNotFoundException();
            return new UserDto(user.Id,user.Username,user.Email);
        }
    }
}
