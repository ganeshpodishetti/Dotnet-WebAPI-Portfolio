namespace Application.DTOs.Skill;

public record SkillResponseDto(
    Guid? Id,
    string? Name,
    string? Proficiency,
    int? YearsOfExperience,
    string? UpdatedAt);