using FluentValidation;

namespace Application.DTOs.Message;

public record MessageRequestDto(
    string Name,
    string Email,
    string Subject,
    string Content);

public class MessageRequestValidator : AbstractValidator<MessageRequestDto>
{
    public MessageRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.Content)
            .NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Subject)
            .MaximumLength(500);
    }
}