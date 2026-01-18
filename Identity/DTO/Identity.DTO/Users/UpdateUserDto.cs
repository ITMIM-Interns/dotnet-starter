using Microsoft.AspNetCore.Http;

namespace Identity.DTO.Users
{
    public sealed record UpdateUserDto
    (
        Guid Id,
        string? Username,
        IFormFile? Image
    );
}
