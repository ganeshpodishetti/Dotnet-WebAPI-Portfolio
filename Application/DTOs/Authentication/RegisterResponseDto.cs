namespace Application.DTOs.Authentication;

public record RegisterResponseDto
{
    public required string Email { get; init; }
    public string? CreateAt { get; init; }
}