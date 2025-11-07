
using System.ComponentModel.DataAnnotations;

namespace BookFlix.Core.Models
{
    public class User : Entity
    {
        [MinLength(4, ErrorMessage = "UsernameLengthTooShort")]
        public string? Username { get; set; }
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string? Email { get; set; }
        public string? PasswordHash { get; set; } // Using BCrypt
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}