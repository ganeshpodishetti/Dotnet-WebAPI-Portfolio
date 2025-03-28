using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class UserRepository(PortfolioDbContext context)
    : GenericRepository<User>(context), IUserRepository
{
    private readonly PortfolioDbContext _context = context;

    public async Task<IEnumerable<User?>> GetUserDetailsAsync()
    {
        return await _context.Users
            .Include(u => u.Educations)
            .Include(u => u.Experiences)
            .Include(u => u.Projects)
            .Include(p => p.Skills)
            .Include(p => p.SocialLinks)
            .Include(p => p.Messages)
            .AsNoTracking()
            .ToListAsync();
    }
}