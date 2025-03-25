using Domain.Enums;

namespace Domain.Errors;

public class Error
{
    private Error(
        string code,
        string description,
        StatusCode statusCode
    )
    {
        Code = code;
        Description = description;
        ErrorType = statusCode;
    }

    public string Code { get; }

    public string Description { get; }

    public StatusCode ErrorType { get; }

    public static Error None(string code, string description, StatusCode errorType)
    {
        return new Error(code, description, errorType);
    }

    public static Error Failure(string code, string description)
    {
        return new Error(code, description, StatusCode.Failure);
    }

    public static Error NotFound(string code, string description)
    {
        return new Error(code, description, StatusCode.NotFound);
    }

    public static Error Validation(string code, string description)
    {
        return new Error(code, description, StatusCode.Validation);
    }

    public static Error Conflict(string code, string description)
    {
        return new Error(code, description, StatusCode.Conflict);
    }

    public static Error AccessUnAuthorized(string code, string description)
    {
        return new Error(code, description, StatusCode.AccessUnAuthorized);
    }

    public static Error AccessForbidden(string code, string description)
    {
        return new Error(code, description, StatusCode.AccessForbidden);
    }
}