using FluentValidation;

namespace Application.DTOs.Education;

public record EducationRequestDto(
    string School,
    string Degree,
    string Location,
    string FieldOfStudy,
    DateOnly StartDate,
    DateOnly? EndDate,
    string Description);

public class EducationRequestValidator : AbstractValidator<EducationRequestDto>
{
    public EducationRequestValidator()
    {
        RuleFor(x => x.School)
            .NotEmpty().WithMessage("School is required.");

        RuleFor(x => x.Degree)
            .NotEmpty().WithMessage("Degree is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue)
            .WithMessage("End date must be greater than or equal to start date.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}