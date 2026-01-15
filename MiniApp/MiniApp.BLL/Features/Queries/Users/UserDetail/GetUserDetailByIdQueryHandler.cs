using MediatR;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.DTOs.Users;

namespace MiniApp.BLL.Features.Queries.Users.GetById
{
    public sealed class GetUserDetailByIdQueryHandler : IRequestHandler<GetUserDetailByIdQuery, UserDto>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserDetailByIdQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<UserDto> Handle(GetUserDetailByIdQuery request, CancellationToken cancellationToken)
        {
            UserDto? user = await _userReadRepository.GetUserDetailByIdAsync(request.Id);
            if (user is null)
                throw new UserNotFoundException(ExceptionMessage.UserNotFoundMessage);
            return user;
        }
    }
}
