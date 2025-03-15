using Domain.Common;

namespace Domain.Entities;

public class Education : BaseEntity
{
    public string School { get; set; } = null!;
    public string Degree { get; set; } = null!;
    public string? FieldOfStudy { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
}