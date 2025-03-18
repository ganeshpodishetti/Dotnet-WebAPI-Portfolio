namespace Application.DTOs.Skill;

public record SkillRequestDto
{
    public required string Name { get; set; }
    public string? Proficiency { get; set; }
    public int? YearsOfExperience { get; set; }
}