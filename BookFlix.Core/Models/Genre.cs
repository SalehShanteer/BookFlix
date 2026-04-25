
namespace BookFlix.Core.Models
{
    public class Genre : LookupEntity
    {
        public List<Book> Books { get; set; }
    }
}