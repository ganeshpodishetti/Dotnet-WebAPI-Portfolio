namespace Application.DTOs.Authentication;

public record ChangePasswordDto
{
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
}