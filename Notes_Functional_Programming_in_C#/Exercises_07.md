# Exercises 07

1. Partial application with a binary arithmetic function:

1.1. Write a function, `Remainder`, that calculates the remainder of integer division
(and works for negative input values!). Notice how the expected order of
parameters isn't the one that's most likely to be required by partial application
(you're more likely to partially apply the divisor (делитель)).

```csharp
Func<int, int, Option<int>> Remainder = (dividend, divisor) =>
    divisor == 0
        ? None
        : Some(dividend % divisor);
```

Manually enabling partial application:

```csharp
Func<int, Func<int, Option<int>>> RemainderInternal = divisor => dividend =>
    divisor == 0
        ? None
        : Some(dividend % divisor);

Func<int, Option<int>> RemainderDivideBy5 =>
    RemainderInternal(5);

Func<int, Option<int>> RemainderDivideBy0 =>
    RemainderInternal(0);
```

1.2. Write an `ApplyR` function that gives the rightmost parameter to a given
binary function. (Try to do so without looking at the implementation for
`Apply`.) Write the signature of `ApplyR` in arrow notation, both in curried and
non-curried forms.

```csharp
// ApplyR: (((T1, T2) -> R), T2) -> T1 -> R
// ApplyR: (T1 -> T2 -> R) -> T2 -> T1 -> R (curried form)
Func<T1, R> ApplyR<T1, T2, R>(this Func<T1, T2, R> func, T2 t2) =>
    t1 => func(t1, t2);
```

1.3. Use `ApplyR` to create a function that returns the remainder of dividing any
number by 5.

```csharp
Func<int, Option<int>> RemainderDivideBy5_v2 = Remainder.ApplyR(5);
```

1.4. Write an overload of `ApplyR` that gives the rightmost argument to a ternary
function.

```csharp
Func<T1, T2, R> ApplyR<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T3 t3) =>
    (t1, t2) => func(t1, t2, t3);
```

Тест, для тестирования 1.1 - 1.3:

```csharp
[Test]
public void Eight_divide_by_five_Return_some_three()
{
    var expected = Some(3);
    var x = 8;
    var y = 5;      // 8 % 5

    var actual1 = Remainder(x, y);          // Обычный метод
    Assert.AreEqual(expected, actual1);

    var actual2 = RemainderDivideBy5(x);    // Manually enabling partial application
    Assert.AreEqual(expected, actual2);

    var partial3 = Remainder.Apply(x);                  // Apply
    var actual3 = partial3(y);
    Assert.AreEqual(expected, actual3);

    // RemainderDivideBy5_v2 = Remainder.ApplyR(5);
    var actual4 = RemainderDivideBy5_v2(x);             // ApplyR
    Assert.AreEqual(expected, actual4);
}
```

2. Ternary functions:

2.1. Define a `PhoneNumber` class with three fields: number type (home, mobile, ...),
country code ('it', 'uk', ...), and number. `CountryCode` should be a custom type
with implicit conversion to and from `string`.

```csharp
class PhoneNumber
{
    public PhoneNumber(NumberType numberType, CountryCode countryCode, string number)
    {
        NumberType = numberType;
        CountryCode = countryCode;
        Number = number;
    }

    public NumberType NumberType { get; }
    public CountryCode CountryCode { get; }
    public string Number { get; }
}

enum NumberType
{
    Home,
    Mobile,
}

class CountryCode
{
    private string _value;

    private CountryCode(string code)
    {
        _value = code;
    }

    public static implicit operator CountryCode(string code) =>
        new CountryCode(code);

    public static implicit operator string(CountryCode code) =>
        code._value;

    public override string ToString() =>
        _value;
}
```

2.2. Define a ternary function that creates a new number, given values for these
fields. What's the signature of your factory function?

```csharp
// CreatePhoneNumber: CountryCode -> NumberType -> string -> PhoneNumber
static Func<CountryCode, NumberType, string, PhoneNumber> CreatePhoneNumber =
    (countryCode, numberType, number) => new PhoneNumber(numberType, countryCode, number);
```

2.3. Use partial application to create a binary function that creates a UK number,
and then again to create a unary function that creates a UK mobile.

```csharp
static Func<NumberType, string, PhoneNumber> СreateUKPhoneNumber =
    CreatePhoneNumber.Apply((CountryCode)"uk");

static Func<string, PhoneNumber> CreateUKMobilePhoneNumber =
    СreateUKPhoneNumber.Apply(NumberType.Mobile);
```

3. Functions everywhere. You may still have a feeling that objects are ultimately
more powerful than functions. Surely a logger object should expose methods
for related operations such as `Debug`, `Info`, and `Error`? To see that this is not
necessarily so, challenge yourself to write a very simple logging mechanism
(logging to the console is fine) that doesn't require any classes or structs. You
should still be able to inject a Log value into a consumer class or function,
exposing the operations Debug , Info , and Error , like so:
`void ConsumeLog(Log log) => log.Info("look! no classes!");`

Реализация:

```csharp
enum Level { Debug, Info, Error }

delegate void Log(Level level, string message);

static Log consoleLogger = (level, message) =>
    Console.WriteLine($"[{level}]: message");

static void Debug(this Log log, string message) =>
    consoleLogger(Level.Debug, message);

static void Info(this Log log, string message) =>
    consoleLogger(Level.Info, message);

static void Error(this Log log, string message) =>
    consoleLogger(Level.Debug, message);

static void _main() =>
    ConsumeLog(consoleLogger);

static void ConsumeLog(Log log) => log.Info("look! no objects!");
```
