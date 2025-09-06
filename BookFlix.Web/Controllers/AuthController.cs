using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;

        public AuthController(IJwtService jwtService, IAuthService authService)
        {
            _jwtService = jwtService;
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (loginDto is null) return BadRequest("Login data cannot be null.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var userResult = await _authService.LoginAsync(loginDto.Email, loginDto.Password, ipAddress);

            if (!userResult.Result.IsValid)
            {
                return userResult.Result.ToActionResult();
            }

            if (userResult.User is null) return BadRequest("The user object is null.");

            var token = _jwtService.GenerateJwtToken(userResult.User);

            return Ok(new { Token = token });
        }

    }
}
