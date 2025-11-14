using BookFlix.Core.Helpers;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;
using static BookFlix.Core.Enums.GeneralEnums;

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

        private async Task<bool> IsUsernameUsedBefore(string username)
            => await _userRepository.IsUsernameExist(username);

        private async Task<bool> IsEmailUsedBefore(string email)
            => await _userRepository.IsEmailExist(email);

        private async Task ValidateUsername(string username, ValidationResult validationResult)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogErrorForValidation("UserEmpty", validationResult);
            }
            else if (username.Length < 4)
            {
                _logger.LogErrorForValidation("UsernameLengthTooShort", validationResult);
            }
            else if (await IsUsernameUsedBefore(username))
            {
                _logger.LogErrorForValidation("UsernameUsed", validationResult);
            }
        }

        private void ValidatePassword(string password, ValidationResult validationResult)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogErrorForValidation("PasswordEmpty", validationResult);
            }
            else if (!PasswordHelper.IsStrongPassword(password))
            {
                _logger.LogErrorForValidation("PasswordWeak", validationResult);
            }
        }

        private async Task ValidateEmail(string email, ValidationResult validationResult)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogErrorForValidation("EmailEmpty", validationResult);
            }
            else if (await IsEmailUsedBefore(email))
            {
                _logger.LogErrorForValidation("EmailUsed", validationResult);
            }
        }

        private async Task<ValidationResult> ValidateUser(User user)
        {
            var validationResult = new ValidationResult();

            await ValidateUsername(user.Username ?? string.Empty, validationResult);
            await ValidateEmail(user.Email ?? string.Empty, validationResult);
            ValidatePassword(user.PasswordHash ?? string.Empty, validationResult);

            return validationResult;
        }

        private (ValidationResult Result, User? User) ReturnUserNotFound(ValidationResult validationResult)
        {
            _logger.LogErrorForValidation($"UserNotFound", validationResult);
            validationResult.StatusCode = enStatusCode.NotFound;
            return (validationResult, null);
        }

        private (ValidationResult Result, User? User) ReturnBadRequest(ValidationResult validationResult)
        {
            validationResult.StatusCode = enStatusCode.BadRequest;
            return (validationResult, null);
        }

        private (ValidationResult Result, string? AccessToken, string? RefreshToken) UnauthorizedRequest(string message)
        {
            var validationResult = new ValidationResult();
            _logger.LogErrorForValidation(message, validationResult);
            validationResult.StatusCode = enStatusCode.Unauthorized;
            return (validationResult, null, null);
        }

        public async Task<(ValidationResult Result, User? User)> AddUserAsync(User user)
        {
            var validationResult = await ValidateUser(user);

            if (!validationResult.IsValid)
            {
                validationResult.StatusCode = enStatusCode.BadRequest;
                return (validationResult, null);
            }

            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash!);
            var userToAdd = await _userRepository.AddAsync(user);

            return (validationResult, userToAdd);
        }

        public async Task<(ValidationResult Result, User? User)> AddUserAsAdminAsync(User user)
        {
            user.Role = "Admin";
            return await AddUserAsync(user);
        }

        public async Task<IReadOnlyCollection<User>> GetAllUsersAsync()
            => await _userRepository.GetAllAsync();

        public async Task<User?> GetUserByIdAsync(int id)
            => await _userRepository.GetByIdAsync(id);

        public async Task<User?> GetUserByRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            if (refreshToken is null || !refreshToken.IsActive) return null;

            return await _userRepository.GetByIdWithRelationsAsync(refreshToken.UserId);
        }

        public async Task<(ValidationResult Result, User? User)> UpdateUserPasswordAsync(User user, string oldPassword)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            var validationResult = new ValidationResult();

            if (existingUser is null) return ReturnUserNotFound(validationResult);

            if (!PasswordHelper.VerifyPassword(oldPassword, existingUser.PasswordHash ?? string.Empty))
            {
                _logger.LogErrorForValidation("Old password is incorrect.", validationResult);
            }
            else if (oldPassword == user.PasswordHash)
            {
                _logger.LogErrorForValidation("New password cannot be the same as the old password.", validationResult);
            }
            else
            {
                ValidatePassword(user.PasswordHash ?? string.Empty, validationResult);
            }

            if (!validationResult.IsValid) ReturnBadRequest(validationResult);

            existingUser.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash!);
            existingUser.UpdatedAt = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return (validationResult, updatedUser);
        }

        public async Task<(ValidationResult Result, User? User)> UpdateUserUsernameAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            var validationResult = new ValidationResult();

            if (existingUser is null) return ReturnUserNotFound(validationResult);
            if (existingUser.Username != user.Username) await ValidateUsername(user.Username ?? string.Empty, validationResult);
            if (!validationResult.IsValid) ReturnBadRequest(validationResult);

            existingUser.Username = user.Username ?? existingUser.Username;
            existingUser.UpdatedAt = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return (validationResult, updatedUser);
        }

        public async Task<(ValidationResult Result, User? User)> UpdateUserEmailAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            var validationResult = new ValidationResult();

            if (existingUser is null) return ReturnUserNotFound(validationResult);
            if (existingUser.Email != user.Email) await ValidateEmail(user.Email ?? string.Empty, validationResult);
            if (!validationResult.IsValid) ReturnBadRequest(validationResult);

            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.UpdatedAt = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return (validationResult, updatedUser);
        }

        public async Task<(ValidationResult Result, string? AccessToken, string? RefreshToken)> UpdateUserRefreshToken(string refreshToken)
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

            return (new ValidationResult(), newAccessToken, newRefreshToken.Token);
        }
    }
}
