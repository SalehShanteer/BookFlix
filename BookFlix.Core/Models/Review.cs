namespace BookFlix.Core.Models
{
    public class Review : BaseEntity
    {
        public string Content { get; set; }
        public byte Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Guid BookID { get; set; }
        public Book Book { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}