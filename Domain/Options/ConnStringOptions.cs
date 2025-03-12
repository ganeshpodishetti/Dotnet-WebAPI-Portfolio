namespace Domain.Options;

public class ConnStringOptions
{
    public const string ConnectionStrings = "ConnectionStrings";
    public string PostgresSqlConnection { get; init; } = null!;
}