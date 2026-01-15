using MiniApp.Models.Commons;
using MiniApp.Models.Enums;

namespace MiniApp.Models.Models
{
    public sealed class UserVerification : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public VerificationType Type { get; set; }
        public string? Code { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public bool IsConfirm { get; set; }
        public bool IsUsed { get; set; }

    }
}
