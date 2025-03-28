using System.Net;
using Domain.Common.BaseErrors;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Handlers;

public static class GlobalErrorHandler
{
    public static IActionResult ToActionResult(this BaseError error)
    {
        var problemDetails = new ProblemDetails
        {
            Title = error.Title,
            Status = (int)error.StatusCode,
            Detail = error.Description
        };

        // Map different error types to appropriate status codes
        return error.StatusCode switch
        {
            StatusCode.NotFound => new NotFoundObjectResult(problemDetails)
                { StatusCode = (int)HttpStatusCode.NotFound },
            StatusCode.Conflict => new ConflictObjectResult(problemDetails)
                { StatusCode = (int)HttpStatusCode.Conflict },
            StatusCode.UnAuthorized => new ObjectResult(problemDetails)
                { StatusCode = (int)HttpStatusCode.Unauthorized },
            StatusCode.BadRequest => new BadRequestObjectResult(problemDetails)
                { StatusCode = (int)HttpStatusCode.BadRequest },
            StatusCode.Validation => new UnprocessableEntityObjectResult(problemDetails)
                { StatusCode = (int)HttpStatusCode.UnprocessableEntity },
            StatusCode.InternalServerError => new ObjectResult(problemDetails)
                { StatusCode = (int)HttpStatusCode.InternalServerError },
            _ => new BadRequestObjectResult(problemDetails) { StatusCode = (int)HttpStatusCode.BadRequest }
        };
    }
}