using BookFlix.Core.Models;

namespace BookFlix.Infrastructure.Data.Config
{
    public static class SeedData
    {
        private static readonly Guid Book_Gatsby = Guid.Parse("b0000000-0000-0000-0000-000000000001");
        private static readonly Guid Book_Mockingbird = Guid.Parse("b0000000-0000-0000-0000-000000000002");
        private static readonly Guid Book_1984 = Guid.Parse("b0000000-0000-0000-0000-000000000003");
        private static readonly Guid Book_Pride = Guid.Parse("b0000000-0000-0000-0000-000000000004");
        private static readonly Guid Book_Hobbit = Guid.Parse("b0000000-0000-0000-0000-000000000005");
        private static readonly Guid Book_Dune = Guid.Parse("b0000000-0000-0000-0000-000000000006");
        private static readonly Guid Book_Sapiens = Guid.Parse("b0000000-0000-0000-0000-000000000007");
        private static readonly Guid Book_DaVinci = Guid.Parse("b0000000-0000-0000-0000-000000000008");

        private static readonly Guid Author_Fitzgerald = Guid.Parse("a0000000-0000-0000-0000-000000000001");
        private static readonly Guid Author_Lee = Guid.Parse("a0000000-0000-0000-0000-000000000002");
        private static readonly Guid Author_Orwell = Guid.Parse("a0000000-0000-0000-0000-000000000003");
        private static readonly Guid Author_Austen = Guid.Parse("a0000000-0000-0000-0000-000000000004");
        private static readonly Guid Author_Tolkien = Guid.Parse("a0000000-0000-0000-0000-000000000005");
        private static readonly Guid Author_Herbert = Guid.Parse("a0000000-0000-0000-0000-000000000006");
        private static readonly Guid Author_Harari = Guid.Parse("a0000000-0000-0000-0000-000000000007");
        private static readonly Guid Author_Brown = Guid.Parse("a0000000-0000-0000-0000-000000000008");

