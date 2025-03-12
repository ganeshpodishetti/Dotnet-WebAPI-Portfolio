using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Context;

namespace Infrastructure.UnitOfWork;

internal sealed class UnitOfWork(PortfolioDbContext context) : IUnitOfWork
{
    // public IEducationRepository Education { get; } = null!;
    // public IExperienceRepository Experience { get; } = null!;
    // public IMessageRepository Message { get; } = null!;
    // public ISocialLinkRepository SocialLink { get; } = null!;
    // public ISkillRepository Skill { get; } = null!;
    // public IProjectRepository Project { get; } = null!;

    public IUserRepository UserRepository { get; } = null!;

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        await context.DisposeAsync();
    }
}