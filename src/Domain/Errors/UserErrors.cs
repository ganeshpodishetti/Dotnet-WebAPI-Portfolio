using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class UserErrors
{
    public static BaseError UserNotFound(string email)
    {
        return BaseError.NotFound("user_not_found", $"User with {email} not found.");
    }

    public static BaseError UserAlreadyExists(string email)
    {
        return BaseError.Conflict("user_already_exists", $"User with {email} already exists.");
    }

    public static BaseError LoginFailed(string email)
    {
        return BaseError.BadRequest("login_failed", $"Invalid email: {email} or password.");
    }

    public static BaseError FailedToUpdateUser(string details)
    {
        return BaseError.BadRequest("FailedToUpdateUser", $"Failed to update user: {details}");
    }

    public static BaseError FailedToRegisterUser(string details)
    {
        return BaseError.BadRequest("FailedToRegisterUser", $"Failed to register user: {details}");
    }

    public static BaseError FailedToDeleteUser(string details)
    {
        return BaseError.BadRequest("FailedToDeleteUser", $"Failed to delete user: {details}");
    }

    public static BaseError FailedToCreateRole(string details)
    {
        return BaseError.BadRequest("FailedToCreateRole", $"Failed to create role: {details}");
    }

    public static BaseError FailedToAssignRole(string details)
    {
        return BaseError.BadRequest("FailedToAssignRole", $"Failed to assign role: {details}");
    }

    public static BaseError FailedToChangePassword(string details)
    {
        return BaseError.BadRequest("FailedToChangePassword", $"Failed to change password: {details}");
    }

    public static BaseError InvalidUserName(string userName)
    {
        return BaseError.Validation("InvalidUserName",
            $"Username {userName} is invalid. Username should not contain special characters or spaces.");
    }

    public static BaseError AdminAlreadyExists(string email)
    {
        return BaseError.Conflict("admin_already_exists", "Application can only have one admin.");
    }
}