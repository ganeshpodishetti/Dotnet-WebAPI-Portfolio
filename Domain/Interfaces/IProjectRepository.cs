using Domain.Entities;

namespace Domain.Interfaces;

public interface IProjectRepository : IGenericRepository<Project>
{
    //Task<Project?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Project>?> GetAllByUserIdAsync(Guid userId);
}