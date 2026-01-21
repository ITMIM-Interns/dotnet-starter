using Identity.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DAL.ModelConfigurations
{
    internal sealed class UserVerificationConfiguration : IEntityTypeConfiguration<UserVerification>
    {
        public void Configure(EntityTypeBuilder<UserVerification> builder)
        {
            builder.Property(uv => uv.UserId).IsRequired();
            builder.Property(uv=>uv.Type).IsRequired();
            builder.Property(uv=>uv.Code).IsRequired(false).HasMaxLength(6);
            builder.HasIndex(uv => new { uv.UserId, uv.Type }).HasFilter("[Status]=1").IsUnique();
        }
    }
}
