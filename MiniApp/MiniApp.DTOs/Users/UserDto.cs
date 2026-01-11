namespace MiniApp.DTOs.Users
{
    public sealed record UserDto(
          Guid id,
          string Username,
          string Email
        );
    
}
