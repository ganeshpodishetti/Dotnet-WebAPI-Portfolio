namespace Application.DTOs.User;

public record UserRequestDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public string Headline { get; set; } = null!;
    public string? Country { get; set; }
    public string? City { get; set; }
}