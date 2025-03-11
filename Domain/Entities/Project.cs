using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string GithubUrl { get; set; } = null!;

    public Guid UserId { get; set; }

    public virtual ICollection<Skill> Skills { get; set; } = [];
}