namespace BookFlix.Core.Models
{
    public class Review : Entity
    {
        public string? Content { get; set; }
        public byte Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}