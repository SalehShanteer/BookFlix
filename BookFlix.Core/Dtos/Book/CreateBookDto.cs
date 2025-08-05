using System.ComponentModel.DataAnnotations;

namespace BookFlix.Core.Dtos.Book
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [RegularExpression(@"^\d{10}|\d{13}$", ErrorMessage = "ISBN must be 10 or 13 digits")]
        public string? ISBN { get; set; }

        [Url(ErrorMessage = "CoverImageUrl must be a valid URL")]
        public string? CoverImageUrl { get; set; }

        public DateTime? PublicationDate { get; set; }

        [StringLength(100, ErrorMessage = "Publisher cannot exceed 100 characters")]
        public string? Publisher { get; set; }

        [Range(1, 10000, ErrorMessage = "PageCount must be between 1 and 10,000")]
        public int? PageCount { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "At least one author is required")]
        [MinLength(1, ErrorMessage = "At least one author is required")]
        public List<int> AuthorIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "At least one Genre is required")]
        [MinLength(1, ErrorMessage = "At least one Genre is required")]
        public List<int> GenreIds { get; set; } = new List<int>();

        [StringLength(500, ErrorMessage = "FileLocation cannot exceed 500 characters")]
        public string? FileLocation { get; set; }

    }
}
