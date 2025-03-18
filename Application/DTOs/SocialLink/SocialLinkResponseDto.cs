namespace Application.DTOs.SocialLink;

public record SocialLinkResponseDto
{
    public Guid? Id { get; set; }
    public string? Platform { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public string? UpdateAt { get; set; }
}