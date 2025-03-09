using Domain.Entities;

namespace Domain.Interfaces;

public interface IExperienceRepository
{
    Task<IEnumerable<Experience>> GetAll();
    Task<Experience?> GetById(Guid id);
    Task<Experience> Add(Experience experience);
    Task Update(Experience experience);
    Task Delete(Experience experience);
}
