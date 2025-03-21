using FluentValidation;

namespace Application.DTOs.Skill;

public record SkillRequestDto
{
    public required string SkillType { get; set; }
    public required List<string> Skills { get; set; }
}

public class SkillRequestValidator : AbstractValidator<SkillRequestDto>
{
    public SkillRequestValidator()
    {
        RuleFor(x => x.SkillType)
            .NotEmpty().WithMessage("Skill type is required.")
            .MaximumLength(100);
        RuleFor(x => x.Skills)
            .NotEmpty().WithMessage("Skills are required.");
    }
}