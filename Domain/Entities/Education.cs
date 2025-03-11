using Domain.Common;

namespace Domain.Entities;

public class Education : BaseEntity
{
    public string School { get; set; } = null!;
    public string Degree { get; set; } = null!;
    public string FieldOfStudy { get; set; } = null!;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = null!;
    public Guid UserId { get; set; }
}
