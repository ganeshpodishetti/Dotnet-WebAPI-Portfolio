namespace Application.DTOs.Message;

public class MessageResponseDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Content { get; set; }
    public string? SentAt { get; set; }
    public string? TimesAgo { get; set; }
    public bool IsRead { get; set; }
}