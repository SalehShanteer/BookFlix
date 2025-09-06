namespace BookFlix.Core.Helpers
{
    public static class PasswordHelper
    {
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;
            bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecial = true;
                if (hasUpper && hasLower && hasDigit && hasSpecial)
                    return true;
            }
            return false;
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
