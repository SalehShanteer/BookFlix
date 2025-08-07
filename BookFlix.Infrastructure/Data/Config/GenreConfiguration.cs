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

            builder.HasData(_LoadGenresData());
        }

        private static List<Genre> _LoadGenresData()
        {
            var genres = new List<Genre>
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

            return genres;
        }
    }
}
