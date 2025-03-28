namespace Application.DTOs.Education;

public record EducationResponseDto(
    string Id,
    string School,
    string Degree,
    string? FieldOfStudy,
    string Location,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description,
    string? UpdatedAtUtc);