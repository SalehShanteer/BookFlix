using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Web.Dtos.UserLog
{
    public class UserLogDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public EventType EventType { get; set; } = EventType.Login; // 0 => Logout, 1 => Login
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public bool Success { get; set; }
    }
}
