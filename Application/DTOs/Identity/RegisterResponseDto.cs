namespace Application.DTOs.Identity;

public record RegisterResponseDto
{
    public required string Email { get; init; }
    public string? CreateAt { get; init; }
}