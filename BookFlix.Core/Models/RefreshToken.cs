namespace BookFlix.Core.Models
{
    public class RefreshToken : Entity
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => RevokedAt is null && !IsExpired;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}