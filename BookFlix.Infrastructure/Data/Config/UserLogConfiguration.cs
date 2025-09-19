using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class UserLogConfiguration : IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            builder.ToTable("UserLogs");
            builder.HasKey(ul => ul.Id);

            builder.Property(ul => ul.EventType)
                .IsRequired();
            builder.Property(ul => ul.Timestamp)
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(ul => ul.IpAddress)
                .HasMaxLength(45)
                .IsRequired(); // Max length for IPv6
            builder.Property(ul => ul.Success)
                .IsRequired();
            builder.Property(ul => ul.UserId)
                .IsRequired();

            // Relationships
            builder.HasOne(ul => ul.User)
                .WithMany(u => u.UserLogs)
                .HasForeignKey(ul => ul.UserId);
        }
    }
}
