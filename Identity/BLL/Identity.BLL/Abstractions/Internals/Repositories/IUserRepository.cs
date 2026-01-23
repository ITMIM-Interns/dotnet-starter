using Identity.DTO.Users;
using Identity.Entity.Models;

namespace Identity.BLL.Abstractions.Internals.Repositories
{
    public interface IUserRepository : IGenericRepository<User, Guid> 
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> FindByEmailAsync(string email,bool hasTracked=false);
        Task<bool> UserNameExistsAsync(string userName);
        Task<bool> UserNameExistAsyncForUpdate(string username,Guid id);
        Task<User?> FindByUsernameAsync(string userName,bool hasTracked=false);
        Task<bool> CheckUserPasswordAsync(User user,string password);
        Task<UserDto?> GetUserDetailByIdAsync(Guid userId);
        Task<bool> ExistUserByidAsync(Guid userId);
    };
   
}
