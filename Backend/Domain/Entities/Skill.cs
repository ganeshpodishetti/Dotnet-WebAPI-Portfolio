using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Skill : BaseEntity, IUserEntity
{
    public required string SkillCategory { get; init; }
    public List<string> SkillsTypes { get; init; } = [];
    public Guid UserId { get; set; }
}