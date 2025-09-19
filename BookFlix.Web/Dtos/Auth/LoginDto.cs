using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 60 characters")]
        public string Password { get; set; } = null!;
    }

}
