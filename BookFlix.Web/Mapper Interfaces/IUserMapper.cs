using BookFlix.Core.Models;
using BookFlix.Web.Dtos.User;

namespace BookFlix.Web.Mapper_Interfaces
{
    public interface IUserMapper
    {
        UserDto ToUserDto(User user);
        User ToUser(UserCreateDto userCreateDto);
        User ToUser(UserUpdatePasswordDto userUpdatePasswordDto);
        User ToUser(UserUpdateEmailDto userUpdateEmailDto);
        User ToUser(UserUpdateUsernameDto userUpdateUsernameDto);
    }
}
