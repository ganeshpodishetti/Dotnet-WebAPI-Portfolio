using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class ExperienceErrors
{
    public static BaseError ExperienceNotBelongToUser(string userId)
    {
        return BaseError.NotFound(
            "Experience.NotBelongToUser",
            $"Experience does not belong to user with ID {userId}");
    }

    public static BaseError FailedToAddExperience(string userId)
    {
        return BaseError.BadRequest(
            "Experience.AddFailed",
            $"Failed to add experience for user with ID {userId}");
    }


    public static BaseError FailedToUpdateExperience(string userId)
    {
        return BaseError.BadRequest(
            "Experience.UpdateFailed",
            $"Failed to update experience for user with ID {userId}");
    }

    public static BaseError FailedToDeleteExperience(string userId)
    {
        return BaseError.BadRequest(
            "Experience.DeleteFailed",
            $"Failed to delete experience for user with ID {userId}");
    }

    public static BaseError ExperienceNotFound(string experienceId)
    {
        return BaseError.NotFound(
            "Experience.NotFound",
            $"Experience with ID {experienceId} was not found");
    }
}