using Domain.Interfaces;
using Domain.UnitOfWork;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal abstract class GenericRepository<T>(PortfolioDbContext context)
    : IGenericRepository<T> where T : class, IUserEntity
{
    // Add a new entity
    public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var entry = await context.Set<T>().AddAsync(entity, cancellationToken);
        return entry.State == EntityState.Added;
    }

    // Get all entities
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    // Update an existing entity
    public Task<bool> UpdateAsync(T entity)
    {
        var entry = context.Set<T>().Update(entity);
        return Task.FromResult(entry.State == EntityState.Modified);
    }

    // Delete an entity
    public Task<bool> DeleteAsync(T entity)
    {
        var entry = context.Set<T>().Remove(entity);
        return Task.FromResult(entry.State == EntityState.Deleted);
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await context.Set<T>()
            // FindAsync for primary key lookups
            .FindAsync(id)
            .AsTask();
    }

    public async Task<T?> GetByUserIdAsync(Guid userId, Guid id)
    {
        return await context.Set<T>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == id);
    }
}