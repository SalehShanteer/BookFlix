using BookFlix.Core.Models;

namespace BookFlix.Infrastructure.Data.Config
{
    public static class SeedData
    {
        private static readonly Guid User_Admin = Guid.Parse("3dba3903-21a6-413d-a479-eb807eb5e6ed");

        private static readonly Guid Role_Admin = Guid.Parse("32684285-5ff9-486d-a2a4-de00bdea2d20");
        private static readonly Guid Role_User = Guid.Parse("2abd05f3-fc73-4a5f-a3b5-01291030851f");

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

        public static IList<Role> LoadRolesData()
        {
            return new List<Role>
        {
            new Role { ID = Role_Admin, Name = "Admin" },
            new Role { ID = Role_User, Name = "User" }
        };
        }

        public static IList<User> LoadUsersData()
        {
            var staticCreatedAt = new DateTime(2025, 8, 5, 10, 0, 0, DateTimeKind.Utc);

            return new List<User>
            {
                new User { ID = User_Admin, Username = "admin", Email = "admin@example.com", PasswordHash = "$2a$11$IAzmX9gT.qkqm45lMnyh/uE0PZ793GOyIKEEn3dNdbAC1cfxcbFVa", CreatedAt = staticCreatedAt}
            };
        }

        public static IList<UserRole> LoadUserRolesData()
        {
            return new List<UserRole>
            {
                new UserRole { UserID = User_Admin, RoleID = Role_Admin }
            };
        }

