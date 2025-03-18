namespace Application.DTOs.Authentication;

public record LoginRequestDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}