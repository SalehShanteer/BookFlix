using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{

    public partial class UserConfiguration
    {
        public class BookConfiguration : IEntityTypeConfiguration<Book>
        {
            public void Configure(EntityTypeBuilder<Book> builder)
            {
                builder.ToTable("Books");
                builder.HasKey(b => b.Id);
                builder.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                builder.Property(b => b.Description)
                    .HasMaxLength(1000);
                builder.Property(b => b.ISBN)
                    .HasMaxLength(20);
                builder.Property(b => b.Genre)
                    .HasMaxLength(100);
                builder.Property(b => b.CoverImageUrl)
                    .HasMaxLength(500);
                builder.Property(b => b.PublicationDate)
                    .HasColumnType("datetime");
                builder.Property(b => b.Publisher)
                    .HasMaxLength(200);
                builder.Property(b => b.PageCount)
                    .IsRequired(false);
                builder.Property(b => b.AverageRating)
                    .HasColumnType("decimal(3, 2)");
                builder.Property(b => b.IsAvailable)
                    .IsRequired();
                builder.Property(b => b.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");
                builder.Property(b => b.UpdatedAt)
                    .IsRequired(false);
                builder.Property(b => b.FileLocation)
                    .IsRequired(false)
                    .HasMaxLength(500);

                // Relationships
                builder.HasMany(b => b.Authors)
                    .WithMany(a => a.Books);
                builder.HasMany(b => b.Reviews)
                    .WithOne(r => r.Book)
                    .HasForeignKey(r => r.BookId);
            }
        }
    }
}
