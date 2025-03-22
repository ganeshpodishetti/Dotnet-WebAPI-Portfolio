using FluentValidation;

namespace Application.DTOs.Skill;

public record SkillRequestDto
{
    public required string SkillCategory { get; set; }
    public required List<string> SkillsTypes { get; set; }
}

public class SkillRequestValidator : AbstractValidator<SkillRequestDto>
{
    public SkillRequestValidator()
    {
        RuleFor(x => x.SkillCategory)
            .NotEmpty().WithMessage("SkillCategory is required.")
            .MaximumLength(100);
        RuleFor(x => x.SkillsTypes)
            .NotEmpty().WithMessage("SkillsTypes are required.");
    }
}