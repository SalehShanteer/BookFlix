namespace BookFlix.Core.Models
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => RevokedAt is null && !IsExpired;

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}