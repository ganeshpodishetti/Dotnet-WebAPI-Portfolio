using Application.DTOs.Education;
using Application.DTOs.Experience;
using Application.DTOs.Message;
using Application.DTOs.Project;
using Application.DTOs.Skill;
using Application.DTOs.SocialLink;

namespace Application.DTOs.User;

public record UserResponseDto
{
    public required string Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? ProfilePicture { get; init; }
    public string? Bio { get; init; }
    public string? Headline { get; init; }
    public string? Country { get; init; }
    public string? City { get; init; }
    public string? UpdatedAt { get; init; }

    public IEnumerable<EducationResponseDto>? Educations { get; init; }
    public IEnumerable<ExperienceResponseDto>? Experiences { get; init; }
    public IEnumerable<SkillResponseDto>? Skills { get; init; }
    public IEnumerable<ProjectResponseDto>? Projects { get; init; }
    public IEnumerable<SocialLinkResponseDto>? SocialLinks { get; init; }
    public IEnumerable<MessageResponseDto>? Messages { get; init; }
}