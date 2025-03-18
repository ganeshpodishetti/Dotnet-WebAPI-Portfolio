using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class EducationRepository(PortfolioDbContext context)
    : GenericRepository<Education>(context), IEducationRepository
{
    private readonly PortfolioDbContext _context = context;

    public async Task<Education?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Educations
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<List<Education>?> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Educations
            .Where(e => e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}