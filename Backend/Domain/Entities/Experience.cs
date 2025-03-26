using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Experience : BaseEntity, IUserEntity
{
    public required string Title { get; set; }
    public required string CompanyName { get; set; }
    public string? Location { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
}