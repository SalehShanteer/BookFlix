using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.Book
{
    public class BookUpdateDto : BookCreateDto
    {
        [Required(ErrorMessage = "IdRequired")]
        public Guid Id { get; set; }
    }
}
