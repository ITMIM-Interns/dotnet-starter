using Identity.Entity.Commons;

namespace Identity.Entity.Models;

public sealed class User :BaseEntity<Guid>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string? Image { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsActive { get; set; }
}
