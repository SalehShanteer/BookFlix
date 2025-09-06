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

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
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
                _logger.LogErrorForValidation("User name is empty.", validationResult);
            }
            else if (username.Length < 4)
            {
                _logger.LogErrorForValidation("Username length should be at least 4 characters.", validationResult);
            }
            else if (await IsUsernameUsedBefore(username))
            {
                _logger.LogErrorForValidation("Username is already in use.", validationResult);
            }
        }

        private void ValidatePassword(string password, ValidationResult validationResult)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogErrorForValidation("Password is empty.", validationResult);
            }
            else if (password.Length < 8)
            {
                _logger.LogErrorForValidation("Password length should be at least 8 characters.", validationResult);
            }
            else if (password.Length > 60)
            {
                _logger.LogErrorForValidation("Password length should not exceed 60 characters.", validationResult);
            }
            else if (!PasswordHelper.IsStrongPassword(password))
            {
                _logger.LogErrorForValidation("Password is not strong enough. It should contain at least one uppercase letter, one lowercase letter, one digit, and one special character.", validationResult);
            }
        }

        private async Task ValidateEmail(string email, ValidationResult validationResult)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogErrorForValidation("Email is empty.", validationResult);
            }
            else if (await IsEmailUsedBefore(email))
            {
                _logger.LogErrorForValidation("Email is already in use.", validationResult);
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

        private (ValidationResult Result, User? User) ReturnUserNotFound(int userId, ValidationResult validationResult)
        {
            _logger.LogErrorForValidation($"User with ID {userId} not found.", validationResult);
            validationResult.StatusCode = enStatusCode.NotFound;
            return (validationResult, null);
        }

        private (ValidationResult Result, User? User) ReturnBadRequest(ValidationResult validationResult)
        {
            _logger.LogErrorForValidation("Bad request.", validationResult);
            validationResult.StatusCode = enStatusCode.BadRequest;
            return (validationResult, null);
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

        public async Task<(ValidationResult Result, User? User)> AddUserAsAdmin(User user)
        {
            user.Role = "Admin";
            return await AddUserAsync(user);
        }

        public async Task<IReadOnlyCollection<User>> GetAllUsersAsync()
            => await _userRepository.GetAllAsync();


        public async Task<User?> GetUserByIdAsync(int id)
            => await _userRepository.GetByIdAsync(id);

        public async Task<(ValidationResult Result, User? User)> UpdateUserPasswordAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            var validationResult = new ValidationResult();

            if (existingUser is null) return ReturnUserNotFound(user.Id, validationResult);
            ValidatePassword(user.PasswordHash ?? string.Empty, validationResult);
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

            if (existingUser is null) return ReturnUserNotFound(user.Id, validationResult);
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

            if (existingUser is null) return ReturnUserNotFound(user.Id, validationResult);
            if (existingUser.Email != user.Email) await ValidateEmail(user.Email ?? string.Empty, validationResult);
            if (!validationResult.IsValid) ReturnBadRequest(validationResult);

            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.UpdatedAt = DateTime.UtcNow;
            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            return (validationResult, updatedUser);
        }
    }
}
