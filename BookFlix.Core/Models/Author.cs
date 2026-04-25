namespace BookFlix.Core.Models
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }
}