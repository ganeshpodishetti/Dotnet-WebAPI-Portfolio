namespace Domain.Errors;

public static class UserErrors
{
    public static Error UserNotFound(string email)
    {
        return Error.NotFound("user_not_found", $"User with email: {email} not found.");
    }

    public static Error UserAlreadyExists(string email)
    {
        return Error.Conflict("user_already_exists", $"User with email: {email} already exists.");
    }

    public static Error LoginFailed(string email)
    {
        return Error.Failure("login_failed", $"Invalid email: {email} or password.");
    }
}