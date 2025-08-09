namespace BookFlix.Core.Services.Validation
{
    public enum enStatusCode
    {
        BadRequest = 1,
        NotFound = 2,
        InternalServerError = 3
    }

    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public enStatusCode StatusCode { get; set; } = enStatusCode.BadRequest;
        public List<string> Errors { get; set; } = new List<string>();
    }
}


