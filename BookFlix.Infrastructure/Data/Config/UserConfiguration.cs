using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{

    public partial class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.Username)
                .IsUnique();
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(5)
                .HasDefaultValue("User");
            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(60);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.UpdatedAt)
                .IsRequired(false);

            // Relationships
            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);
        }
    }
}
