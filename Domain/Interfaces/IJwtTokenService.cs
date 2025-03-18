using Domain.Entities;

namespace Domain.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateJwtToken(User user);
    string GenerateRefreshToken();
    Guid GetUserIdFromToken(string token);
}