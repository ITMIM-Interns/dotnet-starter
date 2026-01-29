using Microsoft.AspNetCore.Http;

namespace Identity.DTO.Users
{
    public sealed record CreateUserDto(
        string Username,
        string Email,
        string Password,
        string ContactNumber,
        IFormFile? Image
        );
   
}
