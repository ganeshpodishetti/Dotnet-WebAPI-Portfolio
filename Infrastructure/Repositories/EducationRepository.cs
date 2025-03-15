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
            .FirstAsync(e => e.UserId == userId);
    }
}