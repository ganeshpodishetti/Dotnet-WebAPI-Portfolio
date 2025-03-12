namespace Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(object id);
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
}