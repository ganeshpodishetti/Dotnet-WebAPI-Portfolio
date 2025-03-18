using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class SocialLinkRepository(PortfolioDbContext context)
    : GenericRepository<SocialLink>(context), ISocialLinkRepository
{
    private readonly PortfolioDbContext _context = context;

    public async Task<SocialLink?> GetByUserIdAsync(Guid userId)
    {
        return await _context.SocialLinks
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<IEnumerable<SocialLink>?> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.SocialLinks
            .Where(e => e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }
}