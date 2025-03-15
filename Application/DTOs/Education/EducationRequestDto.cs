namespace Application.DTOs.Education;

public record EducationRequestDto
{
    public string School { get; set; } = null!;
    public string Degree { get; set; } = null!;
    public string FieldOfStudy { get; set; } = null!;
    public string StartDate { get; set; } = null!;
    public string? EndDate { get; set; }
    public string Description { get; set; } = null!;
}