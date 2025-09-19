using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.User;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null) return NotFound($"User with ID = {id} not found!");
            UserDto userDto = _userMapper.ToUserDto(user);
            return Ok(userDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> AddAdminUserAsync(UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userMapper.ToUser(userCreateDto);
            var (result, createdUser) = await _userService.AddUserAsAdminAsync(user);
            if (!result.IsValid) return result.ToActionResult<UserDto>();
            var userDto = _userMapper.ToUserDto(createdUser!);

            return CreatedAtAction("GetUserById", new { id = userDto.Id }, userDto);
        }

        [HttpPut("{id}/password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUserPasswordAsync(int id, UserUpdatePasswordDto userUpdatePasswordDto)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userMapper.ToUser(userUpdatePasswordDto);
            user.Id = id;
            var (result, updatedUser) = await _userService.UpdateUserPasswordAsync(user, userUpdatePasswordDto.OldPassword);
            if (!result.IsValid) return result.ToActionResult<UserDto>();
            var userDto = _userMapper.ToUserDto(updatedUser!);
            return Ok(userDto);
        }

        [HttpPut("{id}/username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUserUsernameAsync(int id, UserUpdateUsernameDto userUpdateUsernameDto)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userMapper.ToUser(userUpdateUsernameDto);
            user.Id = id;
            var (result, updatedUser) = await _userService.UpdateUserUsernameAsync(user);
            if (!result.IsValid) return result.ToActionResult<UserDto>();
            var userDto = _userMapper.ToUserDto(updatedUser!);
            return Ok(userDto);
        }

        [HttpPut("{id}/email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUserEmailAsync(int id, UserUpdateEmailDto userUpdateEmailDto)
        {
            if (id < 1) return BadRequest("ID must be greater than 0.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userMapper.ToUser(userUpdateEmailDto);
            user.Id = id;
            var (result, updatedUser) = await _userService.UpdateUserEmailAsync(user);
            if (!result.IsValid) return result.ToActionResult<UserDto>();
            var userDto = _userMapper.ToUserDto(updatedUser!);
            return Ok(userDto);
        }


    }
}
