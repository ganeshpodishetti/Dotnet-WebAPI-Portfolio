using Domain.Enums;

namespace Domain.Common.BaseErrors;

public class GeneralError(string title, string description, StatusCode statusCode)
    : BaseError(title, description, statusCode);