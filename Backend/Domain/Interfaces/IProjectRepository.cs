using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<IEnumerable<Project>?> GetAllByUserIdAsync(Guid userId);
}