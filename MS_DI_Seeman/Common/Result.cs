using System.Diagnostics.Metrics;

namespace Common;

public abstract record Error(string? Message, Exception? Exception)
{
    public bool HasException => Exception != null;

    public bool HasMessage => !string.IsNullOrEmpty(Message);
}

public class Result
{
    protected Result(bool success, Error? error)
    {
        IsSuccess = success;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    public static Result Success() =>
        new Result(success: true, error: default);

    public static Result Failure(Error error) =>
        new Result(success: false, error);

    public static Result<T> Success<T>(T value) =>
        new Result<T>(success: true, value, default);

    public static Result<T> Failure<T>(Error error) =>
        new Result<T>(success: false, default, error);
}

public class Result<T> : Result
{
    internal Result(bool success, T? value, Error? error)
        : base(success, error)
    {
        Value = value;
    }

    public T? Value { get; }
}

public struct Unit
{
};
