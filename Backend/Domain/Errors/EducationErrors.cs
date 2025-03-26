namespace Domain.Errors;

public static class EducationErrors
{
    public static BaseError UserNotFoundToAddEducation(string id)
    {
        return BaseError.NotFound("user_not_found",
            $"User with id: {id} does not exist to add education.");
    }

    public static BaseError EducationNotBelongToUser(string userId)
    {
        return BaseError.NotFound("education_not_belong_to_user",
            $"Education record not found or does not belong to the user: {userId}");
    }

    public static BaseError EducationCreationFailed(string details)
    {
        return BaseError.Failure("education_creation_failed", $"Education creation failed: {details}");
    }

    public static BaseError EducationUpdateFailed(string details)
    {
        return BaseError.Failure("education_update_failed", $"Education update failed: {details}");
    }

    public static BaseError EducationDeleteFailed(string details)
    {
        return BaseError.Failure("education_delete_failed", $"Education delete failed: {details}");
    }

    public static BaseError EducationGetAllFailed(string details)
    {
        return BaseError.Failure("education_get_all_failed", $"Education get all failed: {details}");
    }
}