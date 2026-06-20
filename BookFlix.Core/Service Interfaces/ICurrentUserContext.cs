namespace BookFlix.Core.Service_Interfaces
{
    public interface ICurrentUserContext
    {
        bool IsAuthenticated { get; }

        bool IsAdmin { get; }

        Guid UserID { get; }

        string GetClaim(string claimType);

        T? GetClaim<T>(string claimType) where T : struct;
        IEnumerable<T> GetClaims<T>(string claimType);

        bool HasRole(string role);
        bool HasClaim(string claimType, string value);
    }
}
