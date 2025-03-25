using Domain.Enums;
using Domain.Errors;

namespace Domain.Common;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success()
    {
        return new Result(true, Error.None(string.Empty, string.Empty, StatusCode.Success));
    }

    public static Result Failure(string errorMessage)
    {
        return new Result(false, Error.Failure(errorMessage, errorMessage));
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;

    protected Result(T value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value => IsSuccess ? _value! : throw new InvalidOperationException();

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, Error.None(string.Empty, string.Empty, StatusCode.Success));
    }

    public new static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(default!, false, Error.Failure(errorMessage, errorMessage));
    }

    public new static Result<T> Failure(Error error)
    {
        return new Result<T>(default!, false, error);
    }
}