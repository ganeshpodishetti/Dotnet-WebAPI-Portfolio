namespace Domain.Exceptions;

public class PasswordViolationException(string message)
    : Exception(message)
{
}