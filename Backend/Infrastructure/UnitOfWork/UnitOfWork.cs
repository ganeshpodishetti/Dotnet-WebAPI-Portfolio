using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

internal sealed class UnitOfWork(PortfolioDbContext context) : IUnitOfWork
{
    public IUserRepository UserRepository { get; } = new UserRepository(context);
    public IEducationRepository EducationRepository { get; } = new EducationRepository(context);
    public IExperienceRepository ExperienceRepository { get; } = new ExperienceRepository(context);
    public ISkillRepository SkillRepository { get; } = new SkillRepository(context);
    public IProjectRepository ProjectRepository { get; } = new ProjectRepository(context);
    public IMessageRepository MessageRepository { get; } = new MessageRepository(context);
    public ISocialLinkRepository SocialLinkRepository { get; } = new SocialLinkRepository(context);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RollbackAsync()
    {
        await context.DisposeAsync();
    }
}