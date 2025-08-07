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

            builder.HasData(_LoadAuthorsData());
        }

        private static List<Author> _LoadAuthorsData()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "F. Scott Fitzgerald" },
                new Author { Id = 2, Name = "Harper Lee" },
                new Author { Id = 3, Name = "George Orwell" },
                new Author { Id = 4, Name = "Jane Austen" },
                new Author { Id = 5, Name = "J.R.R. Tolkien" },
                new Author { Id = 6, Name = "Frank Herbert" },
                new Author { Id = 7, Name = "Yuval Noah Harari" },
                new Author { Id = 8, Name = "Dan Brown" }
            };
            return authors;
        }
    }

}
