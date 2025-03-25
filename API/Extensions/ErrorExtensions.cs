using Domain.Enums;
using Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ErrorExtensions
{
    public static IActionResult ToActionResult(this Error error)
    {
        // Map different error types to appropriate status codes
        return error.StatusCode switch
        {
            StatusCode.Validation => new BadRequestObjectResult(error),
            StatusCode.NotFound => new NotFoundObjectResult(error),
            StatusCode.UnAuthorized => new UnauthorizedObjectResult(error),
            StatusCode.Forbidden => new ForbidResult(),
            StatusCode.Conflict => new ConflictObjectResult(error),
            StatusCode.Failure => new BadRequestObjectResult(error),
            StatusCode.ServerError => new ObjectResult(error) { StatusCode = 500 },
            _ => new ObjectResult(error) { StatusCode = 500 } // Default to 500
        };
    }
}