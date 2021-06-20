# Chapter 4

## Patterns in functional programming

## 4.1 Applying a function to a structure's inner values

### 4.1.1 Mapping a function onto a sequence

> ### A note about naming
> In FP variable names:
> * `t` - a value of type `T`
> * `ts` - a collection of `T`'s
> * `f` (`g`, `h`, and so on) - function

`Map` applies a function to each element of the given `IEnumerable`:

```csharp
public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f)
{
    foreach (var t in ts)
        yield return f(t);
}
```

Signature:

```text
(IEnumerable<T>, (T -> R)) -> IEnumerable<R>
```

`Map` function функционально эквивалентна `Select` from LINQ.

Т.к. `Select` оптимизирован для `IEnumerable`, лучше определить так -
Definition of `Map` for `IEnumerable`:

```csharp
public static IEnumerable<R> Map<T, R>(this IEnumerable<T> ts, Func<T, R> f) =>
    ts.Select(f);
```

Simple usage:

```csharp
Func<int, int> times3 = x => x * 3;

Range(1, 3).Map(times3);
// => [3, 6, 9]
```

### 4.1.2 Mapping a function onto an `Option`

Signature:

```text
(Option<T>, (T -> R)) -> Option<R>
```

Definition of `Map` for `Option`:

```csharp
public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f) =>
    optT.Match(
        () => None,
        (t) => Some(f(t)));
```

Удобно думать, что `Map` для `Option` это специальный список, который может быть пустым (`None`)
или содержать какое-либо значение (`Some`).

Simple example:

```csharp
Func<string, string> greet = name => $"hello, {name}";

Option<string> _ = None;
Option<string> optJohn = Some("John");

_.Map(greet);           // => None
optJohn.Map(greet);     // => Some("hello, John")
```

Пример с яблоками, из которых делается яблочный пирог:

* При пустой корзине с яблоками остается пустая корзина.
* Корзина с яблоками превращается в корзину с яблочным пирогом:

```csharp
class Apple { }
class ApplePie { public ApplePie(Apple apple) { } }

Func<Apple, ApplePie> makePie = apple => new ApplePie(apple);

Option<Apple> full = Some(new Apple());
Option<Apple> empty = None;

full.Map(makePie)       // => Some(ApplePie)
empty.Map(makePie)      // => None
```

### 4.1.3 How Option raises the level of abstraction

Из предыдущей главы (см. главу 3.2.2):

```csharp
Risk CalculateRiskProfile(Age age) =>
    (age.Value < 60) ? Risk.Low : Risk.Medium;
```

```csharp
class Subject
{
    public Option<Age> Age { get; set; }
    // many more fields...
}
```

Compute the `Risk` of a particular `Subject`:

```csharp
Option<Risk> RiskOf(Subject subject) =>
    subject.Age.Map(CalculateRiskProfile);
```

Because Risk is based on the subject's age, which is optional, the computed Risk is also optional.

### 4.1.4 Introducing functors

Generalize the signature of `Map`:

```text
(C<T>, (T -> R)) -> C<R>
```

`C<T>` indicate a generic "container" that wraps some inner value(s) of type `T`.

При такой записи `Map` может быть определен как *functor*:

* В математике *functor* это mapping функция.
* В программировании *functor* это контейнер, на котором можно сделать map для
функции.

Таким образом `IEnumerable` and `Option` являются functors.

Для практических целей можно сказать, что все, что имеет разумную реализацию `Map`,
является функтором:

* `Map` должна применять функцию к внутренним значениям контейнера
* У `Map` не должно быть побочных эффектов (больше не должен ничего делать).

## 4.2 Performing side effects with `ForEach`

We can define `ForEach` on any IEnumerable:

```csharp
// Changes the Action to a Unit-returning function
// Map only create a lazily evaluated sequence of Units. 
public static IEnumerable<Unit> ForEach<T>(
    this IEnumerable<T> ts, Action<T> action) =>
        ts.Map(action.ToFunc()).ToImmutableList();
```

Usage:

```csharp
Enumerable.Range(1, 5).ForEach(Console.Write);      // prints: 12345
```

Definition of `ForEach` for `Option`:

```csharp
public static Option<Unit> ForEach<T>(
    this Option<T> opt, Action<T> action) =>
        Map(opt, action.ToFunc());
```

Usage:

```csharp
var opt = Some("John");
opt.ForEach(name => Console.WriteLine($"Hello {name}"));    // prints: Hello John
```

We should aim to separate pure logic from side effects (from chapter 2).

> ### Isolate side effects
> Make the scope of the `Action` that you apply with `ForEach` as small as possible:
> * use `Map` for data transformations.
> * use `ForEach` for side effects.

Rewrite previous usage with side effects isolation:

```csharp
opt.Map(name => $"Hello {name}")
.ForEach(WriteLine);
```

И еще пример. We can use the same patterns to work with `Option` and `IEnumerable`:

```csharp
using static System.Console;
using String = LaYumba.Functional.String;

Option<string> name = Some("Enrico");

name
    .Map(String.ToUpper)    // Using wrapper from LaYumba.Functional.String
    .ForEach(WriteLine);    // prints: ENRICO

IEnumerable<string> names = new[] { "Constance", "Albert" };

names
    .Map(String.ToUpper)
    .ForEach(WriteLine);    // prints: CONSTANCE \n ALBERT
```

