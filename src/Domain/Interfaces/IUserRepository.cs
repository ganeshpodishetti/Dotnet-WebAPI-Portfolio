using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IEnumerable<User?>> GetUserDetailsAsync();
}