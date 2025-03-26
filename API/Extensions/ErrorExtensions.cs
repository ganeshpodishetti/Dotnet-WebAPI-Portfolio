using System.Net;
using Domain.Enums;
using Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ErrorExtensions
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
            StatusCode.Failure => new BadRequestObjectResult(problemDetails)
                { StatusCode = (int)HttpStatusCode.BadRequest },
            _ => new BadRequestObjectResult(problemDetails) { StatusCode = (int)HttpStatusCode.BadRequest }
        };
    }
}