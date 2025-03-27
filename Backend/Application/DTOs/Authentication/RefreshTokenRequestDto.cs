namespace Application.DTOs.Authentication;

public record RefreshTokenRequestDto
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}