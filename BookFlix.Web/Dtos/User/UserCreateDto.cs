using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserCreateDto
    {

        [MinLength(4, ErrorMessage = "UsernameLengthTooShort")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
