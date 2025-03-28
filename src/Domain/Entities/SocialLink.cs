using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class SocialLink : BaseEntity, IUserEntity
{
    public required string Platform { get; init; }
    public required string Url { get; init; }
    public string? Icon { get; init; }
    public Guid UserId { get; set; }
}