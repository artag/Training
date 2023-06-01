namespace Common;

public static class ResultExtensions
{
    public static Result<TO> Bind<TI, TO>(
        this Result<TI> result,
        Func<TI, Result<TO>> func)
    {
        if (result.IsFailure)
            return Result.Failure<TO>(result.Error!);

        return func(result.Value!);
    }

    public static Result<TO> Map<TI, TO>(
        this Result<TI> result,
        Func<TI, TO> func)
    {
        if (result.IsFailure)
            return Result.Failure<TO>(result.Error!);

        return Result.Success<TO>(func(result.Value!));
    }
}
