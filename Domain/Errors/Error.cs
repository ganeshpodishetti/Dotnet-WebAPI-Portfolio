using Domain.Enums;

namespace Domain.Errors;

public class Error
{
    private Error(
        string title,
        string description,
        StatusCode statusCode
    )
    {
        Title = title;
        Description = description;
        StatusCode = statusCode;
    }

    public string Title { get; }

    public string Description { get; }

    public StatusCode StatusCode { get; }

    public static Error None()
    {
        return new Error(string.Empty, string.Empty, StatusCode.Success);
    }

    public static Error Failure(string title, string description)
    {
        return new Error(title, description, StatusCode.Failure);
    }

    public static Error NotFound(string title, string description)
    {
        return new Error(title, description, StatusCode.NotFound);
    }

    public static Error Validation(string title, string description)
    {
        return new Error(title, description, StatusCode.Validation);
    }

    public static Error Conflict(string title, string description)
    {
        return new Error(title, description, StatusCode.Conflict);
    }

    public static Error UnAuthorized(string title, string description)
    {
        return new Error(title, description, StatusCode.UnAuthorized);
    }

    public static Error Forbidden(string code, string description)
    {
        return new Error(code, description, StatusCode.Forbidden);
    }
}