using MiniApp.Models.Models;

namespace MiniApp.BLL.Abstractions.Internals.Reads
{
    public interface IUserReadRepository : IGenericReadRepository<User, Guid> 
    {
        Task<bool> EmailExistsAsync(string email);
        Task<User?> FindByEmailAsync(string email,bool hasTracked=false);
        Task<bool> UserNameExistsAsync(string userName);
        Task<bool> UserNameExistAsyncForUpdate(string username,Guid id);
        Task<User?> FindByUsernameAsync(string userName,bool hasTracked=false);
        Task<bool> CheckUserPasswordAsync(User user,string password);
    };
   
}
