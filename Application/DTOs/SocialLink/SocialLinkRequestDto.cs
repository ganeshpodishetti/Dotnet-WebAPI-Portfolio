namespace Application.DTOs.SocialLink;

public record SocialLinkRequestDto
{
    public required string Platform { get; set; }
    public required string Url { get; set; }
    public string? Icon { get; set; }
}