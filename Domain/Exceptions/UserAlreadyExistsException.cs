namespace Domain.Exceptions;

public class UserAlreadyExistsException(string value, bool isEmail = false)
    : Exception(GetMessage(value, isEmail))
{
    public string Value { get; } = value;
    public bool IsEmail { get; } = isEmail;

    private static string GetMessage(string value, bool isEmail)
    {
        return isEmail
            ? $"User with email '{value}' already exists"
            : $"User with username '{value}' already exists";
    }
}