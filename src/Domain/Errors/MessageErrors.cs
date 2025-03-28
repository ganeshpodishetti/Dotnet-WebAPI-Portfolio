using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class MessageErrors
{
    public static BaseError MessageNotFound(string messageId)
    {
        return BaseError.NotFound(
            "Message.NotFound",
            $"Message with ID {messageId} was not found");
    }

    public static BaseError MessageNotBelongToUser(string userId)
    {
        return BaseError.NotFound(
            "Message.NotBelongToUser",
            $"Message does not belong to user with ID {userId}");
    }

    public static BaseError FailedToAddMessage(string userId)
    {
        return BaseError.BadRequest(
            "Message.AddFailed",
            $"Failed to add message for user with ID {userId}");
    }

    public static BaseError FailedToUpdateMessage(string messageId)
    {
        return BaseError.BadRequest(
            "Message.UpdateFailed",
            $"Failed to update message with ID {messageId}");
    }

    public static BaseError FailedToDeleteMessage(string messageId)
    {
        return BaseError.BadRequest(
            "Message.DeleteFailed",
            $"Failed to delete message with ID {messageId}");
    }
}