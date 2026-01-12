using Microsoft.EntityFrameworkCore;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Exceptions.Commons;
using MiniApp.BLL.Exceptions.Users;
using MiniApp.BLL.Helpers;
using MiniApp.DataAccess.Data;
using MiniApp.Models.Models;
using System.Text.Json;

namespace MiniApp.DAL.Implementations.Internals.Reads
{
    public sealed class UserReadRepository : GenericReadRepository<User, Guid>, IUserReadRepository
    {
        public UserReadRepository(AppDbContext context) : base(context) { }

        public async Task<bool> EmailExistsAsync(string email) => await _context.Users.AnyAsync(u => u.Email == email);
        public async Task<bool> UserNameExistsAsync(string userName) => await _context.Users.AnyAsync(u=>u.Username==userName);
        public async Task<User> FindByEmailAsync(string email, bool hasTracked = false)
        {
            User existUser;
            if (hasTracked)
             existUser= await _context.Users.FirstOrDefaultAsync(u=>u.Email==email);
            else 
                existUser= await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            return existUser;
        }

        public async Task<User> FindByUsernameAsync(string userName, bool hasTracked = false)
        {
            User existUser;
            if (hasTracked)
                existUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userName);
            else
                existUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == userName);
            return existUser;
        }

        public Task<bool> CheckUserPasswordAsync(User user, string password)
        {
            byte[] salt=JsonSerializer.Deserialize<byte[]>(user.Salt);
            bool isCorrect = SecurityService.VerifyPassword(password, salt, user.Password);
            return Task.FromResult(isCorrect);
        }
    }
}
