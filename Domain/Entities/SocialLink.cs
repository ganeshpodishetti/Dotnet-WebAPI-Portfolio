using Domain.Common;

namespace Domain.Entities;

public class SocialLink : BaseEntity
{
    public string Platform { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public Guid UserId { get; set; }
}
