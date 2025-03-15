using Domain.Interfaces;

namespace Domain.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IEducationRepository EducationRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}