namespace Domain.Entities;

public class Profile
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string ProfilePicture { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string Headline { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
}