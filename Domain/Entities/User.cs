using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Profile Profile { get; set; } = null!;
    public ICollection<Education> Educations { get; set; } = [];
    public ICollection<Experience> Experiences { get; set; } = [];
    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<Skill> Skills { get; set; } = [];
    public ICollection<SocialLink> SocialLinks { get; set; } = [];
}