namespace Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
    bool UpdateAsync(T entity);
    bool DeleteAsync(T entity);
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
}