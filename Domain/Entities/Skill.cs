using Domain.Common;

namespace Domain.Entities;

public class Skill : BaseEntity
{
    public required string Name { get; set; }
    public string? Proficiency { get; set; }
    public int? YearsOfExperience { get; set; }
    public Guid UserId { get; set; }
}