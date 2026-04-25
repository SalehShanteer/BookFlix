using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.Book
{
    public class BookUpdateDto : BookCreateDto
    {
        [Required(ErrorMessage = "IDRequired")]
        public Guid ID { get; set; }
    }
}
