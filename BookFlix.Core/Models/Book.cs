
namespace BookFlix.Core.Models
{


    public class Book : Entity
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ISBN { get; set; }

        public string? Genre { get; set; }

        public string? CoverImageUrl { get; set; }

        public DateTime? PublicationDate { get; set; }

        public string? Publisher { get; set; }

        public int? PageCount { get; set; }

        public decimal? AverageRating { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? FileLocation { get; set; }

        public ICollection<Author> Authors { get; set; } = new List<Author>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        //public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
}