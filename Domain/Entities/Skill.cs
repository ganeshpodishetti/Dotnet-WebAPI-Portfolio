using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Skill : BaseEntity, IUserEntity
{
    public required string SkillCategory { get; set; }
    public List<string> SkillsTypes { get; set; } = [];
    public Guid UserId { get; set; }
}