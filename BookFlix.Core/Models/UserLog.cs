using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Core.Models
{
    public class UserLog : BaseEntity
    {
        public Guid UserID { get; set; }
        public EventType EventType { get; set; } = EventType.Login; // 0 => Logout, 1 => Login
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public bool Success { get; set; }

        public User User { get; set; }
    }
}