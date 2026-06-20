namespace BookFlix.Core.Enums
{
    public class GeneralEnums
    {
        public enum EventType
        {
            Logout = 0,
            Login = 1
        }

        public enum ErrorType
        {
            Failure = 0,     // A general failure (Default to 400)
            Validation = 1,  // Bad input from the user (Maps to 400)
            NotFound = 2,    // Missing resource (Maps to 404)
            Conflict = 3,     // e.g., Email already exists (Maps to 409)
            Unauthorized = 4,
            Forbidden = 5,    // e.g., User doesn't have permission (Maps to 403)
        }
    }
}
