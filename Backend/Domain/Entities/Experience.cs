using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Experience : BaseEntity, IUserEntity
{
    public required string Title { get; init; }
    public required string CompanyName { get; init; }
    public string? Location { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public string? Description { get; init; }
    public Guid UserId { get; set; }
}