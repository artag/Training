# Chapter 3

## Designing function signatures and types

## 3.1 Function signature design

### 3.1.1 Arrow notation

Hindley-Milner type signatures in functional languages (F#, Haskell):
```text
f : int -> string
```

"f has type of int to string" - (у функции f тип int в строку)

"f takes an int and yields a string" - (f принимает значение int и выдает string)

In C#: `Func<int, string>`

Function signature |C# type              |Example
-------------------|---------------------|-------------------------------------
int -> string      | Func<int, string>   | (int i) => i.ToString()
() -> string       | Func<string>        | () => "hello"
int -> ()          | Action<int>         | (int i) => WriteLine($"gimme {i}")
() -> ()           | Action              | () => WriteLine("Hello World!")
(int, int) -> int  | Func<int, int, int> | (int a, int b) => a + b    // (int a, int b) - tuple

For HOF's code:

```csharp
public static R Connect<R>(string connStr, Func<IDbConnection, R> func) =>
    Using(
        new SqlConnection(connStr),
        conn => { conn.Open(); return func(conn); });
```

HOF's signature:

```text
(string, (IDbConnection -> R)) -> R
```

C# type: `Func<string, Func<IDbConnection, R>, R>`

### 3.1.2 How informative is a signature?

Signature of `Enumerable.Where`:

```text
(IEnumerable<T>, (T -> bool)) -> IEnumerable<T>
```

Signature of `Enumerable.Zip` (`Zip` - the operation of pairing two parallel lists):

```text
(IEnumerable<A>, IEnumerable<B>, ((A, B) -> C)) -> IEnumerable<C>
```

Некоторые сигнатуры гораздо информативнее других. При разработке API желательно делать
сигнатуры функций как можно более выразительными. Это облегчит использование API и
добавит надежности программам.

## 3.2 Capturing data with data objects

To represent data, we use *data objects*: objects that contain data, but no logic.
These are also called *"anemic"* objects.

In FP (unlike OOP) it's natural to draw a separation between logic and data:

* Logic is encoded in functions.
* Data is captured with data objects, which are used as inputs and outputs to these functions.

Example:

```csharp
// The risk profile
enum Risk { Low, Medium, High }

Risk CalculateRiskProfile(int age) =>
    (age < 60) ? Risk.Low : Risk.Medium;

CalculateRiskProfile(30)        // => Low
CalculateRiskProfile(70)        // => Medium
CalculateRiskProfile("Hello")   // => compiler error: cannot convert from 'string' to 'int'
```

### 3.2.1 Primitive types are often not specific enough

Можно ли улучшить предыдущий код и сделать более явным входное значение?

Ведь можно ввести отрицательное или слишком большое число в качестве age:

```csharp
CalculateRiskProfile(-1000) // => Low
CalculateRiskProfile(10000) // => Medium
```

Стандартное решение в стиле ООП. Ввести валидацию с исключением:

```csharp
Risk CalculateRiskProfile(int age)
{
    if (age < 0 || 120 <= age)
        throw new ArgumentException($"{age} is not a valid age");
    return (age < 60) ? Risk.Low : Risk.Medium;
}

CalculateRiskProfile(10000); // => runtime error: 10000 is not a valid age
```

Недостатки такого подхода:

* You'll have to write additional unit tests for the cases in which validation fails.

* In other areas of the application where an age is expected, so you're probably going
to need the same validation in those places. This will cause some duplication.

* Separation of concerns has been broken. `CalculateRiskProfile` не должна делать валидацию
входных значений.

Можно ли сделать лучше?

### 3.2.2 Constraining inputs with custom types

Implement `Age` - a custom type that can only be instantiated with a valid value for an age:

```csharp
public class Age
{
    // The internal representation is kept private.
    private int Value { get; }

    public Age(int value)
    {
        if (!IsValid(value))
            throw new ArgumentException($"{value} is not a valid age");
        Value = value;
    }

    private static bool IsValid(int age) =>
        0 <= age && age < 120;

    // Logic for comparing an Age with another Age.
    public static bool operator <(Age l, Age r) =>
        l.Value < r.Value;
    public static bool operator >(Age l, Age r) =>
        l.Value > r.Value;

    // For readability, makes it possible to compare an Age with an int;
    // the int will first be converted into an Age.
    public static bool operator <(Age l, int r) =>
        l < new Age(r);
    public static bool operator >(Age l, int r) =>
        l > new Age(r);
}
```

Rewrite previous function:

```csharp
Risk CalculateRiskProfile(Age age) =>
    (age < 60) ? Risk.Low : Risk.Medium;
```

Плюсы такого подхода:

* Используются только правильные значения для возраста

* Валидация возраста делается только в одном месте.

Недостаток:

* В конструкторе `Age` все еще выбрасывается исключение, если входное значение неправильное.
(Далее этот недостаток будет решен - см. главу 3.4.5).

### 3.2.3 Writing "honest" functions

*Honest* (честная) function - функция, результат выполнения которой всегда соответствует ее
сигнатуре.

Например такой вариант функции является honest:

```csharp
Risk CalculateRiskProfile(Age age) =>
    (age < 60) ? Risk.Low : Risk.Medium;
```

Ее сигнатура `Age -> Risk` говорит: "Give me an `Age` and I will give you back a `Risk`."
И больше никаких других возможных вариантов выполнения.

А такой вариант *dishonest*:

```csharp
Risk CalculateRiskProfile(int age)
{
    if (age < 0 || 120 <= age)
        throw new ArgumentException($"{age} is not a valid age");
    return (age < 60) ? Risk.Low : Risk.Medium;
}
```

Ее сигнатура `int -> Risk` говорит: "Give me an `int` (any of the 2^32 possible values for 
`int`) and I'll return a `Risk`."

