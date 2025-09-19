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
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IUserRepository userRepository, IUserLogRepository userLogRepository, ILogger<AuthService> logger, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _userLogRepository = userLogRepository;
            _logger = logger;
            _jwtService = jwtService;
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

        private (ValidationResult Result, string? AccessToken, string? RefreshToken) ReturnTokens(User user, ValidationResult result)
        {
            string accessToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            user.RefreshTokens.Add(refreshToken);
            _userRepository.UpdateAsync(user);

            return (result, accessToken, refreshToken.Token);
        }

        public async Task<(ValidationResult Result, string? AccessToken, string? RefreshToken)>
            LoginAsync(string email, string password, string ipAddress)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogErrorForValidation("Email is empty or whitespace", result);
                result.StatusCode = enStatusCode.BadRequest;
                return (result, null, null);
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                _logger.LogErrorForValidation("User not found", result);
                result.StatusCode = enStatusCode.NotFound;
                return (result, null, null);
            }

            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash!))
            {
                _logger.LogErrorForValidation("Invalid password", result);
                result.StatusCode = enStatusCode.Unauthorized;
                LogLoginAttempt(user.Id, false, ipAddress);

                return (result, null, null);
            }

            LogLoginAttempt(user.Id, true, ipAddress);
            return ReturnTokens(user, result);
        }



    }
}
