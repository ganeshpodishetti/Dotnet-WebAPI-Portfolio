using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Project : BaseEntity, IUserEntity
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string? Url { get; set; }
    public required string GithubUrl { get; init; }

    public List<string> Skills { get; init; } = [];

    public Guid UserId { get; set; }
}