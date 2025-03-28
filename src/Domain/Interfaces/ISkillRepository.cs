using Domain.Entities;

namespace Domain.Interfaces;

public interface ISkillRepository : IGenericRepository<Skill>
{
    Task<IEnumerable<Skill>?> GetAllByUserIdAsync(Guid userId);
}