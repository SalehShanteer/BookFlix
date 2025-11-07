using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserUpdateUsernameDto
    {
        [MinLength(4, ErrorMessage = "UsernameLengthTooShort")]
        public string NewUsername { get; set; } = null!;
    }

}
