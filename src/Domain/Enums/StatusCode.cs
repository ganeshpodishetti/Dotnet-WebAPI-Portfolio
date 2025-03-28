namespace Domain.Enums;

public enum StatusCode
{
    BadRequest = 400,
    NotFound = 404,
    Validation = 422,
    Conflict = 409,
    UnAuthorized = 401,
    Forbidden = 403,
    ServiceUnavailable = 503,
    Success = 200,
    BadGateway = 502,
    InternalServerError = 500
}