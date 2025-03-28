namespace Application.DTOs.Authentication;

public record RefreshTokenResponseDto(
    string AccessToken,
    string AccessTokenExpirationAtUtc,
    string RefreshToken,
    string RefreshTokenExpirationAtUtc)
{
    public RefreshTokenResponseDto()
        : this(string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }
}