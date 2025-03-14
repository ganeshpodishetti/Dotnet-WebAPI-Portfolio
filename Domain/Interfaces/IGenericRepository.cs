namespace Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<T?> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
}