        public static IList<Book> LoadBookData()
        {
            var staticCreatedAt = new DateTime(2025, 8, 5, 10, 0, 0, DateTimeKind.Utc);

            return new List<Book>
        {
            new Book { ID = Book_Gatsby, Title = "The Great Gatsby", Description = "A novel set in the Roaring Twenties.", ISBN = "9780743273565", CoverImageUrl = "https://example.com/great-gatsby.jpg", PublicationDate = new DateTime(1925, 4, 10), Publisher = "Scribner", PageCount = 180, AverageRating = 4.20m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { ID = Book_Mockingbird, Title = "To Kill a Mockingbird", Description = "A novel about racial injustice in the Deep South.", ISBN = "9780061120084", CoverImageUrl = "https://example.com/to-kill-a-mockingbird.jpg", PublicationDate = new DateTime(1960, 7, 11), Publisher = "J.B. Lippincott & Co.", PageCount = 281, AverageRating = 4.30m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { ID = Book_1984, Title = "1984", Description = "A dystopian novel about totalitarianism.", ISBN = "9780451524935", CoverImageUrl = "https://example.com/1984.jpg", PublicationDate = new DateTime(1949, 6, 8), Publisher = "Secker & Warburg", PageCount = 328, AverageRating = 4.40m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { ID = Book_Pride, Title = "Pride and Prejudice", Description = "A romantic novel about love and social class.", ISBN = "9780141439518", CoverImageUrl = "https://example.com/pride-and-prejudice.jpg", PublicationDate = new DateTime(1813, 1, 28), Publisher = "Penguin Classics", PageCount = 432, AverageRating = 4.25m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { ID = Book_Hobbit, Title = "The Hobbit", Description = "A fantasy adventure about Bilbo Baggins.", ISBN = "9780547928227", CoverImageUrl = "https://example.com/the-hobbit.jpg", PublicationDate = new DateTime(1937, 9, 21), Publisher = "Houghton Mifflin", PageCount = 310, AverageRating = 4.27m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { ID = Book_Dune, Title = "Dune", Description = "A science fiction epic about a desert planet.", ISBN = "9780441172719", CoverImageUrl = "https://example.com/dune.jpg", PublicationDate = new DateTime(1965, 8, 1), Publisher = "Ace Books", PageCount = 412, AverageRating = 4.21m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { ID = Book_Sapiens, Title = "Sapiens: A Brief History of Humankind", Description = "A nonfiction exploration of human history.", ISBN = "9780062316097", CoverImageUrl = "https://example.com/sapiens.jpg", PublicationDate = new DateTime(2014, 9, 9), Publisher = "Harper", PageCount = 443, AverageRating = 4.38m, IsAvailable = true, CreatedAt = staticCreatedAt },
            new Book { ID = Book_DaVinci, Title = "The Da Vinci Code", Description = "A thriller involving a religious conspiracy.", ISBN = "9780307277671", CoverImageUrl = "https://example.com/da-vinci-code.jpg", PublicationDate = new DateTime(2003, 3, 18), Publisher = "Doubleday", PageCount = 454, AverageRating = 3.85m, IsAvailable = true, CreatedAt = staticCreatedAt }
        };
        }

        public static List<Author> LoadAuthorsData()
        {
            return new List<Author>
        {
            new Author { ID = Author_Fitzgerald, Name = "F. Scott Fitzgerald" },
            new Author { ID = Author_Lee, Name = "Harper Lee" },
            new Author { ID = Author_Orwell, Name = "George Orwell" },
            new Author { ID = Author_Austen, Name = "Jane Austen" },
            new Author { ID = Author_Tolkien, Name = "J.R.R. Tolkien" },
            new Author { ID = Author_Herbert, Name = "Frank Herbert" },
            new Author { ID = Author_Harari, Name = "Yuval Noah Harari" },
            new Author { ID = Author_Brown, Name = "Dan Brown" }
        };
        }

        public static List<BookAuthor> LoadBookAuthorsData()
        {
            return new List<BookAuthor>
        {
             new BookAuthor { BookID = Book_Gatsby, AuthorID = Author_Fitzgerald },
             new BookAuthor { BookID = Book_Mockingbird, AuthorID = Author_Lee },
             new BookAuthor { BookID = Book_1984, AuthorID = Author_Orwell },
             new BookAuthor { BookID = Book_Pride, AuthorID = Author_Austen },
             new BookAuthor { BookID = Book_Hobbit, AuthorID = Author_Tolkien },
             new BookAuthor { BookID = Book_Dune, AuthorID = Author_Herbert },
             new BookAuthor { BookID = Book_Sapiens, AuthorID = Author_Harari },
             new BookAuthor { BookID = Book_DaVinci, AuthorID = Author_Brown }
        };
        }

        public static List<Genre> LoadGenresData()
        {
            return new List<Genre>
        {
            new Genre { ID = 1, Name = "Fiction" },
            new Genre { ID = 2, Name = "Nonfiction" },
            new Genre { ID = 3, Name = "Science Fiction" },
            new Genre { ID = 4, Name = "Fantasy" },
            new Genre { ID = 5, Name = "Mystery" },
            new Genre { ID = 6, Name = "Thriller" },
            new Genre { ID = 7, Name = "Romance" },
            new Genre { ID = 8, Name = "Historical Fiction" },
            new Genre { ID = 9, Name = "Biography" },
            new Genre { ID = 10, Name = "Autobiography" },
            new Genre { ID = 11, Name = "Self-Help" },
            new Genre { ID = 12, Name = "Business" },
            new Genre { ID = 13, Name = "Science" },
            new Genre { ID = 14, Name = "History" },
            new Genre { ID = 15, Name = "Young Adult" },
            new Genre { ID = 16, Name = "Children" },
            new Genre { ID = 17, Name = "Poetry" },
            new Genre { ID = 18, Name = "Horror" },
            new Genre { ID = 19, Name = "Adventure" },
            new Genre { ID = 20, Name = "Crime" },
            new Genre { ID = 21, Name = "Literary Criticism" },
            new Genre { ID = 22, Name = "Cooking" },
            new Genre { ID = 23, Name = "Travel" },
            new Genre { ID = 24, Name = "Philosophy" },
            new Genre { ID = 25, Name = "Religion" }
        };
        }

        public static List<BookGenre> LoadBookGenresData()
        {
            return new List<BookGenre>
        {
            new BookGenre { BookID = Book_Gatsby, GenreID = 1 }, // Fiction
            new BookGenre { BookID = Book_Mockingbird, GenreID = 1 }, // Fiction
            new BookGenre { BookID = Book_1984, GenreID = 3 }, // Science Fiction
            new BookGenre { BookID = Book_Pride, GenreID = 7 }, // Romance
            new BookGenre { BookID = Book_Pride, GenreID = 8 }, // Historical Fiction
            new BookGenre { BookID = Book_Hobbit, GenreID = 4 }, // Fantasy
            new BookGenre { BookID = Book_Hobbit, GenreID = 19 }, // Adventure
            new BookGenre { BookID = Book_Dune, GenreID = 3 }, // Science Fiction
            new BookGenre { BookID = Book_Sapiens, GenreID = 2 }, // Nonfiction
            new BookGenre { BookID = Book_Sapiens, GenreID = 14 }, // History
            new BookGenre { BookID = Book_DaVinci, GenreID = 6 }, // Thriller
            new BookGenre { BookID = Book_DaVinci, GenreID = 5 }  // Mystery
        };
        }
    }
}
