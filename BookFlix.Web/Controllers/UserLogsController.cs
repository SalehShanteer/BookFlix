using BookFlix.Core.Extensions;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.UserLog;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Authorize]
    [Route("api/userlogs")]
    [ApiController]
    public class UserLogsController : ControllerBase
    {
        private readonly IUserLogService _userLogService;
        private readonly IUserLogMapper _userLogMapper;

        public UserLogsController(IUserLogService userLogService, IUserLogMapper userLogMapper)
        {
            _userLogService = userLogService;
            _userLogMapper = userLogMapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserLogs(Guid userId)
        {
            var userLogs = await _userLogService.GetLogsByUserIdAsync(userId);

            if (userLogs.IsEmpty()) return Ok(new List<UserLogDto>());
            var userLogDtos = _userLogMapper.ToUserLogDtos(userLogs);

            return Ok(userLogDtos);
        }
    }
}
