using Domain.Common;

namespace Domain.Entities;

public class Skill : BaseEntity
{
    public required string Name { get; set; }
    public string Proficiency { get; set; } = null!;
    public int? YearsOfExperience { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
