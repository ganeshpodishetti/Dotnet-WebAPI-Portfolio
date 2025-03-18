using Domain.Entities;

namespace Domain.Interfaces;

public interface IMessageRepository
    : IGenericRepository<Message>
{
    //Task<Message?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Message>?> GetAllByUserIdAsync(Guid userId);
    Task<int> GetNumberOfUnread(Guid userId);
}