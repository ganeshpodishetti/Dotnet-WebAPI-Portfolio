using Domain.Enums;

namespace Domain.Errors;

public class GeneralError(string title, string description, StatusCode statusCode)
    : BaseError(title, description, statusCode);