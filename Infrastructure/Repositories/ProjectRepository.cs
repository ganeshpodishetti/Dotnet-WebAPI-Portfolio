using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class ProjectRepository(PortfolioDbContext context)
    : GenericRepository<Project>(context), IProjectRepository
{
    private readonly PortfolioDbContext _context = context;

    public async Task<Project?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<IEnumerable<Project>?> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Projects
            .Where(e => e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}