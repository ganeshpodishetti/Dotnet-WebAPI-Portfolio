namespace Application.DTOs.Skill;

public record SkillResponseDto(
    string Id,
    string SkillCategory,
    List<string> SkillsTypes,
    string? UpdatedAt);