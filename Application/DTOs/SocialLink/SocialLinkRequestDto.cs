using FluentValidation;

namespace Application.DTOs.SocialLink;

public record SocialLinkRequestDto
{
    public required string Platform { get; set; }
    public required string Url { get; set; }
    public string? Icon { get; set; }
}

public class SocialLinkRequestValidator : AbstractValidator<SocialLinkRequestDto>
{
    public SocialLinkRequestValidator()
    {
        RuleFor(x => x.Platform)
            .NotEmpty().WithMessage("Platform is required.")
            .MaximumLength(100);
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL is required.")
            .MaximumLength(200);
        RuleFor(x => x.Icon)
            .MaximumLength(200);
    }
}