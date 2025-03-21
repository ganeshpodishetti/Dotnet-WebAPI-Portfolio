using Domain.Common;
using Domain.UnitOfWork;

namespace Domain.Entities;

public class Education : BaseEntity, IUserEntity
{
    public required string School { get; set; }
    public required string Degree { get; set; }
    public required string Location { get; set; }
    public string? FieldOfStudy { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
}