using Domain.Entities;

namespace Domain.Interfaces;

public interface ISkillRepository
{
    Task<IEnumerable<Skill>> GetAll();
    Task<Skill?> GetById(Guid id);
    Task<Skill> Add(Skill skill);
    Task Update(Skill skill);
    Task Delete(Skill skill);
}
