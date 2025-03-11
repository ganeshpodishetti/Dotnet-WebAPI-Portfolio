using Domain.Interfaces;

namespace Domain.UnitOfWork;

public interface IUnitOfWork
{
    IEducationRepository Education { get; }
    IExperienceRepository Experience { get; }
    IMessageRepository Message { get; }
    ISocialLinkRepository SocialLink { get; }
    IUserRepository User { get; }
    IProjectRepository Project { get; }
    ISkillRepository Skill { get; }

    Task CommitAsync();
    Task RollbackAsync();
}