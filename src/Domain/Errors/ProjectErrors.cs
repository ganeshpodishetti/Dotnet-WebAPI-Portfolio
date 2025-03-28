using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class ProjectErrors
{
    public static BaseError ProjectNotFound(string projectId)
    {
        return BaseError.NotFound(
            "Project.NotFound",
            $"Project with ID {projectId} was not found");
    }

    public static BaseError ProjectNotBelongToUser(string userId)
    {
        return BaseError.NotFound(
            "Project.NotBelongToUser",
            $"Project does not belong to user with ID {userId}");
    }

    public static BaseError FailedToAddProject(string userId)
    {
        return BaseError.BadRequest(
            "Project.AddFailed",
            $"Failed to add project for user with ID {userId}");
    }

    public static BaseError FailedToUpdateProject(string projectId)
    {
        return BaseError.BadRequest(
            "Project.UpdateFailed",
            $"Failed to update project with ID {projectId}");
    }

    public static BaseError FailedToDeleteProject(string projectId)
    {
        return BaseError.BadRequest(
            "Project.DeleteFailed",
            $"Failed to delete project with ID {projectId}");
    }
}