namespace MiniApp.DTOs.Users
{
    public sealed record UserDto(
          Guid id,
          string Username,
          string Email,
          string Password,
          string ImageUrl,
          bool IsActive,
          bool isEmailConfirmed
        );
    
}
