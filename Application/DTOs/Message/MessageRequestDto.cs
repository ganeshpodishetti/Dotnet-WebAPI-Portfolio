namespace Application.DTOs.Message;

public record MessageRequestDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Content { get; set; }
    public string? SentAt { get; set; }
    public bool IsRead { get; set; }
}