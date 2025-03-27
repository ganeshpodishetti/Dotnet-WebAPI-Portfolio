namespace Application.DTOs.Authentication;

public record RefreshTokenResponseDto
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required string AccessTokenExpirationAtUtc { get; init; }
    public required string RefreshTokenExpirationAtUtc { get; init; }
}