using Domain.Common;

namespace Domain.Entities;

public class SocialLink : BaseEntity
{
    public required string Platform { get; set; }
    public required string Url { get; set; }
    public string? Icon { get; set; }
    public Guid UserId { get; set; }
}