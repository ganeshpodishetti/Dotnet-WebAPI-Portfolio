using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Skill : BaseEntity, IUserEntity
{
    public required string Name { get; set; }
    public string? Proficiency { get; set; }
    public int? YearsOfExperience { get; set; }
    public Guid UserId { get; set; }
}