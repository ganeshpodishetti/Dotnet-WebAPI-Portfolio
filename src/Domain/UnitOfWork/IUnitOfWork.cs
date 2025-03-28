using Domain.Interfaces;

namespace Domain.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IEducationRepository EducationRepository { get; }
    IExperienceRepository ExperienceRepository { get; }
    IProjectRepository ProjectRepository { get; }
    ISkillRepository SkillRepository { get; }
    IMessageRepository MessageRepository { get; }
    ISocialLinkRepository SocialLinkRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}