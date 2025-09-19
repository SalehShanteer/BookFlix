using BookFlix.Core.Models;
using BookFlix.Web.Dtos.User;
using BookFlix.Web.Mapper_Interfaces;

namespace BookFlix.Web.Mappers
{
    public class UserMapper : IUserMapper
    {
        public User ToUser(UserCreateDto userCreateDto)
        {
            return new User
            {
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                PasswordHash = userCreateDto.Password
            };
        }

        public User ToUser(UserUpdatePasswordDto userUpdatePasswordDto)
        {
            return new User
            {
                PasswordHash = userUpdatePasswordDto.NewPassword
            };

        }

        public User ToUser(UserUpdateEmailDto userUpdateEmailDto)
        {
            return new User
            {
                Email = userUpdateEmailDto.NewEmail
            };
        }

        public User ToUser(UserUpdateUsernameDto userUpdateUsernameDto)
        {
            return new User
            {
                Username = userUpdateUsernameDto.NewUsername
            };
        }

        public UserDto ToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Role = user.Role
            };

        }


    }
}
