# Exceptional

```csharp
public static partial class F
{
    public static Exceptional<T> Exceptional<T>(T value) =>
        new Exceptional<T>(value);
}

public struct Exceptional<T>
{
    internal Exception Ex { get; }
    internal T Value { get; }

    public bool Success =>
        Ex == null;

    public bool Exception =>
        Ex != null;

    internal Exceptional(Exception ex)
    {
        if (ex == null) throw new ArgumentNullException(nameof(ex));
        Ex = ex;
        Value = default(T);
    }

    internal Exceptional(T right)
    {
        Value = right;
        Ex = null;
    }

    public static implicit operator Exceptional<T>(Exception left) =>
        new Exceptional<T>(left);

    public static implicit operator Exceptional<T>(T right) =>
        new Exceptional<T>(right);

    public TR Match<TR>(Func<Exception, TR> Exception, Func<T, TR> Success) =>
        this.Exception ? Exception(Ex) : Success(Value);

    public Unit Match(Action<Exception> Exception, Action<T> Success) =>
        Match(Exception.ToFunc(), Success.ToFunc());

    public override string ToString() =>
        Match(
            ex => $"Exception({ex.Message})",
            t => $"Success({t})");
}

public static class Exceptional
{
    // creating a new Exceptional

    public static Func<T, Exceptional<T>> Return<T>() =>
        t => t;

    public static Exceptional<R> Of<R>(Exception left) =>
        new Exceptional<R>(left);

    public static Exceptional<R> Of<R>(R right) =>
        new Exceptional<R>(right);

    // applicative

    public static Exceptional<R> Apply<T, R>(
        this Exceptional<Func<T, R>> exF, Exceptional<T> arg) =>
            exF.Match(
                Exception: ex => ex,
                Success: func => arg.Match(
                    Exception: ex => ex,
                    Success: t => new Exceptional<R>(func(t))));

    // functor

    public static Exceptional<RR> Map<R, RR>(
        this Exceptional<R> exF, Func<R, RR> func) =>
            exF.Success ? func(exF.Value) : new Exceptional<RR>(exF.Ex);

    public static Exceptional<Unit> ForEach<R>(
        this Exceptional<R> exF, Action<R> act) =>
            Map(exF, act.ToFunc());

    public static Exceptional<RR> Bind<R, RR>(
        this Exceptional<R> exF, Func<R, Exceptional<RR>> func) =>
            exF.Success ? func(exF.Value) : new Exceptional<RR>(exF.Ex);

    // LINQ

    public static Exceptional<R> Select<T, R>(
        this Exceptional<T> exF, Func<T, R> map) =>
            exF.Map(map);

    public static Exceptional<RR> SelectMany<T, R, RR>(
        this Exceptional<T> exF, Func<T, Exceptional<R>> bind, Func<T, R, RR> project)
    {
        if (exF.Exception)
            return new Exceptional<RR>(exF.Ex);

        var bound = bind(exF.Value);

        return bound.Exception
           ? new Exceptional<RR>(bound.Ex)
           : project(exF.Value, bound.Value);
    }
}
```
