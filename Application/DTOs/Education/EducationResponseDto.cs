namespace Application.DTOs.Education;

public record EducationResponseDto
{
    public Guid? Id { get; set; }
    public string? School { get; set; }
    public string? Degree { get; set; }
    public string? FieldOfStudy { get; set; }
    public string? Location { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Description { get; set; }
    public string? UpdatedAtUtc { get; set; }
}