namespace Application.DTOs.Project;

public record ProjectRequestDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Url { get; set; }
    public string? GithubUrl { get; set; }
    public List<string>? Skills { get; set; } = [];
}