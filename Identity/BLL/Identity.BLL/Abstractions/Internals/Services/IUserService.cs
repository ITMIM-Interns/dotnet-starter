using Identity.DTO.Users;
using Identity.Entity.Models;


namespace Identity.BLL.Abstractions.Internals.Services
{
    public interface IUserService
    {
        Task<Guid> Add(CreateUserDto request);
        Task Remove(Guid id);
        Task Update(UpdateUserDto request);
        Task<User?> GetByIdAsync(Guid id, bool hasTracked = false);
        public Task<UserDto> GetUserDetailByIdAsync(Guid id);
    }
}
