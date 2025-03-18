namespace Application.DTOs.Authentication;

public record RegisterRequestDto
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}