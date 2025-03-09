using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAll();
    Task<Project?> GetById(Guid id);
    Task<Project> Add(Project project);
    Task Update(Project project);
    Task Delete(Project project);
}
