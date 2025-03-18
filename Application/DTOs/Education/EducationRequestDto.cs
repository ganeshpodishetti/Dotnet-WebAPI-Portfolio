namespace Application.DTOs.Education;

public record EducationRequestDto
{
    public required string School { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; } = null!;
    public required string StartDate { get; set; }
    public string? EndDate { get; set; }
    public string Description { get; set; } = null!;
}