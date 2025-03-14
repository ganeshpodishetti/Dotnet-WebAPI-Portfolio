using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal abstract class GenericRepository<T>(PortfolioDbContext context)
    : IGenericRepository<T> where T : class
{
    // Add a new entity
    public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await context.Set<T>().AddAsync(entity, cancellationToken);
        return true;
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        var entity = await context.Set<T>().FindAsync(id);
        return entity;
    }

    // Get all entities
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<T>().ToListAsync(cancellationToken);
    }

    // Update an existing entity
    public Task<bool> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        return Task.FromResult(true);
    }

    // Delete an entity
    public Task<bool> DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return Task.FromResult(true);
    }
}