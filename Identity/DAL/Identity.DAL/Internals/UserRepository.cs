using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.BLL.Helpers;
using Identity.DAL.Data;
using Identity.DTO.Users;
using Identity.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.DAL.Internals
{
    public sealed class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<bool> UserNameExistsAsync(string userName) => await _context.Users.AnyAsync(u => u.Username == userName);
        public async Task<bool> UserNameExistAsyncForUpdate(string username, Guid id) => await _context.Users.AnyAsync(u => u.Username == username && u.Id != id);
        public async Task<bool> EmailExistsAsync(string email) => await _context.Users.AnyAsync(u => u.Email == email);
        public async Task<bool> ExistUserByidAsync(Guid userId) => await _context.Users.AnyAsync(u => u.Id == userId && u.IsActive);

        public Task<bool> CheckUserPasswordAsync(User user, string password)
        {
            byte[] salt = Convert.FromBase64String(user.Salt);
            bool isCorrect = SecurityService.VerifyPassword(password, salt, user.Password);
            return Task.FromResult(isCorrect);
        }
        public async Task<User?> FindByEmailAsync(string email, bool hasTracked = false)
        {
            IQueryable<User> query = _context.Users;
            if (hasTracked is false)
                query = query.AsNoTracking();
            User? existUser = await query.FirstOrDefaultAsync(u => u.Email == email);
            return existUser;
        }

        public async Task<User?> FindByUsernameAsync(string userName, bool hasTracked = false)
        {
            IQueryable<User> query = _context.Users;
            if (hasTracked is false)
                query = query.AsNoTracking();
            User? existUser = await query.FirstOrDefaultAsync(u => u.Username == userName);
            return existUser;
        }

        public async Task<UserDto?> GetUserDetailByIdAsync(Guid userId)
        {
            UserDto? user = await _context.Users.Where(u => u.Id == userId).Select(u => new UserDto(
                   u.Id, u.Username, u.Email, u.Image, u.IsActive,u.IsConfirmed)).FirstOrDefaultAsync();
            return user;
        }


    }
}
