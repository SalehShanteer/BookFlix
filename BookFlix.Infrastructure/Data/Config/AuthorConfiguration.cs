using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.Name)
                .IsUnique();

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(SeedData.LoadAuthorsData());
        }
    }
}
