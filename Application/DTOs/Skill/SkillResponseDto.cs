namespace Application.DTOs.Skill;

public record SkillResponseDto(
    Guid? Id,
    string? Name,
    string? Logo,
    string? UpdatedAt);