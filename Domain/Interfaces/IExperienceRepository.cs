using Domain.Entities;

namespace Domain.Interfaces;

public interface IExperienceRepository
    : IGenericRepository<Experience>
{
    Task<Experience?> GetByUserIdAsync(Guid userId);
    Task<List<Experience>?> GetAllByUserIdAsync(Guid userId);
}