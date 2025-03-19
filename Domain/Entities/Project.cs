using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Project : BaseEntity, IUserEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Url { get; set; }
    public string? GithubUrl { get; set; }

    public List<string>? Skills { get; set; } = [];

    public Guid UserId { get; set; }
}