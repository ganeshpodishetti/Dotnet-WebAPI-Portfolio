using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Context;
using Infrastructure.Repositories;

namespace Infrastructure.UnitOfWork;

internal sealed class UnitOfWork(PortfolioDbContext context) : IUnitOfWork
{
    public IUserRepository UserRepository { get; } = new UserRepository(context);

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RollbackAsync()
    {
        await context.DisposeAsync();
    }
}