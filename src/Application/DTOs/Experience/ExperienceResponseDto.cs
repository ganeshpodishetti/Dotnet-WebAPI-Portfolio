namespace Application.DTOs.Experience;

public record ExperienceResponseDto(
    string Id,
    string Title,
    string CompanyName,
    string? Location,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description,
    string? UpdatedAtUtc
);