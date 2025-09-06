using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Core.Models
{
    public class UserLog : Entity
    {
        public int UserId { get; set; }
        public enEventType EventType { get; set; } = enEventType.Login; // 0 => Logout, 1 => Login
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public bool Success { get; set; }

        public User User { get; set; } = null!;
    }
}