Но сигнатура не всегда соблюдается - при некоторых `int` будет выбрасываться `ArgumentException`.

Говорят что такая функция *dishonest* (нечестная).
Что ее сигнатура должна говорить в реальности: "Give me an `int`, and I may return a `Risk`,
or I may throw an exception instead."

**Резюме**: function is *honest* if its behavior can be predicted by its signature: it
returns a value of the declared type; no throwing exceptions, and no `null` return values.

### 3.2.4 Composing values with tuples and objects

Введем больше данных для более корректного CalculateRiskProfile:

```csharp
enum Gender { Female, Male }

Risk CalculateRiskProfile(Age age, Gender gender)
{
    var threshold = (gender == Gender.Female) ? 62 : 60;
    return (age < threshold) ? Risk.Low : Risk.Medium;
}
```

Сигнатура: `(Age, Gender) -> Risk`

Всего 2 * 120 = 240 возможных входных комбинаций.

Аналогичное число комбинаций будет, если вместо `Age` и `Gender` задать `HealthData`:

```csharp
class HealthData
{
    public Age Age;
    public Gender Gender;
}
```

**Резюме**: рекомендуется описывать сущности таким образом, чтобы обеспечить точный контроль над
диапазоном входных данных, которые ваши функции должны будут обрабатывать.
Точное/конечное число возможных входных комбинаций вносит ясность. Как только у вас есть контроль
над этими простыми значениями, их легко объединить в более сложные объекты данных.

## 3.3 Modeling the absence of data with `Unit`

### 3.3.1 Why `void` isn't ideal

Write a HOF that starts a stopwatch, runs the given function, and stops the stopwatch,
printing out some diagnostic information:

```csharp
public static class Instrumentation
{
    public static T Time<T>(string op, Func<T> f)
    {
        var sw = new Stopwatch();
        sw.Start();

        T t = f();

        sw.Stop();
        Console.WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
        return t;
    }
}
```

Using:

```csharp
var contents = Instrumentation.Time(
    "reading from file.txt",
    () => File.ReadAllText("file.txt"));
```

If we want to use measure function like this (returning `void`):

```csharp
Instrumentation.Time(
    "writing to file.txt",
    () => File.AppendAllText("file.txt", "New content!", Encoding.UTF8));
```

We you'd need to add an overload of `Instrumentation.Time` that takes an `Action`, like this:

```csharp
public static void Time(string op, Action act)
{
    var sw = new Stopwatch();
    sw.Start();

    act();

    sw.Stop();
    Console.WriteLine($"{op} took {sw.ElapsedMilliseconds}ms");
}
```

This is terrible! You have to duplicate the entire implementation between the `Func` and `Action`
delegates (тоже самое будет для `Task` и `Task<T>`).

How can you avoid this?

### 3.3.2 Bridging the gap between `Action` and `Func` with `Unit`

Instead of using `void`, which is a special language construct, we'll use a special `value`:
the empty tuple. FP convention of calling it `Unit`:

