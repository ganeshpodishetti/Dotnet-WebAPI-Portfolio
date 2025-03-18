using Domain.Entities;

namespace Domain.Interfaces;

public interface ISkillRepository : IGenericRepository<Skill>
{
    Task<Skill?> GetByUserIdAsync(Guid userId);
    Task<List<Skill>?> GetAllByUserIdAsync(Guid userId);
}