using Domain.Interfaces;

namespace Domain.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    Task CommitAsync();
    Task RollbackAsync();
}