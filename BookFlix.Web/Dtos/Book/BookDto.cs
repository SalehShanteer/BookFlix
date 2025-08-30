using BookFlix.Web.Dtos.Author;
using BookFlix.Web.Dtos.Genre;

namespace BookFlix.Web.Dtos.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ISBN { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? Publisher { get; set; }
        public int? PageCount { get; set; }
        public decimal? AverageRating { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? FileLocation { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
    }
}
