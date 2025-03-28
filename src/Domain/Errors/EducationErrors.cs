using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class EducationErrors
{
    public static BaseError EducationNotBelongToUser(string userId)
    {
        return BaseError.NotFound(
            "education_not_belong_to_user",
            $"Education record not found or does not belong to the user: {userId}");
    }

    public static BaseError FailedToAddEducation(string details)
    {
        return BaseError.BadRequest(
            "education_creation_failed",
            $"Education creation failed: {details}");
    }


    public static BaseError FailedToUpdateEducation(string details)
    {
        return BaseError.BadRequest(
            "education_update_failed",
            $"Education update failed: {details}");
    }


    public static BaseError FailedToDeleteEducation(string details)
    {
        return BaseError.BadRequest("education_delete_failed", $"Education delete failed: {details}");
    }

    public static BaseError EducationGetAllFailed(string details)
    {
        return BaseError.BadRequest("education_get_all_failed", $"Education get all failed: {details}");
    }
}