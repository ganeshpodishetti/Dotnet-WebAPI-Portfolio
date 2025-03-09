using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public string? Headline { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
