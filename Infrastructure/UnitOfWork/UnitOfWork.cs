using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

internal sealed class UnitOfWork(PortfolioDbContext context) : IUnitOfWork
{
    public IEducationRepository Education { get; private set; } = null!;
    public IExperienceRepository Experience { get; private set; } = null!;
    public IMessageRepository Message { get; private set; } = null!;
    public ISocialLinkRepository SocialLink { get; private set; } = null!;
    public ISkillRepository Skill { get; private set; } = null!;
    public IProjectRepository Project { get; private set; } = null!;
    public IUserRepository UserRepository { get; } = new UserRepository(context);

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        await context.DisposeAsync();
    }
}