        public static IList<Book> LoadBookData()
        {
            var staticCreatedAt = new DateTime(2025, 8, 5, 10, 0, 0, DateTimeKind.Utc);

            return new List<Book>
        {
            new Book { Id = Book_Gatsby, Title = "The Great Gatsby", Description = "A novel set in the Roaring Twenties.", ISBN = "9780743273565", CoverImageUrl = "https://example.com/great-gatsby.jpg", PublicationDate = new DateTime(1925, 4, 10), Publisher = "Scribner", PageCount = 180, AverageRating = 4.20m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { Id = Book_Mockingbird, Title = "To Kill a Mockingbird", Description = "A novel about racial injustice in the Deep South.", ISBN = "9780061120084", CoverImageUrl = "https://example.com/to-kill-a-mockingbird.jpg", PublicationDate = new DateTime(1960, 7, 11), Publisher = "J.B. Lippincott & Co.", PageCount = 281, AverageRating = 4.30m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { Id = Book_1984, Title = "1984", Description = "A dystopian novel about totalitarianism.", ISBN = "9780451524935", CoverImageUrl = "https://example.com/1984.jpg", PublicationDate = new DateTime(1949, 6, 8), Publisher = "Secker & Warburg", PageCount = 328, AverageRating = 4.40m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { Id = Book_Pride, Title = "Pride and Prejudice", Description = "A romantic novel about love and social class.", ISBN = "9780141439518", CoverImageUrl = "https://example.com/pride-and-prejudice.jpg", PublicationDate = new DateTime(1813, 1, 28), Publisher = "Penguin Classics", PageCount = 432, AverageRating = 4.25m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { Id = Book_Hobbit, Title = "The Hobbit", Description = "A fantasy adventure about Bilbo Baggins.", ISBN = "9780547928227", CoverImageUrl = "https://example.com/the-hobbit.jpg", PublicationDate = new DateTime(1937, 9, 21), Publisher = "Houghton Mifflin", PageCount = 310, AverageRating = 4.27m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { Id = Book_Dune, Title = "Dune", Description = "A science fiction epic about a desert planet.", ISBN = "9780441172719", CoverImageUrl = "https://example.com/dune.jpg", PublicationDate = new DateTime(1965, 8, 1), Publisher = "Ace Books", PageCount = 412, AverageRating = 4.21m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { Id = Book_Sapiens, Title = "Sapiens: A Brief History of Humankind", Description = "A nonfiction exploration of human history.", ISBN = "9780062316097", CoverImageUrl = "https://example.com/sapiens.jpg", PublicationDate = new DateTime(2014, 9, 9), Publisher = "Harper", PageCount = 443, AverageRating = 4.38m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { Id = Book_DaVinci, Title = "The Da Vinci Code", Description = "A thriller involving a religious conspiracy.", ISBN = "9780307277671", CoverImageUrl = "https://example.com/da-vinci-code.jpg", PublicationDate = new DateTime(2003, 3, 18), Publisher = "Doubleday", PageCount = 454, AverageRating = 3.85m, IsAvailable = true, CreatedAt = staticCreatedAt }
        };
        }

        public static List<Author> LoadAuthorsData()
        {
            return new List<Author>
        {
            new Author { Id = Author_Fitzgerald, Name = "F. Scott Fitzgerald" },
            new Author { Id = Author_Lee, Name = "Harper Lee" },
            new Author { Id = Author_Orwell, Name = "George Orwell" },
            new Author { Id = Author_Austen, Name = "Jane Austen" },
            new Author { Id = Author_Tolkien, Name = "J.R.R. Tolkien" },
            new Author { Id = Author_Herbert, Name = "Frank Herbert" },
            new Author { Id = Author_Harari, Name = "Yuval Noah Harari" },
            new Author { Id = Author_Brown, Name = "Dan Brown" }
        };
        }

        public static List<BookAuthor> LoadBookAuthorsData()
        {
            return new List<BookAuthor>
        {
             new BookAuthor { BookId = Book_Gatsby, AuthorId = Author_Fitzgerald },
             new BookAuthor { BookId = Book_Mockingbird, AuthorId = Author_Lee },
             new BookAuthor { BookId = Book_1984, AuthorId = Author_Orwell },
             new BookAuthor { BookId = Book_Pride, AuthorId = Author_Austen },
             new BookAuthor { BookId = Book_Hobbit, AuthorId = Author_Tolkien },
             new BookAuthor { BookId = Book_Dune, AuthorId = Author_Herbert },
             new BookAuthor { BookId = Book_Sapiens, AuthorId = Author_Harari },
             new BookAuthor { BookId = Book_DaVinci, AuthorId = Author_Brown }
        };
        }

        public static List<Genre> LoadGenresData()
        {
            return new List<Genre>
        {
            new Genre { Id = 1, Name = "Fiction" },
            new Genre { Id = 2, Name = "Nonfiction" },
            new Genre { Id = 3, Name = "Science Fiction" },
            new Genre { Id = 4, Name = "Fantasy" },
            new Genre { Id = 5, Name = "Mystery" },
            new Genre { Id = 6, Name = "Thriller" },
            new Genre { Id = 7, Name = "Romance" },
            new Genre { Id = 8, Name = "Historical Fiction" },
            new Genre { Id = 9, Name = "Biography" },
            new Genre { Id = 10, Name = "Autobiography" },
            new Genre { Id = 11, Name = "Self-Help" },
            new Genre { Id = 12, Name = "Business" },
            new Genre { Id = 13, Name = "Science" },
            new Genre { Id = 14, Name = "History" },
            new Genre { Id = 15, Name = "Young Adult" },
            new Genre { Id = 16, Name = "Children" },
            new Genre { Id = 17, Name = "Poetry" },
            new Genre { Id = 18, Name = "Horror" },
            new Genre { Id = 19, Name = "Adventure" },
            new Genre { Id = 20, Name = "Crime" },
            new Genre { Id = 21, Name = "Literary Criticism" },
            new Genre { Id = 22, Name = "Cooking" },
            new Genre { Id = 23, Name = "Travel" },
            new Genre { Id = 24, Name = "Philosophy" },
            new Genre { Id = 25, Name = "Religion" }
        };
        }

        public static List<BookGenre> LoadBookGenresData()
        {
            return new List<BookGenre>
        {
            new BookGenre { BookId = Book_Gatsby, GenreId = 1 }, // Fiction
            new BookGenre { BookId = Book_Mockingbird, GenreId = 1 }, // Fiction
            new BookGenre { BookId = Book_1984, GenreId = 3 }, // Science Fiction
            new BookGenre { BookId = Book_Pride, GenreId = 7 }, // Romance
            new BookGenre { BookId = Book_Pride, GenreId = 8 }, // Historical Fiction
            new BookGenre { BookId = Book_Hobbit, GenreId = 4 }, // Fantasy
            new BookGenre { BookId = Book_Hobbit, GenreId = 19 }, // Adventure
            new BookGenre { BookId = Book_Dune, GenreId = 3 }, // Science Fiction
            new BookGenre { BookId = Book_Sapiens, GenreId = 2 }, // Nonfiction
            new BookGenre { BookId = Book_Sapiens, GenreId = 14 }, // History
            new BookGenre { BookId = Book_DaVinci, GenreId = 6 }, // Thriller
            new BookGenre { BookId = Book_DaVinci, GenreId = 5 }  // Mystery
        };
        }
    }
}
