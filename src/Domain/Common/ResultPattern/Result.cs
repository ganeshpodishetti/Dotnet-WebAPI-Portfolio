using Domain.Common.BaseErrors;

namespace Domain.Common.ResultPattern;

// This is a generic result class that can be used to return a value or an error.
public class Result<TValue>
{
    private readonly TValue? _value;

    private Result(TValue value)
    {
        IsSuccess = true;
        _value = value;
        Error = null;
    }

    private Result(BaseError error)
    {
        IsSuccess = false;
        _value = default;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public BaseError? Error { get; }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value when Result is a failure");

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static Result<TValue> Failure(BaseError error)
    {
        return new Result<TValue>(error);
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }

    public static implicit operator Result<TValue>(BaseError error)
    {
        return Failure(error);
    }
}

// This is a non-generic result class that can be used to return an error.
public class Result
{
    private Result(bool isSuccess, BaseError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    private bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public BaseError? Error { get; }

    public static Result Success()
    {
        return new Result(true, null);
    }

    private static Result Failure(BaseError error)
    {
        return new Result(false, error);
    }

    public static implicit operator Result(BaseError error)
    {
        return Failure(error);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return Result<TValue>.Success(value);
    }

    public static Result<TValue> Failure<TValue>(BaseError error)
    {
        return Result<TValue>.Failure(error);
    }
}