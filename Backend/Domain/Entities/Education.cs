using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Education : BaseEntity, IUserEntity
{
    public required string School { get; init; }
    public required string Degree { get; init; }
    public required string Location { get; init; }
    public string? FieldOfStudy { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public string? Description { get; init; }
    public Guid UserId { get; set; }
}