using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class SkillRepository(PortfolioDbContext context)
    : GenericRepository<Skill>(context), ISkillRepository
{
    private readonly PortfolioDbContext _context = context;

    public async Task<Skill?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Skills
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<List<Skill>?> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Skills
            .Where(e => e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}