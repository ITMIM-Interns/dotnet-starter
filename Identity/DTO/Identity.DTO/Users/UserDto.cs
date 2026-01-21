namespace Identity.DTO.Users
{
    public sealed record UserDto
    (
              Guid id,
              string Username,
              string Email,
              string ImageUrl,
              bool IsActive,
              bool isEmailConfirmed
    );
}
