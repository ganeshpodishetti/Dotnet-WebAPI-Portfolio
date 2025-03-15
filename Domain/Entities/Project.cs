using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Url { get; set; }
    public string? GithubUrl { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<Skill> Skills { get; set; } = [];
}