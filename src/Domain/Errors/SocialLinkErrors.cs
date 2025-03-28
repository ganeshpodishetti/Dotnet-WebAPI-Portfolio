using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class SocialLinkErrors
{
    public static BaseError SocialLinkNotFound(string linkId)
    {
        return BaseError.NotFound(
            "SocialLink.NotFound",
            $"Social link with ID {linkId} was not found");
    }

    public static BaseError SocialLinkNotBelongToUser(string userId)
    {
        return BaseError.NotFound(
            "SocialLink.NotBelongToUser",
            $"Social link does not belong to user with ID {userId}");
    }

    public static BaseError FailedToAddSocialLink(string userId)
    {
        return BaseError.BadRequest(
            "SocialLink.AddFailed",
            $"Failed to add social link for user with ID {userId}");
    }

    public static BaseError FailedToUpdateSocialLink(string linkId)
    {
        return BaseError.BadRequest(
            "SocialLink.UpdateFailed",
            $"Failed to update social link with ID {linkId}");
    }

    public static BaseError FailedToDeleteSocialLink(string linkId)
    {
        return BaseError.BadRequest(
            "SocialLink.DeleteFailed",
            $"Failed to delete social link with ID {linkId}");
    }
}