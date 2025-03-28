using Domain.Common.BaseErrors;

namespace Domain.Errors;

public static class SkillErrors
{
    public static BaseError SkillNotFound(string skillId)
    {
        return BaseError.NotFound(
            "Skill.NotFound",
            $"Skill with ID {skillId} was not found");
    }

    public static BaseError SkillNotBelongToUser(string userId)
    {
        return BaseError.NotFound(
            "Skill.NotBelongToUser",
            $"Skill does not belong to user with ID {userId}");
    }

    public static BaseError FailedToAddSkill(string userId)
    {
        return BaseError.BadRequest(
            "Skill.AddFailed",
            $"Failed to add skill for user with ID {userId}");
    }

    public static BaseError FailedToUpdateSkill(string skillId)
    {
        return BaseError.BadRequest(
            "Skill.UpdateFailed",
            $"Failed to update skill with ID {skillId}");
    }

    public static BaseError FailedToDeleteSkill(string skillId)
    {
        return BaseError.BadRequest(
            "Skill.DeleteFailed",
            $"Failed to delete skill with ID {skillId}");
    }
}