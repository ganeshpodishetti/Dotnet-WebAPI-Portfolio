namespace Application.DTOs.Message;

public record MessageResponseDto(
    string Id,
    string Name,
    string Email,
    string Content,
    string SentAt,
    bool IsRead);