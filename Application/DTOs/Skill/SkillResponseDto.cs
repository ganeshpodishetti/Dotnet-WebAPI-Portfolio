namespace Application.DTOs.Skill;

public record SkillResponseDto(
    string Name,
    string? Proficiency,
    int? YearsOfExperience,
    string? UpdatedAt);