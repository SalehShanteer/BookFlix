namespace BookFlix.Web.Dtos.User
{
    public class UserUpdatePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
