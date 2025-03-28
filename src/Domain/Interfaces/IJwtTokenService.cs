using Domain.Entities;

namespace Domain.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateJwtToken(User user);
    string GenerateRefreshToken();
    Guid GetUserIdFromToken(string token);
    Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string accessToken, string refreshToken);
    bool ValidateCurrentToken(string token);
}