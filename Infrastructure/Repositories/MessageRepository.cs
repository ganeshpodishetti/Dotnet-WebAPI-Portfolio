using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class MessageRepository(PortfolioDbContext context)
    : GenericRepository<Message>(context), IMessageRepository
{
    private readonly PortfolioDbContext _context = context;

    public async Task<Message?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Messages
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<IEnumerable<Message>?> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Messages
            .OrderBy(c => c.IsRead)
            .ThenByDescending(c => c.SentAt)
            .Where(e => e.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetNumberOfUnread(Guid userId)
    {
        return await _context.Messages
            .Where(c => !c.IsRead && c.UserId == userId)
            .CountAsync();
    }
}