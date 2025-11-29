using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.Auth;
using BookFlix.Web.Dtos.User;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserMapper _userMapper;

        public AuthController(IAuthService authService, IUserService userService, IUserMapper userMapper)
        {
            _authService = authService;
            _userService = userService;
            _userMapper = userMapper;
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignupAsync(UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userMapper.ToUser(userCreateDto);
            var (result, createdUser) = await _userService.AddUserAsync(user);

            if (!result.IsValid || createdUser is null) return result.ToActionResult();

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var authResult = await _authService.LoginAsync(userCreateDto.Email, userCreateDto.Password, ipAddress);
            return Ok(new TokensDto { AccessToken = authResult.AccessToken, RefreshToken = authResult.RefreshToken });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("signup/admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignupAsAdminAsync(UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userMapper.ToUser(userCreateDto);
            var (result, createdUser) = await _userService.AddUserAsAdminAsync(user);

            if (!result.IsValid || createdUser is null) return result.ToActionResult();

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var authResult = await _authService.LoginAsync(userCreateDto.Email, userCreateDto.Password, ipAddress);

            return Ok(new TokensDto { AccessToken = authResult.AccessToken, RefreshToken = authResult.RefreshToken });
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (loginDto is null) return BadRequest("NullLoginRequest");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var (result, token, refreshToken) = await _authService.LoginAsync(loginDto.Email, loginDto.Password, ipAddress);

            if (!result.IsValid) return result.ToActionResult();

            return Ok(new TokensDto { AccessToken = token, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] string refreshToken)
        {
            var (result, newAccessToken, newRefreshToken) = await _userService.UpdateUserRefreshToken(refreshToken);

            if (!result.IsValid) return result.ToActionResult();

            return Ok(new TokensDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


    }
}
