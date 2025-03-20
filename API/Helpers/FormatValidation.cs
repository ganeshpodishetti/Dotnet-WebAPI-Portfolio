using FluentValidation.Results;

namespace API.Helpers;

public interface IFormatValidation
{
    object FormatValidationErrors(ValidationResult validationResult);
}

public class FormatValidation : IFormatValidation
{
    public object FormatValidationErrors(ValidationResult validationResult)
    {
        var detailedErrors = validationResult.Errors
            .Select(e => new
            {
                Field = e.PropertyName,
                Message = e.ErrorMessage
            })
            .ToList();

        // For teaching purposes, return both formats to show the difference
        return new
        {
            Title = "Validation Failed",
            Status = StatusCodes.Status400BadRequest,
            DetailedErrors = detailedErrors
        };
    }
}