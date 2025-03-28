using Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class User : IdentityUser<Guid>, IUserEntity
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public AboutMe AboutMe { get; init; } = new();
    public ICollection<Education> Educations { get; init; } = [];
    public ICollection<Experience> Experiences { get; init; } = [];
    public ICollection<Project> Projects { get; init; } = [];
    public ICollection<Skill> Skills { get; init; } = [];
    public ICollection<SocialLink> SocialLinks { get; init; } = [];
    public ICollection<Message> Messages { get; init; } = [];

    // Implement IUserEntity interface
    Guid IUserEntity.UserId
    {
        get => Id;
        set => Id = value;
    }
}