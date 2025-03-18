namespace Application.DTOs.Experience;

public record ExperienceResponseDto
{
    public string? Title { get; set; }
    public string? CompanyName { get; set; }
    public string? Location { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public string? UpdatedAtUtc { get; set; }
}