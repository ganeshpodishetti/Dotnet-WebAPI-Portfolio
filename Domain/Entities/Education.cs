using Domain.Common;

namespace Domain.Entities;

public class Education : BaseEntity
{
    public required string School { get; set; }
    public string? Degree { get; set; }
    public string? Field { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
