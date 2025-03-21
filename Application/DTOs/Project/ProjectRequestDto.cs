using FluentValidation;

namespace Application.DTOs.Project;

public record ProjectRequestDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Url { get; set; }
    public string? GithubUrl { get; set; }
    public List<string>? Skills { get; set; } = [];
}

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