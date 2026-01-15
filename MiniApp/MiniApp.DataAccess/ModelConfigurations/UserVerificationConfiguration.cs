using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniApp.Models.Models;

namespace MiniApp.DataAccess.ModelConfigurations
{
    internal sealed class UserVerificationConfiguration : IEntityTypeConfiguration<UserVerification>
    {
        public void Configure(EntityTypeBuilder<UserVerification> builder)
        {
            builder.Property(uv => uv.UserId).IsRequired();
            builder.Property(uv=>uv.Type).IsRequired();
            builder.Property(uv=>uv.Code).IsRequired(false).HasMaxLength(6);
        }
    }
}
