using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class Repository<T>(PortfolioDbContext context)
    : IRepository<T> where T : class
{
    // Add a new entity
    public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.Set<T>().AddAsync(entity, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Update an existing entity
    public bool UpdateAsync(T entity)
    {
        try
        {
            context.Set<T>().Update(entity);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Get an entity by id
    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.Set<T>().FindAsync(id, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Get all entities
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.Set<T>().ToListAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // Delete an entity
    public bool DeleteAsync(T entity)
    {
        try
        {
            context.Set<T>().Remove(entity);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}