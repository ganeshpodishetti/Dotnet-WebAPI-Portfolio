namespace Domain.Options;

public class JwtTokenOptions
{
    public const string JwtConfig = "JwtConfig";
    public string? Key { get; init; }
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
    public double AccessTokenExpirationMinutes { get; init; }
    public int RefreshTokenExpirationDays { get; set; }
}