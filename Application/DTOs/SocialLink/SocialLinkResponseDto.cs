namespace Application.DTOs.SocialLink;

public record SocialLinkResponseDto
{
    public string? Platform { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public string? UpdateAt { get; set; }
}