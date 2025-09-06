using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Helpers
{
    public static class LoggerExtensions
    {
        public static void LogErrorForValidation(this ILogger logger, string message, ValidationResult result)
        {
            result.Errors.Add(message);
            logger.LogError(message);
        }

        public static void LogExceptionErrorForValidation(this ILogger logger, Exception? ex, string message, ValidationResult result)
        {
            result.Errors.Add(message);
            logger.LogError(ex, message);
        }
    }
}
