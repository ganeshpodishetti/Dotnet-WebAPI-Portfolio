namespace Application.DTOs;

public record UserProfileDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string ProfilePicture { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string Headline { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string UpdatedAt { get; set; } = null!;
}