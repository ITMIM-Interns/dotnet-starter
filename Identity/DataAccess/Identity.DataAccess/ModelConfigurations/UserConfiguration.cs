using Identity.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.DataAccess.ModelConfigurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u=>u.Username).IsRequired().HasMaxLength(100);
            builder.Property(u=>u.Username).IsRequired().HasMaxLength(50);
            builder.HasIndex(u=>u.Username).IsUnique();
            builder.Property(u=>u.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u=>u.ContactNumber).IsRequired().HasMaxLength(50);
            builder.Property(u=>u.Salt).IsRequired().HasMaxLength(64);
            builder.Property(u=>u.Password).IsRequired();
        }   
    }
}
