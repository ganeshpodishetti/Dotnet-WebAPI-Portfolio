using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Skill : BaseEntity, IUserEntity
{
    public required string SkillType { get; set; }
    public required List<string> Skills { get; set; }
    public Guid UserId { get; set; }
}