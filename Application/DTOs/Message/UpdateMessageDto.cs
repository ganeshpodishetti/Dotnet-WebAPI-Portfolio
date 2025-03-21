using FluentValidation;

namespace Application.DTOs.Message;

public class UpdateMessageDto
{
    public required bool IsRead { get; set; }
}

public class UpdateMessageDtoValidator : AbstractValidator<UpdateMessageDto>
{
    public UpdateMessageDtoValidator()
    {
        RuleFor(x => x.IsRead).NotNull();
    }
}