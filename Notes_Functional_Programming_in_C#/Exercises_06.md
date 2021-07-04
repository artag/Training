# Exercises 06

1. Write a `ToOption` extension method to convert an `Either` into an
`Option`. Then write a `ToEither` method to convert an `Option` into an `Either`,
with a suitable parameter that can be invoked to obtain the appropriate `Left` value,
if the `Option` is `None`. (Tip: start by writing the function signatures in arrow notation).

```csharp
public static Option<R> ToOption<L, R>(this Either<L, R> either) =>
    either.Match(
        Left: _ => None,
        Right: value => Some(value));
```

```csharp
// ToEither<L, R>: (Option<R>, Func<L>) -> Either<L, R>
public static Either<L, R> ToEither<L, R>(this Option<R> option, Func<L> left) =>
    option.Match<Either<L, R>>(
        None: () => Left(left()),
        Some: value => Right(value));
```

2.1. Take a workflow where 2 or more functions that return an `Option` are chained using `Bind`.

```csharp
static Option<int> Parse(string num) =>
    int.TryParse(num, out var result)
        ? Some(result)
        : None;

static Option<int> Divide(int x, int y) =>
    y == 0
        ? None
        : Some(x / y);

static Option<(int X, int Y)> Parse(string x, string y)
{
    var parsedX = 0;
    return Parse(x)
        .Bind(px =>
        {
            parsedX = px;
            return Parse(y);
        })
        .Map(parsedY => (parsedX, parsedY));
}

static Func<string, string, Option<int>> ParseAndDivide =
    (x, y) => Parse(x, y)
        .Bind(pair => Divide(pair.X, pair.Y));
```

Tests:

```csharp
[Test]
public void Test_Parse_method()
{
    var actual1 = Parse("a", "1");
    Assert.AreEqual(None, actual1);

    var actual2 = Parse("1", "a");
    Assert.AreEqual(None, actual2);

    var actual3 = Parse("a", "a");
    Assert.AreEqual(None, actual3);

    var actual4 = Parse("1", "1");
    Assert.AreEqual(Some((1, 1)), actual4);
}

[Test]
public void Test_ParseAndDivide_method()
{
    var act1 = ParseAndDivide("12", "3");
    Assert.AreEqual(Some(4), act1);

    var act2 = ParseAndDivide("12", "0");
    Assert.AreEqual(None, act2);

    var act3 = ParseAndDivide("a", "3");
    Assert.AreEqual(None, act3);

    var act4 = ParseAndDivide("12", "a");
    Assert.AreEqual(None, act4);
}
```

2.2. Then change the first one of the functions to return an `Either`.

```csharp
static Either<string, (int X, int Y)> Parse2(string x, string y)
{
    var parsedX = 0;
    var parseX = Parse(x).ToEither(() => $"Parse error {x}");
    var parseY = Parse(y).ToEither(() => $"Parse error {y}");
    return parseX
        .Bind(px =>
        {
            parsedX = px;
            return parseY;
        })
        .Map(parsedY => (parsedX, parsedY));
}
```

Tests:

```csharp
[Test]
public void Test_Parse2_method()
{
    var actual1 = Parse2("a", "1").ToString();
    Assert.AreEqual(Left("Parse error a").ToString(), actual1);

    var actual2 = Parse2("1", "b").ToString();
    Assert.AreEqual(Left("Parse error b").ToString(), actual2);

    var actual3 = Parse2("c", "d").ToString();
    Assert.AreEqual(Left("Parse error c").ToString(), actual3);

    var actual4 = Parse2("1", "1").ToString();
    Assert.AreEqual(Right((1, 1)).ToString(), actual4);
}
```

2.3. This should cause compilation to fail. Since `Either` can be
converted into an `Option` as we have done in the previous exercise,
write extension overloads for `Bind`, so that
functions returning `Either` and `Option` can be chained with `Bind`,
yielding an `Option`.

```csharp
static Option<TR2> BindCustom<L, TR1, TR2>(
    this Either<L, TR1> either, Func<TR1, Option<TR2>> func) =>
        either.Match<Option<TR2>>(
            Left: _ => None,
            Right: value => func(value));
```

```csharp
static Func<string, string, Option<int>> ParseAndDivide2 =
    (x, y) => Parse2(x, y)
        .BindCustom(pair => Divide(pair.X, pair.Y));
```

2.4. Использование `ToOption` для конвертации `Either` в `Option`.

```csharp
static Func<string, string, Option<int>> ParseAndDivide3 =
    (x, y) => Parse2(x, y)
        .ToOption()
        .Bind(pair => Divide(pair.X, pair.Y));
```

3. Write a function `Safely` of type
`((() -> R), (Exception -> L)) -> Either<L, R>` that will
run the given function in a `try/catch`, returning an appropriately
populated `Either`.

```csharp
static Either<L, R> Safely<L, R>(this Func<R> func, Func<Exception, L> left)
{
    try
    {
        return func();
    }
    catch(Exception ex)
    {
        return left(ex);
    }
}
```

4. Write a function `Try` of type `(() → T) → Exceptional<T>` that will
run the given function in a `try/catch`, returning an appropriately
populated `Exceptional`.

```csharp
static Exceptional<T> Try<T>(this Func<T> func)
{
    try
    {
        return func();
    }
    catch (Exception ex)
    {
        return ex;
    }
}
```
