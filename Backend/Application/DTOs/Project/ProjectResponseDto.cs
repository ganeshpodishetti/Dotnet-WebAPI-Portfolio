namespace Application.DTOs.Project;

public record ProjectResponseDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? GithubUrl { get; set; }
    public string? UpdatedAt { get; set; }
    public List<string>? Skills { get; set; } = [];
}