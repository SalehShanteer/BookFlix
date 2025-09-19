namespace BookFlix.Web.Dtos.User
{
    public class UserUpdatePasswordDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

}
