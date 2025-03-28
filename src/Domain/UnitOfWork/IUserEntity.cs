namespace Domain.UnitOfWork;

public interface IUserEntity
{
    Guid Id { get; set; }
    Guid UserId { get; set; }
}