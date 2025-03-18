using Domain.Common;

namespace Domain.Entities;

public class Education : BaseEntity
{
    public required string School { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
}