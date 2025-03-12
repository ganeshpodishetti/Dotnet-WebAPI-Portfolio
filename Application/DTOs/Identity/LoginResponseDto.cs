namespace Application.DTOs.Identity;

public record LoginResponseDto
{
    public required string UserName { get; init; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? RefreshTokenExpiresAtUtc { get; init; }
}