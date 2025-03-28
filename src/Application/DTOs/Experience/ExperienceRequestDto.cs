using FluentValidation;

namespace Application.DTOs.Experience;

public record ExperienceRequestDto(
    string Title,
    string CompanyName,
    string Location,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Description);

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