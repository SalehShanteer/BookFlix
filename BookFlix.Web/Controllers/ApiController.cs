using BookFlix.Core.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Web.Controllers
{
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public abstract class ApiController : ControllerBase
    {
        protected IActionResult HandleFailure(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot handle a successful result as a failure.");
            }
            return result.Error.ErrorType switch
            {
                ErrorType.NotFound => NotFound(result.Error.Key),
                ErrorType.Validation => BadRequest(result.Error.Key),
                ErrorType.Conflict => Conflict(result.Error.Key),
                ErrorType.Unauthorized => Unauthorized(result.Error.Key),
                ErrorType.Forbidden => Forbid(result.Error.Key),
                ErrorType.Failure => StatusCode(500, result.Error),
                _ => StatusCode(500, "An unexpected error occurred.")
            };
        }
    }
}
