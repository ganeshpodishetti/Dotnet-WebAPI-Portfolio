using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual Profile Profile { get; set; } = null!;
    public virtual ICollection<Education> Educations { get; set; } = [];
    public virtual ICollection<Experience> Experiences { get; set; } = [];
    public virtual ICollection<Project> Projects { get; set; } = [];
    public virtual ICollection<Skill> Skills { get; set; } = [];
    public virtual ICollection<SocialLink> SocialLinks { get; set; } = [];
}