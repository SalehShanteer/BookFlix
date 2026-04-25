using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.User;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Authorize]
    [Route("api/Users")]
    public class UsersController : ApiController
    {
        public readonly IUserMapper _userMapper;
        public readonly IUserService _userService;

        public UsersController(IUserMapper userMapper, IUserService userService)
        {
            _userMapper = userMapper;
            _userService = userService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByIDAsync(Guid id)
        {
            var result = await _userService.GetUserByIDAsync(id);
            if (result.IsFailure) return HandleFailure(result);
            UserDto userDto = _userMapper.ToUserDto(result.Value);
            return Ok(userDto);
        }

        [HttpPut("{id}/password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserPasswordAsync(Guid id, UserUpdatePasswordDto userUpdatePasswordDto)
        {
            var result = await _userService.UpdateUserPasswordAsync(id, userUpdatePasswordDto.OldPassword, userUpdatePasswordDto.NewPassword);
            if (result.IsFailure) return HandleFailure(result);
            return NoContent();
        }

        [HttpPut("{id}/username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserUsernameAsync(Guid id, UserUpdateUsernameDto userUpdateUsernameDto)
        {
            var result = await _userService.UpdateUserUsernameAsync(id, userUpdateUsernameDto.Username);
            if (result.IsFailure) return HandleFailure(result);
            var userDto = _userMapper.ToUserDto(result.Value);
            return Ok(userDto);
        }

        [HttpPut("{id}/email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserEmailAsync(Guid id, UserUpdateEmailDto userUpdateEmailDto)
        {
            var result = await _userService.UpdateUserEmailAsync(id, userUpdateEmailDto.Email);
            if (result.IsFailure) return HandleFailure(result);
            var userDto = _userMapper.ToUserDto(result.Value);
            return Ok(userDto);
        }
    }
}
