using Identity.Entity.Commons;
using Identity.Entity.Enums;

namespace Identity.Entity.Models
{
    public sealed class UserVerification : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public VerificationType Type { get; set; }
        public string? Code { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public VerificationStatus Status { get; set; }

    }
}
