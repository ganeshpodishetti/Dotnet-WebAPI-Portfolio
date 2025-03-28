namespace Application.DTOs.Authentication;

public record RegisterResponseDto(
    string UserId,
    string Email,
    string CreateAt);