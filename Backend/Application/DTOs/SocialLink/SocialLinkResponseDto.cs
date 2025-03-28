namespace Application.DTOs.SocialLink;

public record SocialLinkResponseDto(
    string Id,
    string Platform,
    string Url,
    string? Icon,
    string? UpdateAt);