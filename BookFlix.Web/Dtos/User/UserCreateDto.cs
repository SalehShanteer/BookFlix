using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserCreateDto
    {

        [MinLength(4, ErrorMessage = "UsernameLengthTooShort")]
        public string Username { get; set; } = null!;

        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