```csharp
// Aliases the empty tuple as Unit
using Unit = System.ValueTuple;

namespace LaYumba.Functional
{
    using static F;

    public static partial class F
    {
        // Convenience (удобный) method that allows you to simply write return
        // Unit() in functions that return Unit.
        public static Unit Unit() => default(Unit);
    }

    // Adapter functions that convert an Action into a Unit-returning Func
    public static class ActionExt
    {
        public static Func<Unit> ToFunc(this Action action) =>
            () => { action(); return Unit(); };

        public static Func<T, Unit> ToFunc<T>(this Action<T> action) =>
            (t) => { action(t); return Unit(); };

        // more overloads to cater (удовлетворить) for Action's with more arguments...
    }
}
```

Writing HOFs that take a `Func` or an `Action`, without duplication:

```csharp
using LaYumba.Functional;
using Unit = System.ValueTuple;

public static class Instrumentation
{
    // Includes an overload that takes an Action.
    // Converts the Action to a Func<Unit> and passes it to the overload taking a Func<T>.
    public static void Time(string op, Action act) =>
        Time<Unit>(op, act.ToFunc());

    public static T Time<T>(string op, Func<T> f)
        // same as before...
}
```

**Резюме**:

* Use `void` to indicate the absence (отсутствие) of data, meaning that your function is only
called for side effects and returns no information.

* Use `Unit` as an alternative, more flexible representation when there's a need for
consistency in the handling of `Func` and `Action`.

## 3.4 Modeling the possible absence of data with `Option`

`Option` gives a more robust and expressive representation of the possible absence of data
(`null` or exception).

### 3.4.1 The bad APIs you use every day

### 3.4.2 An introduction to the Option type

`Option` is essentially a container that wraps a value... or no value.

The symbolic definition for `Option`:

```text
Option<T> = None | Some(T)
```

`Option<T>` can be in one of two "states":

* `None` - A special value indicating the absence of a value.
If the `Option` has no inner value, we say that "the Option is None."

* `Some(T)` - A container that wraps a value of type `T`. If the `Option` has an inner
value, we say that "the Option is Some."

`Option` is also called `Maybe`, with `Just` (like Some) and `Nothing` (like None) states.

#### Using library with REPL

1) Reference the `LaYumba.Functional` library in your REPL:

```text
#r "functional-csharp-code\LaYumba.Functional\bin\Debug\netstandard1.6\LaYumba.Functional.dll"
```

2) Type imports:

```csharp
using LaYumba.Functional;
using static LaYumba.Functional.F;
```

#### Usage Option

```csharp
string greet(Option<string> greetee) =>
    greetee.Match(
        None: () => "Sorry, who?",    // If greetee is None, Match will evaluate this function.
        Some: (name) => $"Hello, {name}");

greet(None)     // => "Sorry, who?"
greet("John")   // => "Hello, John"
```

**Резюме**:

* Use `Some(value)` to wrap a value into an `Option`.

* Use `None` to create an empty `Option`.

* Use `Match` to run some code depending on the state of the `Option`.

### 3.4.3 Implementing `Option` and `Option<T>`

#### Implementing `Option`

```csharp
namespace LaYumba.Functional
{
    public static partial class F
    {
        // The None value
        public static Option.None None =>
            Option.None.Default;

        // The Some function wraps the given value into a Some
        public static Option.Some<T> Some<T>(T value) =>
            new Option.Some<T>(value);
    }

    namespace Option
    {
        // None has no members because it contains no data.
        public struct None
        {
            internal static readonly None Default = new None();
        }

        // Some simply wraps a value.
        public struct Some<T>
        {
            // Some represents the presence of data, so don't allow the null value.
            internal Some(T value)
            {
                if (value == null)
                    throw new ArgumentNullException();
                Value = value;
            }

            internal T Value { get; }       // can't be null .
        }
    }
}
```

Usage:

```csharp
using static LaYumba.Functional.F;
var firstName = Some("Enrico");
var middleName = None;
```

#### Implementing `Option<T>`

In terms of sets, `Option<T>` is the union of the set `Some<T>` with the singleton set `None`.

