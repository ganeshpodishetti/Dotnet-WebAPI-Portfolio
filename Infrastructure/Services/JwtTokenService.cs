using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

internal class JwtTokenService(
    IOptions<JwtOptions> jwtOptions,
    UserManager<User> userManager) : IJwtTokenService
{
    // Generate JWT Token
    public async Task<string> GenerateJwtToken(User user)
    {
        var jwtSettings = jwtOptions.Value;
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Key))
            throw new InvalidOperationException("JWT secret key is not configured.");

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Key));

        var singingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = await GetClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(singingCredentials, claims);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return jwtToken;
    }

    // Generate Refresh Token
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    // Get User Id from Token
    public string GetUserIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Token is required");

        token = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

        var jwtSettings = jwtOptions.Value;
        var key = Encoding.UTF8.GetBytes(jwtSettings.Key!);
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Value.Issuer,
            ValidAudience = jwtOptions.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, parameters, out _);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            throw new Exception("Invalid token");

        return userId;
    }

    // Get Claims
    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user?.UserName ?? string.Empty),
            new(ClaimTypes.NameIdentifier, user?.Id.ToString() ?? string.Empty),
            new(ClaimTypes.Email, user?.Email ?? string.Empty),
            new("FirstName", user?.Profile.FirstName ?? string.Empty),
            new("LastName", user?.Profile.LastName ?? string.Empty)
        };

        var roles = await userManager.GetRolesAsync(user!);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    // Generate Token Options
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        return new JwtSecurityToken(
            jwtOptions.Value.Issuer,
            jwtOptions.Value.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpiryInMinutes),
            signingCredentials: signingCredentials
        );
    }
}