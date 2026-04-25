using BookFlix.Core.Helpers;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<Result<User>> AddUserAsync(User user)
        {
            var result = await ValidateUser(user);

            if (result.IsFailure) return Result.Failure<User>(result.Error);

            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
            var userToAdd = await _userRepository.AddAsync(user);

            return Result.Success(userToAdd);
        }

        public async Task<Result<User>> AddUserAsAdminAsync(User user)
        {
            user.Role = "Admin";
            return await AddUserAsync(user);
        }

        public async Task<IReadOnlyCollection<User>> GetAllUsersAsync() => await _userRepository.GetAllAsync();

        public async Task<Result<User>> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null) return Result.Failure<User>(Error.NotFound("UserNotFound"));
            return Result.Success(user);
        } 

        public async Task<User> GetUserByRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            if (refreshToken is null || !refreshToken.IsActive) return null;

            return await _userRepository.GetByIdWithRelationsAsync(refreshToken.UserId);
        }

        public async Task<Result> UpdateUserPasswordAsync(Guid userID, string oldPassword, string newPassword)
        {
            var existingUser = await _userRepository.GetByIdAsync(userID);

            if (existingUser is null) return ReturnUserNotFound();

            if (!PasswordHelper.VerifyPassword(oldPassword, existingUser.PasswordHash))
            {
                _logger.LogWarning("Old password is incorrect.");
                return Result.Failure(Error.Validation("OldPasswordIncorrect"));
            }
            else if (oldPassword == newPassword)
            {
                _logger.LogWarning("New password cannot be the same as the old password.");
                return Result.Failure<User>(Error.Validation("NewPasswordEqualsOldPassword"));
            }
            else
            {
                var passwordResult = ValidatePassword(newPassword);
                if (passwordResult.IsFailure) return passwordResult;
            }

            existingUser.PasswordHash = PasswordHelper.HashPassword(newPassword);
            existingUser.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(existingUser);

            return Result.Success();
        }

        public async Task<Result<User>> UpdateUserUsernameAsync(Guid id, string username)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser is null) return ReturnUserNotFound();
            if (existingUser.Username != username)
            {
                var usernameResult = await ValidateUsername(username);
                if (usernameResult.IsFailure) return Result.Failure<User>(Error.Validation(usernameResult.Error.Key));
            }

            existingUser.Username = username;
            existingUser.UpdatedAt = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return Result.Success(updatedUser);
        }

        public async Task<Result<User>> UpdateUserEmailAsync(Guid id, string email)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser is null) return ReturnUserNotFound();

            if (existingUser.Email != email)
            {
                var emailResult = await ValidateEmail(email);
                if (emailResult.IsFailure) return Result.Failure<User>(Error.Validation(emailResult.Error.Key));
            }

            existingUser.Email = email;
            existingUser.UpdatedAt = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(existingUser);

            return Result.Success(updatedUser);
        }

        public async Task<Result<(string AccessToken, string RefreshToken)>> UpdateUserRefreshToken(string refreshToken)
        {
            var user = await GetUserByRefreshToken(refreshToken);
            if (user is null) return UnauthorizedRequest("InvalidRefreshToken");

            var storedToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
            if (storedToken is null || !storedToken.IsActive)
                return UnauthorizedRequest("RefreshTokenExpiredOrRevoked");

            // Issue new tokens
            var newAccessToken = _jwtService.GenerateJwtToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken(user.Id, storedToken.ExpiresAt);

            // Revoke old token
            storedToken.RevokedAt = DateTime.UtcNow;
            user.RefreshTokens.Add(newRefreshToken);

            await _userRepository.UpdateAsync(user);

            return Result.Success((newAccessToken, newRefreshToken.Token));
        }

        private async Task<bool> IsUsernameUsedBefore(string username) => await _userRepository.IsUsernameExist(username);

        private async Task<bool> IsEmailUsedBefore(string email) => await _userRepository.IsEmailExist(email);

        private async Task<Result> ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("Validation failed: The username provided is empty.");
                return Result.Failure(Error.Validation("UsernameIsEmpty"));
            }

            if (username.Length < 4)
            {
                _logger.LogWarning("Validation failed: The provided username '{Username}' is too short.", username);
                return Result.Failure(Error.Validation("UsernameLengthTooShort"));
            }

            if (await IsUsernameUsedBefore(username))
            {
                _logger.LogWarning("Validation failed: The requested username '{Username}' is already taken.", username);
                return Result.Failure(Error.Conflict("UsernameUsed"));
            }

            return Result.Success();
        }

        private async Task<Result> ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Validation failed: The email address provided is empty.");
                return Result.Failure(Error.Validation("EmailEmpty"));
            }

            if (await IsEmailUsedBefore(email))
            {
                _logger.LogWarning("Validation failed: The provided email address '{Email}' is already registered.", email);
                return Result.Failure(Error.Conflict("EmailUsed"));
            }

            return Result.Success();
        }

        private Result ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Validation failed: The password provided is empty or contains only whitespace.");
                return Result.Failure(Error.Validation("PasswordEmpty"));
            }

            if (!PasswordHelper.IsStrongPassword(password))
            {
                _logger.LogWarning("Validation failed: The provided password does not meet the minimum security requirements.");
                return Result.Failure(Error.Validation("PasswordWeak"));
            }

            return Result.Success();
        }
        private async Task<Result> ValidateUser(User user)
        {
            var usernameResult = await ValidateUsername(user.Username);
            if (usernameResult.IsFailure) return usernameResult;

            var emailResult = await ValidateEmail(user.Email);
            if (emailResult.IsFailure) return emailResult;

            var passwordResult = ValidatePassword(user.PasswordHash);
            if (passwordResult.IsFailure) return passwordResult;

            return Result.Success();
        }

        private Result<User> ReturnUserNotFound()
        {
            _logger.LogWarning("The user is not found");

            return Result.Failure<User>(Error.NotFound("UserNotFound"));
        }

        private Result<(string AccessToken, string RefreshToken)> UnauthorizedRequest(string message)
        {
            _logger.LogWarning(message);
            return Result.Failure<(string, string)>(Error.Unauthorized(message));
        }
    }
}