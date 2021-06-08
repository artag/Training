# Result

## IResult

```csharp
public interface IResult
{
    bool IsFailure { get; }
    bool IsSuccess { get; }
}

public interface IValue<out T>
{
    T Value { get; }
}

public interface IError<out E>
{
    E Error { get; }
}

public interface IResult<out T, out E> : IResult, IValue<T>, IError<E>
{
}

public interface IResult<out T> : IResult<T, string>
{
}
```

## Result

```csharp
[Serializable]
public partial struct Result : IResult, ISerializable
{
    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;

    private readonly string _error;
    public string Error => ResultCommonLogic.GetErrorWithSuccessGuard(IsFailure, _error);

    private Result(bool isFailure, string error)
    {
        IsFailure = ResultCommonLogic.ErrorStateGuard(isFailure, error);
        _error = error;
    }

    private Result(SerializationInfo info, StreamingContext context)
    {
        var values = ResultCommonLogic.Deserialize(info);
        IsFailure = values.IsFailure;
        _error = values.Error;
    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) =>
        ResultCommonLogic.GetObjectData(this, info);
}
```

## ResultT

```csharp
[Serializable]
public partial struct Result<T> : IResult<T>, ISerializable
{
    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;

    private readonly string _error;
    public string Error => ResultCommonLogic.GetErrorWithSuccessGuard(IsFailure, _error);

    private readonly T _value;
    public T Value => IsSuccess ? _value : throw new ResultFailureException(Error);

    internal Result(bool isFailure, string error, T value)
    {
        IsFailure = ResultCommonLogic.ErrorStateGuard(isFailure, error);
        _error = error;
        _value = value;
    }

    private Result(SerializationInfo info, StreamingContext context)
    {
        var values = ResultCommonLogic.Deserialize(info);
        IsFailure = values.IsFailure;
        _error = values.Error;
        _value = IsFailure ? default : (T)info.GetValue("Value", typeof(T));
    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        => ResultCommonLogic.GetObjectData(this, info, this);

    public static implicit operator Result<T>(T value)
    {
        if (value is IResult<T> result)
        {
            string resultError = result.IsFailure ? result.Error : default;
            T resultValue = result.IsSuccess ? result.Value : default;

            return new Result<T>(result.IsFailure, resultError, resultValue);
        }

        return Result.Success(value);
    }

    public static implicit operator Result(Result<T> result)
    {
        if (result.IsSuccess)
            return Result.Success();
        else
            return Result.Failure(result.Error);
    }
}
```

## ResultTE

```csharp
[Serializable]
public partial struct Result<T, TE> : IResult<T, TE>, ISerializable
{
    public bool IsFailure { get; }
    public bool IsSuccess => !IsFailure;

    private readonly TE _error;
    public TE Error => ResultCommonLogic.GetErrorWithSuccessGuard(IsFailure, _error);

    private readonly T _value;
    public T Value => IsSuccess ? _value : throw new ResultFailureException<TE>(Error);

    internal Result(bool isFailure, TE error, T value)
    {
        IsFailure = ResultCommonLogic.ErrorStateGuard(isFailure, error);
        _error = error;
        _value = value;
    }

    private Result(SerializationInfo info, StreamingContext context)
    {
        var values = ResultCommonLogic.Deserialize<TE>(info);
        IsFailure = values.IsFailure;
        _error = values.Error;
        _value = IsFailure ? default : (T)info.GetValue("Value", typeof(T));
    }

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) =>
        ResultCommonLogic.GetObjectData(this, info, this);

    public static implicit operator Result<T, TE>(T value)
    {
        if (value is IResult<T, TE> result)
        {
            TE resultError = result.IsFailure ? result.Error : default;
            T resultValue = result.IsSuccess ? result.Value : default;

            return new Result<T, TE>(result.IsFailure, resultError, resultValue);
        }

        return Result.Success<T, TE>(value);
    }

    public static implicit operator Result<T, TE>(TE error)
    {
        if (error is IResult<T, TE> result)
        {
            TE resultError = result.IsFailure ? result.Error : default;
            T resultValue = result.IsSuccess ? result.Value : default;

            return new Result<T, TE>(result.IsFailure, resultError, resultValue);
        }

        return Result.Failure<T, TE>(error);
    }
}
```