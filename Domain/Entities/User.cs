using Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class User : IdentityUser<Guid>, IUserEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public AboutMe AboutMe { get; set; } = null!;
    public ICollection<Education> Educations { get; set; } = [];
    public ICollection<Experience> Experiences { get; set; } = [];
    public ICollection<Project> Projects { get; set; } = [];
    public ICollection<Skill> Skills { get; set; } = [];
    public ICollection<SocialLink> SocialLinks { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];

    // Implement IUserEntity interface
    Guid IUserEntity.UserId
    {
        get => Id;
        set => Id = value;
    }
}