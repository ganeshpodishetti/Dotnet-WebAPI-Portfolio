namespace Application.DTOs.Authentication;

public record RegisterResponseDto
{
    public string? Email { get; init; }
    public string? CreateAt { get; init; }
}