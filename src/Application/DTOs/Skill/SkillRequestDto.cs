using FluentValidation;

namespace Application.DTOs.Skill;

public record SkillRequestDto(string SkillCategory, List<string> SkillsTypes);

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