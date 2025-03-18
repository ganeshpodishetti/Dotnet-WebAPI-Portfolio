using Domain.Common;

namespace Domain.Entities;

public class Message : BaseEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public Guid UserId { get; set; }
}