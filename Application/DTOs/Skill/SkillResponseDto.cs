namespace Application.DTOs.Skill;

public record SkillResponseDto
{
    public string? SkillCategory { get; set; }
    public List<string>? SkillsTypes { get; set; }
    public string? UpdatedAt { get; set; }
}