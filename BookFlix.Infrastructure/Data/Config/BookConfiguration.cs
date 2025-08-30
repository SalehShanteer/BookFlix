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
                builder.HasIndex(b => b.ISBN)
                    .IsUnique();

                builder.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(150);
                builder.Property(b => b.Description)
                    .HasMaxLength(1000);
                builder.Property(b => b.ISBN)
                    .HasMaxLength(20);
                builder.Property(b => b.CoverImageUrl)
                    .HasMaxLength(500);
                builder.Property(b => b.PublicationDate)
                    .HasColumnType("datetime");
                builder.Property(b => b.Publisher)
                    .HasMaxLength(100);
                builder.Property(b => b.PageCount)
                    .IsRequired(false);
                builder.Property(b => b.AverageRating)
                    .HasColumnType("decimal(3, 2)");
                builder.Property(b => b.IsAvailable)
                    .IsRequired();
                builder.Property(b => b.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                builder.Property(b => b.UpdatedAt)
                    .IsRequired(false);

                builder.Property(b => b.FileLocation)
                    .IsRequired(false)
                    .HasMaxLength(50);

                // Relationships
                builder.HasMany(b => b.Authors)
                    .WithMany(a => a.Books)
                    .UsingEntity<BookAuthor>();
                builder.HasMany(b => b.Reviews)
                    .WithOne(r => r.Book)
                    .HasForeignKey(r => r.BookId);
                builder.HasMany(b => b.Genres)
                    .WithMany(g => g.Books)
                    .UsingEntity<BookGenre>();

                // Seed data
                builder.HasData(_LoadBookData());
            }

            private static IList<Book> _LoadBookData()
            {
                var staticCreatedAt = new DateTime(2025, 8, 5, 10, 0, 0, DateTimeKind.Utc); // Hardcoded UTC timestamp
                var books = new List<Book>
            {
                new Book { Id = 1, Title = "The Great Gatsby", Description = "A novel set in the Roaring Twenties.", ISBN = "9780743273565", CoverImageUrl = "https://example.com/great-gatsby.jpg", PublicationDate = new DateTime(1925, 4, 10), Publisher = "Scribner", PageCount = 180, AverageRating = 4.20m, IsAvailable = true, CreatedAt = staticCreatedAt },
                new Book { Id = 2, Title = "To Kill a Mockingbird", Description = "A novel about racial injustice in the Deep South.", ISBN = "9780061120084", CoverImageUrl = "https://example.com/to-kill-a-mockingbird.jpg", PublicationDate = new DateTime(1960, 7, 11), Publisher = "J.B. Lippincott & Co.", PageCount = 281, AverageRating = 4.30m, IsAvailable = true, CreatedAt = staticCreatedAt },
                new Book { Id = 3, Title = "1984", Description = "A dystopian novel about totalitarianism.", ISBN = "9780451524935", CoverImageUrl = "https://example.com/1984.jpg", PublicationDate = new DateTime(1949, 6, 8), Publisher = "Secker & Warburg", PageCount = 328, AverageRating = 4.40m, IsAvailable = true, CreatedAt = staticCreatedAt },
                new Book { Id = 4, Title = "Pride and Prejudice", Description = "A romantic novel about love and social class.", ISBN = "9780141439518", CoverImageUrl = "https://example.com/pride-and-prejudice.jpg", PublicationDate = new DateTime(1813, 1, 28), Publisher = "Penguin Classics", PageCount = 432, AverageRating = 4.25m, IsAvailable = true, CreatedAt = staticCreatedAt },
                new Book { Id = 5, Title = "The Hobbit", Description = "A fantasy adventure about Bilbo Baggins.", ISBN = "9780547928227", CoverImageUrl = "https://example.com/the-hobbit.jpg", PublicationDate = new DateTime(1937, 9, 21), Publisher = "Houghton Mifflin", PageCount = 310, AverageRating = 4.27m, IsAvailable = true, CreatedAt = staticCreatedAt },
                new Book { Id = 6, Title = "Dune", Description = "A science fiction epic about a desert planet.", ISBN = "9780441172719", CoverImageUrl = "https://example.com/dune.jpg", PublicationDate = new DateTime(1965, 8, 1), Publisher = "Ace Books", PageCount = 412, AverageRating = 4.21m, IsAvailable = true, CreatedAt = staticCreatedAt },
                new Book { Id = 7, Title = "Sapiens: A Brief History of Humankind", Description = "A nonfiction exploration of human history.", ISBN = "9780062316097", CoverImageUrl = "https://example.com/sapiens.jpg", PublicationDate = new DateTime(2014, 9, 9), Publisher = "Harper", PageCount = 443, AverageRating = 4.38m, IsAvailable = true, CreatedAt = staticCreatedAt },
                new Book { Id = 8, Title = "The Da Vinci Code", Description = "A thriller involving a religious conspiracy.", ISBN = "9780307277671", CoverImageUrl = "https://example.com/da-vinci-code.jpg", PublicationDate = new DateTime(2003, 3, 18), Publisher = "Doubleday", PageCount = 454, AverageRating = 3.85m, IsAvailable = true, CreatedAt = staticCreatedAt }
            };
                return books;
            }

        }
    }
}


