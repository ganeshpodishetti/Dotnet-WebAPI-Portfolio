using FluentValidation;

namespace Application.DTOs.Experience;

public record ExperienceRequestDto
{
    public required string Title { get; set; }
    public required string CompanyName { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class ExperienceRequestDtoValidator : AbstractValidator<ExperienceRequestDto>
{
    public ExperienceRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100);
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(100);
        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(100);
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date.");
        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}