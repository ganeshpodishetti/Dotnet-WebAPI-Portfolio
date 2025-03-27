namespace Application.DTOs.Authentication;

public record LoginResponseDto
{
    public required string UserId { get; init; }
    public string? AccessToken { get; set; }
    public string? AccessTokenExpirationAtUtc { get; set; }
    public string? RefreshToken { get; set; }
    public string? RefreshTokenExpirationAtUtc { get; set; }
}