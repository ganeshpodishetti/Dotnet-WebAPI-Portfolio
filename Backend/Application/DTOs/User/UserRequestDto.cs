using FluentValidation;

namespace Application.DTOs.User;

public record UserRequestDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? ProfilePicture { get; set; }
    public string? Bio { get; set; }
    public string Headline { get; set; } = null!;
    public string? Country { get; set; }
    public string? City { get; set; }
}

public class UserRequestValidator : AbstractValidator<UserRequestDto>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);
        RuleFor(x => x.Headline)
            .NotEmpty().WithMessage("Headline is required.")
            .MaximumLength(100);

        RuleFor(x => x.ProfilePicture)
            .MaximumLength(200);

        RuleFor(x => x.Bio)
            .MaximumLength(1000);

        RuleFor(x => x.Country)
            .MaximumLength(100);

        RuleFor(x => x.City)
            .MaximumLength(100);
    }
}