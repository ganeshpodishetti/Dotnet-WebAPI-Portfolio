namespace Domain.Options;

public class JwtOptions
{
    public const string JwtConfig = "JwtConfig";
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public double ExpiryInMinutes { get; set; }
}