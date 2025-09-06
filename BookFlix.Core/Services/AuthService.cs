using BookFlix.Core.Helpers;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;
using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLogRepository _userLogRepository;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IUserRepository userRepository, IUserLogRepository userLogRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _userLogRepository = userLogRepository;
            _logger = logger;
        }

        private void LogLoginAttempt(int userId, bool isSuccess, string ipAddress)
        {
            var log = new UserLog
            {
                UserId = userId,
                EventType = enEventType.Login,
                Success = isSuccess,
                IpAddress = ipAddress,
            };
            _userLogRepository.AddAsync(log);
        }

        public async Task<(User? User, ValidationResult Result)> LoginAsync(string email, string password, string ipAddress)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogErrorForValidation("Email is empty or whitespace", result);
                result.StatusCode = enStatusCode.BadRequest;
                return (null, result);
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                _logger.LogErrorForValidation("User not found", result);
                result.StatusCode = enStatusCode.NotFound;
                return (null, result);
            }

            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash!))
            {
                _logger.LogErrorForValidation("Invalid password", result);
                result.StatusCode = enStatusCode.Unauthorized;
                LogLoginAttempt(user.Id, false, ipAddress);

                return (null, result);
            }

            LogLoginAttempt(user.Id, true, ipAddress);
            return (user, result);
        }

    }
}
