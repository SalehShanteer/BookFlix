namespace BookFlix.Core.Enums
{

    public class GeneralEnums
    {
        public enum enEventType
        {
            Logout = 0,
            Login = 1
        }

        public enum enStatusCode
        {
            Ok = 0,
            BadRequest = 1,
            NotFound = 2,
            InternalServerError = 3,
            Unauthorized = 4
        }

    }
}
