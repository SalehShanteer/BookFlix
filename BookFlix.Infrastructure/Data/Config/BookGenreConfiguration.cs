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

            builder.HasData(_LoadBookGenresData());
        }

        private static List<BookGenre> _LoadBookGenresData()
        {
            var bookGenres = new List<BookGenre>
            {
                new BookGenre { BookId = 1, GenreId = 1 }, // The Great Gatsby: Fiction
                new BookGenre { BookId = 2, GenreId = 1 }, // To Kill a Mockingbird: Fiction
                new BookGenre { BookId = 3, GenreId = 3 }, // 1984: Science Fiction (Dystopian)
                new BookGenre { BookId = 4, GenreId = 7 }, // Pride and Prejudice: Romance
                new BookGenre { BookId = 4, GenreId = 8 }, // Pride and Prejudice: Historical Fiction
                new BookGenre { BookId = 5, GenreId = 4 }, // The Hobbit: Fantasy
                new BookGenre { BookId = 5, GenreId = 19 }, // The Hobbit: Adventure
                new BookGenre { BookId = 6, GenreId = 3 }, // Dune: Science Fiction
                new BookGenre { BookId = 7, GenreId = 2 }, // Sapiens: Nonfiction
                new BookGenre { BookId = 7, GenreId = 14 }, // Sapiens: History
                new BookGenre { BookId = 8, GenreId = 6 }, // The Da Vinci Code: Thriller
                new BookGenre { BookId = 8, GenreId = 5 }  // The Da Vinci Code: Mystery
            };

            return bookGenres;
        }
    }

}
