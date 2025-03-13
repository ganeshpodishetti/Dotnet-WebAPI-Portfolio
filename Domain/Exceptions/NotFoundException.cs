namespace Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceIdentifier)
    : Exception($"{resourceType} with id: '{resourceIdentifier}' was not found.");