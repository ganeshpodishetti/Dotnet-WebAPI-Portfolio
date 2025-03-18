using Application.DTOs.Education;
using Application.DTOs.Experience;
using Application.DTOs.Message;
using Application.DTOs.Project;
using Application.DTOs.Skill;
using Application.DTOs.SocialLink;

namespace Application.DTOs.User;

public record UserResponseDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public string? Headline { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? UpdatedAt { get; set; }

    public IEnumerable<EducationResponseDto>? Educations { get; set; }
    public IEnumerable<ExperienceResponseDto>? Experiences { get; set; }
    public IEnumerable<SkillResponseDto>? Skills { get; set; }
    public IEnumerable<ProjectResponseDto>? Projects { get; set; }
    public IEnumerable<SocialLinkResponseDto>? SocialLinks { get; set; }
    public IEnumerable<MessageResponseDto>? Messages { get; set; }
}