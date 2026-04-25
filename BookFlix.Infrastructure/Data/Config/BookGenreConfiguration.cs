using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.ToTable("BookGenres");
            builder.HasKey(bg => new { bg.BookId, bg.GenreId });

            builder.HasData(SeedData.LoadBookGenresData());
        }
    }
}
