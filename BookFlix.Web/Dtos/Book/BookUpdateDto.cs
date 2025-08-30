using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.Book
{
    public class BookUpdateDto : BookCreateDto
    {
        [Required(ErrorMessage = "Id is required")]

        public int Id { get; set; }
    }
}
