
using System.ComponentModel.DataAnnotations;

namespace BookFlix.Core.Models
{
    public class User : Entity
    {
        [MinLength(4, ErrorMessage = "Username length should be at least 4 characters.")]
        public string? Username { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        public string? PasswordHash { get; set; } // Using BCrypt
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}