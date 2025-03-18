namespace Application.DTOs.Education;

public record EducationRequestDto
{
    public required string School { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
}