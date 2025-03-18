using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class ExperienceRepository(PortfolioDbContext context)
    : GenericRepository<Experience>(context), IExperienceRepository
{
    private readonly PortfolioDbContext _context = context;

    public async Task<List<Experience>?> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Experiences
            .Where(e => e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}