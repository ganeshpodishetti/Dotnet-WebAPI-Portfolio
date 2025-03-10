using Domain.Common;

namespace Domain.Entities;

public class Experience : BaseEntity
{
    public required string Title { get; set; }
    public required string CompanyName { get; set; }
    public string? Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
}
