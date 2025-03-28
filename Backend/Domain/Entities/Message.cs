using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Message : BaseEntity, IUserEntity
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public string? Subject { get; init; }
    public required string Content { get; init; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; init; }
    public Guid UserId { get; set; }
}