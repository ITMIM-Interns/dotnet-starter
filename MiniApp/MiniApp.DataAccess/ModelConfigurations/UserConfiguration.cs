using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniApp.Models.Models;

namespace MiniApp.DataAccess.ModelConfigurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u=>u.Username).IsRequired().HasMaxLength(100);
            builder.HasIndex(u=>u.Username).IsUnique();
            builder.Property(u=>u.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u=>u.ContactNumber).IsRequired().HasMaxLength(50);
            builder.Property(u=>u.Salt).IsRequired().HasMaxLength(150);
            builder.Property(u=>u.Password).IsRequired().HasMaxLength(300);
        }   
    }
}
