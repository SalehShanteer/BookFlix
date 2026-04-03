using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        public string Password { get; set; }
    }
}
