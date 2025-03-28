namespace Application.DTOs.Authentication;

public record LoginResponseDto(
    string UserId,
    string AccessToken,
    string AccessTokenExpirationAtUtc,
    string RefreshToken,
    string RefreshTokenExpirationAtUtc);