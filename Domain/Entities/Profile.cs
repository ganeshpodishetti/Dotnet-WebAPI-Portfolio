namespace Domain.Entities;

public class Profile
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public required string Headline { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}