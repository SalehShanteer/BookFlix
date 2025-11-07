using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.Book
{
    public class BookUpdateDto : BookCreateDto
    {
        [Required(ErrorMessage = "IdRequired")]
        public int Id { get; set; }
    }
}
