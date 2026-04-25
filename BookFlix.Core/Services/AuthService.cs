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

        public async Task<Result<(string AccessToken, string RefreshToken)>> LoginAsync(string email, string password, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("Email is empty or whitespace");
                return Result.Failure<(string, string)>(Error.Validation("EmailIsEmpty"));
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                _logger.LogError("User not found");
                return Result.Failure<(string, string)>(Error.NotFound("UserNotFound"));
            }

            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                _logger.LogError("Invalid password");
                LogLoginAttempt(user.Id, false, ipAddress);

                return Result.Failure<(string, string)>(Error.Validation("InvalidPassword"));
            }

            LogLoginAttempt(user.Id, true, ipAddress);
            return ReturnTokens(user);
        }

        private void LogLoginAttempt(Guid userId, bool isSuccess, string ipAddress)
        {
            var log = new UserLog
            {
                UserId = userId,
                EventType = EventType.Login,
                Success = isSuccess,
                IpAddress = ipAddress,
            };
            _userLogRepository.AddAsync(log);
        }

        private Result<(string AccessToken, string RefreshToken)> ReturnTokens(User user)
        {
            string accessToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);
            user.RefreshTokens.Add(refreshToken);
            _userRepository.UpdateAsync(user);

            return Result.Success((accessToken, refreshToken.Token));
        }
    }
}
