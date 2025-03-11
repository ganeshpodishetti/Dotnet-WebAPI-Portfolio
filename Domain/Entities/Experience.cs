using Domain.Common;

namespace Domain.Entities;

public class Experience : BaseEntity
{
    public string Title { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string Location { get; set; } = null!;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = null!;
    public Guid UserId { get; set; }
} 
