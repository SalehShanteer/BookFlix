using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserUpdateEmailDto
    {
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string NewEmail { get; set; } = null!;
    }

}