**Резюме**:

`ForEach` is similar to `Map`, but it takes an `Action` rather than a function,
so it's used to perform side effects.

## 4.3 Chaining functions with Bind

`Bind` is another very important function, similar to `Map`
but slightly more complex.

### 4.3.1 Combining Option-returning function

Signature of `Bind`:

```text
(Option<T>, (T -> Option<R>)) -> Option<R>
```

Implementation of `Bind`:

```csharp
// Bind takes an Option-returning function.
public static Option<R> Bind<T, R>(
    this Option<T> optT, Func<T, Option<R>> f) =>
        optT.Match(
            () => None,
            (t) => f(t));
```

Comparing with implementation of `Map`:

```csharp
// Map takes a regular function.
public static Option<R> Map<T, R>(
    this Option<T> optT, Func<T, R> f) =>
        optT.Match(
            () => None,
            (t) => Some(f(t)));
```

Using `Bind` to compose two functions that return an `Option`:

```csharp
// From chapter 3.4.5, static class Int
public static Option<int> Parse(string s)
{
    int result;
    return int.TryParse(s, out result)
        ? Some(result)
        : None;
}

// From chapter 3.4.5, static class Age
// valid age from 0 to 120
public static Option<Age> Of(int age) =>
    IsValid(age) ? Some(new Age(age)) : None;


// Using Bind
Func<string, Option<Age>> parseAge =
    s => Int.Parse(s).Bind(Age.Of);

parseAge("26");             // => Some(26)
parseAge("notAnAge");       // => None
parseAge("180");            // => None
```

Simple program that reads an age from the console and prints out a related message:

```csharp
public static class AskForValidAgeAndPrintFlatteringMessage
{
    public static void Main() =>
        Console.WriteLine($"Only {ReadAge()}! That's young!");

    static Age ReadAge() =>
        ParseAge(Prompt("Please enter your age"))
            .Match(
                () => ReadAge(),    // <- Recursively calls itself as
                (age) => age);      // long as parsing the age fails

    // Combines parsing a string as an int and creating an age from the int
    static Option<Age> ParseAge(string s) =>
        Int.Parse(s).Bind(Age.Of);

    static string Prompt(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }
}
```

### 4.3.2 Flattening nested lists with `Bind`

The resulting list is flattened to a one-dimensional list:

```csharp
public static IEnumerable<R> Bind<T, R>(
    this IEnumerable<T> ts, Func<T, IEnumerable<R>> f)
{
    foreach (T t in ts)
        foreach (R r in f(t))
            yield return r;
}
```

Usage:

```csharp
var neighbors = new[]
{
    new { Name = "John", Pets = new Pet[] {"Fluffy", "Thor"} },
    new { Name = "Tim", Pets = new Pet[] {} },
    new { Name = "Carl", Pets = new Pet[] {"Sybil"} },
};

IEnumerable<IEnumerable<Pet>> nested = neighbors.Map(n => n.Pets);
// => [["Fluffy", "Thor"], [], ["Sybil"]]

IEnumerable<Pet> flat = neighbors.Bind(n => n.Pets);
// => ["Fluffy", "Thor", "Sybil"]
```

### 4.3.3 Actually, it's called a monad

Generalize the signature of `Bind`:

```text
(C<T>, (T -> C<R>)) -> C<R>
```

Signature of `Map`:

```text
(C<T>, (T -> R)) -> C<R>
```

Определение *monad* аналогично определению functor:

*Monads* это тип (контейнер) на котором можно выполнять операцию `Bind`
для функции.

(В конеце раздела 4.3.4 будет более полное определение *monad*).

### 4.3.4 The `Return` function

Monads must also have a `Return` function that "lifts"
a normal value `T` into a monadic value `C<T>`.

* For `Option`, this is the `Some` "lift" function (см. главу 3.4.3).

* For `IEnumerable`. Еhere are many possible ways to create an IEnumerable.
Вut to stick with functional design principles, it uses an immutable
implementation:

```csharp
using System.Collections.Immutable;

public static IEnumerable<T> List<T>(params T[] items) =>
    items.ToImmutableList();
```

Usage "lift" fuction for `IEnumerable<T>`:

```csharp
using static F;

var empty = List<string>();             // => []
var singleton = List("Andrej");         // => ["Andrej"]
var many = List("Karina", "Natasha");   // => ["Karina", "Natasha"]
```

**Резюме**:

*Monad* это такой тип `M<T>`, для которого можно определить следующие 2 функции:

```text
Return : T -> M<T>
Bind : (M<T>, (T -> C<R>)) -> C<R>
```

* `Return` должна делать минимальный объем работы, требуемой для lift
`T` в `M<T>` и ничего кроме нее.

Плюс, monad должна обладать определенными свойствами, которые будут рассмотрены
в главе 8.

### 4.3.5 Relation between functors and monads

Signatures of the core functions:

```text
Map : (C<T>, (T -> R)) -> C<R>
Bind : (C<T>, (T -> C<R>)) -> C<R>
Return : T -> C<T>
```

Из сигнатур этих методов следует:

* Every monad **is** also a functor (`Map` можно выразить через `Bind` и `Return`).

* **Not** every functor is a monad (не для каждого типа, для которого определен
`Map`, можно определить `Bind`).
