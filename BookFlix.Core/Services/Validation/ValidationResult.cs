using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Core.Services.Validation
{

    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public enStatusCode StatusCode { get; set; } = enStatusCode.Ok;
        public List<string> Errors { get; set; } = new List<string>();

        public IActionResult ToActionResult()
        {
            return StatusCode switch
            {
                enStatusCode.BadRequest => new BadRequestObjectResult(Errors),
                enStatusCode.NotFound => new NotFoundObjectResult(Errors),
                enStatusCode.Unauthorized => new ObjectResult(Errors) { StatusCode = StatusCodes.Status401Unauthorized },
                _ => new ObjectResult(Errors) { StatusCode = StatusCodes.Status500InternalServerError }
            };
        }
        public ActionResult<T> ToActionResult<T>()
        {
            return StatusCode switch
            {
                enStatusCode.BadRequest => new BadRequestObjectResult(Errors),
                enStatusCode.NotFound => new NotFoundObjectResult(Errors),
                enStatusCode.Unauthorized => new ObjectResult(Errors) { StatusCode = StatusCodes.Status401Unauthorized },
                _ => new ObjectResult(Errors) { StatusCode = StatusCodes.Status500InternalServerError }
            };
        }

    }
}


