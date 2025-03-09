using Domain.Entities;

namespace Domain.Interfaces;

public interface IEducationRepository
{
    Task<IEnumerable<Education>> GetAll();
    Task<Education?> GetById(Guid id);
    Task<Education> Add(Education education);
    Task Update(Education education);
    Task Delete(Education education);
}
