using Domain.Enums;

namespace Domain.Errors;

public abstract class BaseError(string title, string description, StatusCode statusCode)
{
    public string Title { get; } = title;
    public string Description { get; } = description;
    public StatusCode StatusCode { get; } = statusCode;

    public static BaseError None()
    {
        return new GeneralError(string.Empty, string.Empty, StatusCode.Success);
    }

    public static BaseError Failure(string title, string description)
    {
        return new GeneralError(title, description, StatusCode.Failure);
    }

    public static BaseError NotFound(string title, string description)
    {
        return new GeneralError(title, description, StatusCode.NotFound);
    }

    public static BaseError Validation(string title, string description)
    {
        return new GeneralError(title, description, StatusCode.Validation);
    }

    public static BaseError Conflict(string title, string description)
    {
        return new GeneralError(title, description, StatusCode.Conflict);
    }

    public static BaseError UnAuthorized(string title, string description)
    {
        return new GeneralError(title, description, StatusCode.UnAuthorized);
    }

    public static BaseError Forbidden(string title, string description)
    {
        return new GeneralError(title, description, StatusCode.Forbidden);
    }
}