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

            builder.HasData(_LoadBookAuthorsData());

        }

        private static List<BookAuthor> _LoadBookAuthorsData()
        {
            var bookAuthors = new List<BookAuthor>
            {
                 new BookAuthor { BookId = 1, AuthorId = 1 }, // The Great Gatsby: F. Scott Fitzgerald
                 new BookAuthor { BookId = 2, AuthorId = 2 }, // To Kill a Mockingbird: Harper Lee
                 new BookAuthor { BookId = 3, AuthorId = 3 }, // 1984: George Orwell
                 new BookAuthor { BookId = 4, AuthorId = 4 }, // Pride and Prejudice: Jane Austen
                 new BookAuthor { BookId = 5, AuthorId = 5 }, // The Hobbit: J.R.R. Tolkien
                 new BookAuthor { BookId = 6, AuthorId = 6 }, // Dune: Frank Herbert
                 new BookAuthor { BookId = 7, AuthorId = 7 }, // Sapiens: Yuval Noah Harari
                 new BookAuthor { BookId = 8, AuthorId = 8 }  // The Da Vinci Code: Dan Brown
            };

            return bookAuthors;
        }
    }

}
