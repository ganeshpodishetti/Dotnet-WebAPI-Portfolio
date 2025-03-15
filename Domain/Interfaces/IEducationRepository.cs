using Domain.Entities;

namespace Domain.Interfaces;

public interface IEducationRepository
    : IGenericRepository<Education>
{
    Task<Education?> GetByUserIdAsync(Guid userId);
}