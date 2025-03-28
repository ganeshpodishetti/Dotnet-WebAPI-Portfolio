namespace Application.DTOs.Project;

public record ProjectResponseDto(
    string Id,
    string Name,
    string Description,
    string? Url,
    string GithubUrl,
    string? UpdatedAt,
    List<string> Skills);