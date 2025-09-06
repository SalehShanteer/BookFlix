using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Web.Dtos.UserLog
{
    public class UserLogDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public enEventType EventType { get; set; } = enEventType.Login; // 0 => Logout, 1 => Login
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public bool Success { get; set; }
    }
}
