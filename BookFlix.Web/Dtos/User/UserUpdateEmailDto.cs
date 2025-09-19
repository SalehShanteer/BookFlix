using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserUpdateEmailDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string NewEmail { get; set; } = null!;
    }

}
