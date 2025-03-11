using Domain.Common;

namespace Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Proficiency { get; set; } = null!;
    public int YearsOfExperience { get; set; }
    public Guid UserId { get; set; }
}
