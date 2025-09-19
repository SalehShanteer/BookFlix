using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Content)
                .IsRequired(false)
                .HasMaxLength(255);
            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(r => r.UpdatedAt)
                .IsRequired(false);
            builder.Property(r => r.Rating)
                .IsRequired();
        }
    }
}
