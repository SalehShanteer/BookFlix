using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.Auth;
using BookFlix.Web.Dtos.User;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserMapper _userMapper;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IUserService userService, IUserMapper userMapper, IJwtService jwtService)
        {
            _authService = authService;
            _userService = userService;
            _userMapper = userMapper;
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignupAsync(UserCreateDto userCreateDto)
        {
            var user = _userMapper.ToUser(userCreateDto);
            var result = await _userService.AddUserAsync(user);

            if (result.IsFailure) return HandleFailure(result);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var authResult = await _authService.LoginAsync(userCreateDto.Email, userCreateDto.Password, ipAddress);

            if (authResult.IsFailure) return HandleFailure(authResult);
                
            return Ok(new TokensDto
            {
                AccessToken = authResult.Value.AccessToken,
                RefreshToken = authResult.Value.RefreshToken
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("signup/admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignupAsAdminAsync(UserCreateDto userCreateDto)
        {
            var user = _userMapper.ToUser(userCreateDto);
            var result = await _userService.AddUserAsAdminAsync(user);

            if (result.IsFailure) return HandleFailure(result);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var authResult = await _authService.LoginAsync(userCreateDto.Email, userCreateDto.Password, ipAddress);

            if (authResult.IsFailure) return HandleFailure(authResult);

            return Ok(new TokensDto
            {
                AccessToken = authResult.Value.AccessToken,
                RefreshToken = authResult.Value.RefreshToken
            });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password, ipAddress);

            if (result.IsFailure) return HandleFailure(result);

            return Ok(new TokensDto
            {
                AccessToken = result.Value.AccessToken,
                RefreshToken = result.Value.RefreshToken
            });
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenDto refreshToken)
        {
            var result = await _userService.UpdateUserRefreshToken(refreshToken.Token);

            if (result.IsFailure) return HandleFailure(result);

            return Ok(new TokensDto
            {
                AccessToken = result.Value.AccessToken,
                RefreshToken = result.Value.RefreshToken
            });
        }

        [HttpPost("is-authenticated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsAuthenticatedAsync([FromBody] RefreshTokenDto refreshToken)
        {
            var isAuthenticated = await _jwtService.IsValidRefreshToken(refreshToken.Token);

            return Ok(isAuthenticated);
        }
    }
}