namespace Domain.Enums;

public enum StatusCode
{
    Failure = 400,
    NotFound = 404,
    Validation = 422,
    Conflict = 409,
    AccessUnAuthorized = 401,
    AccessForbidden = 403,
    ServerError = 500,
    ServiceUnavailable = 503,
    Success = 200
}