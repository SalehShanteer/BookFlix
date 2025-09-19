using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserUpdateUsernameDto
    {
        [MinLength(4, ErrorMessage = "Username length should be at least 4 characters.")]
        public string NewUsername { get; set; } = null!;
    }

}
