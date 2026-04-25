namespace BookFlix.Web.Dtos.User
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
