using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres");
            builder.HasKey(g => g.Id);
            builder.HasIndex(g => g.Name)
                .IsUnique();

            builder.Property(g => g.Id).ValueGeneratedNever();
            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasData(SeedData.LoadGenresData());
        }
    }
}
