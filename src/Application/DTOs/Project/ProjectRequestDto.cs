using FluentValidation;

namespace Application.DTOs.Project;

public record ProjectRequestDto(
    string Name,
    string Description,
    string GithubUrl,
    string Url,
    List<string> Skills);

public class ProjectRequestValidator : AbstractValidator<ProjectRequestDto>
{
    public ProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500);
        RuleFor(x => x.Url)
            .MaximumLength(200);
        RuleFor(x => x.GithubUrl)
            .NotEmpty().WithMessage("Github URL is required.")
            .MaximumLength(200);
        RuleFor(x => x.Skills)
            .NotEmpty();
    }
}