```csharp
public struct Option<T>
{
    // Captures the state of the Option: true if the Option is Some.
    readonly bool isSome;

    // The inner value of the Option.
    readonly T value;

    private Option(T value)
    {
        this.isSome = true;
        this.value = value;
    }

    // Converts None into an Option
    public static implicit operator Option<T>(Option.None _) =>
        new Option<T>();

    // Converts Some into an Option
    public static implicit operator Option<T>(Option.Some<T> some) =>
        new Option<T>(some.Value);

    // "Lifts" a regular value into an Option
    public static implicit operator Option<T>(T value) =>
        value == null ? None : Some(value);

    // Match takes two functions and evaluates one or the other
    // depending on the state of the Option.
    public R Match<R>(Func<R> onNone, Func<T, R> onSome) =>
        isSome ? onSome(value) : onNone();
}
```

### 3.4.4 Gaining robustness by using `Option` instead of `null`

Example. Some class in DB with some function:

```csharp
public class Subscriber
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public string GreetingFor(Subscriber subscriber) =>
    $"Dear {subscriber.Name.ToUpper()},";
```

Но потом бизнес решает убрать имя из БД (`Subscriber.Name = null`) и теперь
`subscriber.Name.ToUpper()` будет выбрасывать исключение.

Решение - сделать `Name` опциональным:

```csharp
public class Subscriber
{
    // Name is now explicitly marked as optional.
    public Option<string> Name { get; set; }
    public string Email { get; set; }
}
```

Это не только более четко описывает `Subscriber`, но и заставляет внести изменения в метод:

```csharp
public string GreetingFor(Subscriber subscriber) =>
    subscriber.Name.Match(
        () => "Dear Subscriber,",
        (name) => $"Dear {name.ToUpper()},");
```

### 3.4.5 `Option` as the natural result type of partial functions

*Total functions* (общие) - are mappings that are defined for *every* element of the domain.

*Partial functions* (частичные) - are mappings that are defined for *some*, but not all,
elements of the domain.

The `Option` type offers a perfect solution to model partial functions:
if the function is defined for the given input, it returns a `Some` wrapping the result;
otherwise, it returns `None`.

#### Example. Parsing strings

Parsing function with this signature is partial:

```text
string -> int 
```

Parsing function with this signature is total:

```text
string -> Option<int>
```

```csharp
public static class Int
{
    public static Option<int> Parse(string s)
    {
        int result;
        return int.TryParse(s, out result)
            ? Some(result)
            : None;
    }
}
```

Usage:

```csharp
Int.Parse("10")         // => Some(10)
Int.Parse("hello")      // => None
```

#### Example. Looking up data in a collection

Initial version:

```csharp
new NameValueCollection()["green"]          // "green" отсутствует в словаре
// => null

new Dictionary<string, string>()["blue"]
// => runtime error: KeyNotFoundException   // "blue" отсутствует в словаре
```

Решение для `NameValueCollection`:

```csharp
// возвратит Option<string> из-за implicit operator Option<T>(T value)
public static Option<string> Lookup(this NameValueCollection @this, string key) =>
    @this[key];
```

Решение для `IDictionary`:

```csharp
public static Option<T> Lookup<K, T>(this IDictionary<K, T> dict, K key)
{
    T value;
    return dict.TryGetValue(key, out value)
        ? Some(value)
        : None;
}
```

Теперь отсутствующий ключ в словарях не будет приводит к негативным эффектам:

```csharp
new NameValueCollection().Lookup("green")          // => None
new Dictionary<string, string>().Lookup("blue")    // => None
```

#### Example. The smart constructor pattern

*Smart constructor*: it's aware of (знает о) some rules and can prevent the construction of an
invalid object.

Пример smart constructor для `Age` (класс был описан в главе 3.2.2):

```csharp
public struct Age
{
    // The constructor can be marked as private.
    private Age(int value)
    {
        if (!IsValid(value))
            throw new ArgumentException($"{value} is not a valid age");
        Value = value;
    }

    private int Value { get; }

    // A smart constructor returning an Option
    public static Option<Age> Of(int age) =>
        IsValid(age) ? Some(new Age(age)) : None;

    private static bool IsValid(int age) =>
        0 <= age && age < 120;
}
```

#### Guarding against (защита от) `NullReferenceException`

* *Никогда* не писать функции возвращающие `null`.

* Всегда проверять входные аргументы в public методах API на `null` значение.
(Кроме опциональных аргументов, которые должны иметь значение compile-time constant по умолчанию,
отличное от `null`).

**Резюме**:

* Use `Option` in your data objects to model the fact that a property may
not be set, and in your functions to indicate the possibility that a suitable value may
not be returned.

* `Option` will enrich your model and make your code more self-documenting.

* Use `Match` with the functions you'd like to evaluate in the `None` and `Some` cases.
