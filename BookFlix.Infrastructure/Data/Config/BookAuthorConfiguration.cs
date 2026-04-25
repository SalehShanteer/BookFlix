using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
    {
        public void Configure(EntityTypeBuilder<BookAuthor> builder)
        {
            builder.ToTable("BookAuthors");
            builder.HasKey(bg => new { bg.BookId, bg.AuthorId });

            builder.HasData(SeedData.LoadBookAuthorsData());

        }
    }
}
