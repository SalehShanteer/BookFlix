using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserCreateDto
    {

        [MinLength(4, ErrorMessage = "Username length should be at least 4 characters.")]
        public string Username { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
