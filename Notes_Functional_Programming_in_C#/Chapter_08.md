# Chapter 8

## Working effectively with multi-argument functions

To integrate multi-argument functions in your workflows we can use two approaches:
*applicative* and *monadic*.

## 8.1 Function application in the elevated world

From previous chapters. Apply *unary* function to number with `Map`:

```csharp
Func<int, int> multBy2 = i => i * 2;
Some(3).Map(multBy2)                    // => Some(6)
```

Apply *binary* function to two numbers with `Map`. (using currying - see chapter 7):

```csharp
Func<int, Func<int, int>> multiply = x => y => x * y;
var multBy3 = Some(3).Map(multiply);    // => Some(y => 3 * y))
```

Types:

```text
multiply              : int -> int -> int
Some(3)               : Option<int>
Some(3).Map(multiply) : Option<int -> int>
```

Here's the signature of `Map` for a functor `F`:

```text
Map : F<T> -> (T -> R) -> F<R>
```

Если `R` это функция с сигнатурой: `T1 -> T2`, то сигнатура `Map` будет выглядеть так:

```text
Map: F<T> -> (T -> T1 -> T2) -> F<T1 -> T2>
```

Здесь `T -> T1 -> T2` это binary function in curried form. This means that you can really use
`Map` with functions of any arity (любым количеством аргументов)!

Functional library includes overloads of `Map` that accept functions of various arities
and take care of currying.

Пара примеров `Map` для разного количества аргументов:

```csharp
static Option<Func<T2, R>> Map<T1, T2, R>(this Option<T1> opt, Func<T1, T2, R> func) =>
    opt.Map(func.Curry());

static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Option<T1> opt, Func<T1, T2, T3, R> func) =>
    opt.Map(func.CurryFirst());
```

Где `Curry` и `CurryFirst` выглядят так (см. главу 7.3):

```csharp
static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> func) =>
    t1 => t2 => func(t1, t2);

static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>(this Func<T1, T2, T3, R> func) =>
    t1 => (t2, t3) => func(t1, t2, t3);
```

Использование нового `Map`. Mapping a binary function onto an `Option`:

```csharp
Func<int, int, int> multiply = (x, y) => x * y;
var multBy3 = Some(3).Map(multiply);    // => Some(y => 3 * y))
```

Осталось пара вопросов:

1. How do you supply to function its second argument.
2. What if the second argument is also wrapped in an `Option`.

Грубое решение - явно "распаковать" оба значения и применить функцию:

```csharp
Func<int, int, int> multiply = (x, y) => x * y;

Option<int> optX = Some(3), optY = Some(4);

var result = optX.Map(multiply).Match(
    () => None,
    (f) => optY.Match(
        () => None,
        (y) => Some(f(y))
    )
);

result      // => Some(12)
```

Это грубое решение плохое: распаковка и обратная упаковка значений в `Option`. Решение см. далее.

### 8.1.1 Understanding applicatives (or applicative functors)

Из главы 7, сигнатуры функций `Apply` для regular values:

```text
Apply : (T -> R) -> T -> R
Apply : (T1 -> T2 -> R) -> T1 -> (T2 -> R)
Apply : (T1 -> T2 -> T3 -> R) -> T1 -> (T2 -> T3 -> R)
```

Нужно определить подобные функции `Apply` для functors:

```text
Apply : A<T -> R> -> A<T> -> A<R>
Apply : A<T1 -> T2 -> R> -> A<T1> -> A<T2 -> R>
Apply : A<T1 -> T2 -> T3 -> R> -> A<T1> -> A<T2 -> T3 -> R>
```

Такая implementation of `Apply` is defined for a functor `A`, this is called an
*applicative functor*, or an *applicative*.

Implementation of `Apply` for `Option`:

```csharp
// (1) - Only applies the wrapped function to the wrapped value if both Options are Some.
static Option<R> Apply<T, R>(this Option<Func<T, R>> optF, Option<T> optT) =>
    optF.Match(
        () => None,
        (f) => optT.Match(
            () => None,
            (t) => Some(f(t))));    // (1)

// (1) - Curries the wrapped function and uses the overload that takes an Option
//       wrapping a unary function.
public static Option<Func<T2, R>> Apply<T1, T2, R>(
    this Option<Func<T1, T2, R>> optF, Option<T1> optT) =>
        Apply(optF.Map(F.Curry), optT);     // (1)
```

Using `Apply` with a binary function:

```csharp
Func<int, int, int> multiply = (x, y) => x * y;

Some(3).Map(multiply).Apply(Some(4));    // => Some(12)
Some(3).Map(multiply).Apply(None);       // => None
```

### 8.1.2 Lifting functions

Functions "lifted" into a container by mapping a multi-argument function:

```csharp
Some(3).Map(multiply)
```

**1 способ** function application in the elevated world.

(Из предыдущего раздела 8.1.1) `Map` the function, then `Apply`:

```csharp
Some(3)
    .Map(multiply)
    .Apply(Some(4))
```

**2 способ** function application in the elevated world.

После введения `Apply` можно теперь делать так:

```csharp
Some(multiply)          // Lifts the function into an Option
    .Apply(Some(3))     // Supplies arguments with Apply
    .Apply(Some(4))     // Supplies arguments with Apply
// => Some(12)
```

Функциональность `Option` при таком вызове сохраняется:

```csharp
Some(multiply)
    .Apply(None)
    .Apply(Some(4))
// => None
```

Сравнение partial application in the worlds of regular and elevated values:

* Partial application with regular values

```csharp
multiply                // => 12
    .Apply(3)
    .Apply(4)
```

* Partial application with elevated values

```csharp
Some(multiply)          // => Some(12)
    .Apply(Some(3))
    .Apply(Some(4))
```

Whether you obtain the function by using Map or lifting it with Return doesn't matter
in terms of the resulting functor. This is a requirement, and it will hold if the applicative
is correctly implemented, so that it's sometimes called the *applicative law*.

### 8.1.3 An introduction to property-based testing

Property-based tests are parameterized unit tests whose assertions should hold for
*any* possible value of the parameters. For this tests using framework FsCheck (см. Links).

FsCheck repeatedly run the test with a large set of randomly generated parameter values.
By default, FsCheck generates 100 values, but the number and range of input values can be
customized. For the real work being able to fine-tune the parameters with which the values are
generated becomes quite important.

*Примечание*: FsCheck для `FsCheck.NUnit` был (и может быть до сих пор) реализован хуже, чем
модуль для `FsCheck.Xunit`.

Example. A property-based test illustrating the applicative law:

```csharp
Func<int, int, int> multiply = (i, j) => i * j;

// (1) - Marks a property-based test
// (2) - FsCheck will randomly generate a large set of input values to run the test with.
[Property]                                  // (1)
void ApplicativeLawHolds(int a, int b)      // (2)
{
    var first = Some(multiply)
        .Apply(Some(a))
        .Apply(Some(b));

    var second = Some(a)
        .Map(multiply)
        .Apply(Some(b));

    Assert.Equal(first, second);
}
```

Аналогично, можно сделать подобные тесты для входных параметров `Option<T>`.

Teaching FsCheck to create an arbitrary `Option`:

```csharp
static class ArbitraryOption
{
    public static Arbitrary<Option<T>> Option<T>()
    {
        var gen = from isSome in Arb.Generate<bool>()
                  from val in Arb.Generate<T>()
                  select isSome && val != null ? Some(val) : None;
        return gen.ToArbitrary();
    }
}
```

Сам тест. The property-based test, parameterized with arbitrary `Options`:

```csharp
[Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
void ApplicativeLawHolds(Option<int> a, Option<int> b) =>
    Assert.Equal(
        Some(multiply).Apply(a).Apply(b),
        a.Map(multiply).Apply(b)
    );
```

## 8.2 Functors, applicatives, monads

Summary of the core functions and how they define patterns

* **Functor** defined by an implementation of:
  1. `Map`       `F<T> -> (T -> R) -> F<R>`

* **Applicative** defined by an implementation of:
  1. `Return`    `T -> A<T>`
  2. `Apply`     `A<(T -> R)> -> A<T> -> A<R>`

* **Monad** defined by an implementation of:
  1. `Return`    `T -> M<T>`
  2. `Bind`      `M<T> -> (T -> M<R>) -> M<R>`

Relation of functor, applicative, and monad:

```text
Functor <------- Applicative <------- Monad
(Map)            (Apply)              (Bind)
<------------------------------------------>
More general                   More powerful
```

You can read this as a class diagram: if functor were an interface, applicative would
extend it. And monad extends applicative.

1. Applicative is more powerful than Functor.
For example, we can define `Map` in terms of `Return` and `Apply`:

```csharp
static Option<R> Map<T, R>(this Option<T> opt, Func<T, R> f) =>
    Some(f).Apply(opt);
```

2. Monads are more powerful than applicatives.
For example we can define `Apply` in terms of `Bind`:

```csharp
static Option<R> Apply<T, R>(this Option<Func<T, R>> optF, Option<T> optT) =>
    optT.Bind(t => optF.Bind<Func<T, R>, R>(f => f(t)));
```

3. The `fold` function (`Aggregate` in LINQ) is the most powerful of them all because you can define `Bind` in terms of it.

## 8.3 The monad laws

Remember, a *monad* is a type, `M`, for which the following functions are defined:

* `Return` - takes a regular value of type `T` and *lifts* it into a monadic value of
type `M<T>`.

* `Bind` - takes a monadic value, `m`, and a world-crossing function, `f`, and
"extracts" from `m` its inner value `t` and applies `f` to it.

`Return` and `Bind` should have the following three properties:
1. Right identity
2. Left identity
3. Associativity

### 8.3.1 Right identity

Следующее должно соблюдаться:

```csharp
m == m.Bind(Return)
```

A property-based test proving that right identity holds for the `Option` type:

```csharp
[Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
void RightIdentityHolds(Option<object> m) =>
    Assert.Equal(m, m.Bind(Some));
```

### 8.3.2 Left identity

Следующее должно соблюдаться:

```csharp
Return(t).Bind(f) == f(t)
```

A property-based test illustrating left identity for `IEnumerable`:

```csharp
Func<int, IEnumerable<int>> f = i => Range(0, i);

[Property] void LeftIdentityHolds(int t) =>
    Assert.Equal(f(t), List(t).Bind(f));
```

**Выводы**, которые являются следстивем выполения left identity и right identity:

1. Lifting operation in `Return` and unwrapping operation in `Bind` are neutral operations,
that have no side effects and don't distort the value of `t` or the behavior of `f`.

2. `Return` should be as dumb as possible: no side effects, no conditional logic, no acting
upon the given `t`. Only the minimal work required to satisfy the signature `T -> C<T>`.

### 8.3.3 Associativity (ассоциативность - наиболее значимое правило из трех)

Associativity на примере операции сложения:

```text
(a + b) + c == a + (b + c)
```

Обозначим `Bind` как `>>=`: вместо `m.Bind(f)` можно написать `m >>= f`, где `m` - a monadic value
и `f` - a world-crossing function.

 `Bind` также associative:

```text
(m >>= f) >>= g == m >>= (f >>= g)
```

Левая часть `(m >>= f) >>= g` это `m.Bind(f).Bind(g)`

В правой части `(f >>= g)` синтактически неверен: `f` - a function, not a monadic value.
`f` можно записать как `x => f(x)`, поэтому правую часть можно переписать так:

```text
m >>= (x => f(x) >>= g)
```

И все вместе, правая и левая части:

```text
(m >>= f) >>= g == m >>= (x => f(x) >>= g)
```

Или (**конечная версия**):

```csharp
m.Bind(f).Bind(g) == m.Bind(x => f(x).Bind(g))
```

Если еще расширить правую часть:

```csharp
m.Bind(f).Bind(g) == m.Bind(x => f(x).Bind(y => g(y)))
```

Here - `g` has visibility not only of `y`, but also of `x`. This is
what enables you to integrate multi-argument functions in a monadic flow.

A property-based test illustrating `Bind` associativity for `Option`:

```csharp
//Exposes an Option returning Parse function
using Double = LaYumba.Functional.Double;

Func<double, Option<double>> safeSqrt =
    d => d < 0
        ? None
        : Some(Math.Sqrt(d));

[Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
void AssociativityHolds(Option<string> m) =>
    Assert.Equal(
        m.Bind(Double.Parse).Bind(safeSqrt),
        m.Bind(x => Double.Parse(x).Bind(safeSqrt))
    );
```

### 8.3.4 Using Bind with multi-argument functions

Следствие из 8.3.3: можно записать операцию умножения, которая парсит два входных строчных
параметра и выполняет их перемножение так:

```csharp
static Option<int> MultiplicationWithBind(string strX, string strY) =>
    Int.Parse(strX)
        .Bind(x => Int.Parse(strY)
            .Bind<int, int>(y => multiply(x, y)));
```

Но, такой код плохо читается - можно воспользоваться синтаксисом LINQ (см. далее).

## 8.4 Improving readability by using LINQ with any monad

LINQ is a dedicated syntax for expressing queries:

```csharp
// Normal method invocation
Enumerable.Range(1, 100)
    .Where(i => i % 20 == 0)
    .OrderBy(i => -i)
    .Select(i => $"{i}%")

// LINQ expression
from i in Enumerable.Range(1, 100)
where i % 20 == 0 orderby -i
select $"{i}%"
```

### 8.4.1 Using LINQ with arbitrary functors

A LINQ expression with a single `from` clause and its interpretation

```csharp
from x in Range(1, 4)
select x * 2

Range(1, 4)
    .Select(x => x * 2)
```

LINQ's pattern-based approach means that you can define `Select` for any type you please.

`Select` for `Option`:

```csharp
public static Option<R> Select<T, R>(this Option<T> opt, Func<T, R> f) =>
    opt.Map(f);
```

Usage:

```csharp
from x in Some(12)
select x * 2                    // => Some(24)

from x in (Option<int>)None
select x * 2                    // => None

(from x in Some(1) select x * 2) == Some(1).Map(x => x * 2)    // => true
```

### 8.4.2 Using LINQ with arbitrary (произвольный) monads

Two data sources:

```csharp
var chars = new[] { 'a', 'b', 'c' };
var ints = new [] { 2, 3 };
```

LINQ query that combine data from multiple data sources:

```csharp
from c in chars
from i in ints
select (c, i)
// => [(a, 2), (a, 3), (b, 2), (b, 3), (c, 2), (c, 3)]
```

Equivalent expression using `Map` and `Bind`:

```csharp
chars
    .Bind(c => ints
        .Map(i => (c, i)));
```

Using the standard LINQ methods (`Select` instead of `Map` and `SelectMany` instead of `Bind`):

```csharp
chars
    .SelectMany(c => ints
        .Select(i => (c, i)));
```

Two overloads of `SelectMany` are required to implement the query pattern:

```csharp
// Plain vanilla (простой/обычный) SelectMany, equivalent to Bind
public static IEnumerable<R> SelectMany<T, R>(
    this IEnumerable<T> source, Func<T, IEnumerable<R>> func)
{
    foreach (T t in source)
        foreach (R r in func(t))
            yield return r;
}

// SelectMany that actually gets called
// Extended overload of SelectMany, which is used
// when translating a query with multiple from clauses
public static IEnumerable<RR> SelectMany<T, R, RR>(
    this IEnumerable<T> source, Func<T, IEnumerable<R>> bind, Func<T, R, RR> project)
{
    foreach (T t in source)
        foreach (R r in bind(t))
            yield return project(t, r);
}
```

Последнее выражение используется в LINQ такого вида:

```csharp
from c in chars
from i in ints
select (c, i)
```

Такой LINQ преобразуется в следующее выражение:

```csharp
chars.SelectMany(c => ints, (c, i) => (c, i))
```

We can define a reasonable implementation of the LINQ-flavored `SelectMany` for **any** monad.

`SelectMany` for Option:

```csharp
public static Option<RR> SelectMany<T, R, RR>(
    this Option<T> opt, Func<T, Option<R>> bind, Func<T, R, RR> project) =>
        opt.Match(
            () => None,
            (t) => bind(t).Match(
                () => None,
                (r) => Some(project(t, r))));
```

Usage example:

```csharp
WriteLine("Enter first addend:");
var s1 = ReadLine();
WriteLine("Enter second addend:");
var s2 = ReadLine();

var result = Calculate(s1, s2);

WriteLine(result.Match(
Some: r => $"{s1} + {s2} = {r}",
None: () => "Please enter 2 valid integers"));
```

Different ways to add two optional integers (function `Calculate`).

```csharp
// 1. using LINQ query
from a in Int.Parse(s1)
from b in Int.Parse(s2)
select a + b

// 2. normal method invocation
Int.Parse(s1)
    .Bind(a => Int.Parse(s2)
        .Map(b => a + b))

// 3. the method invocation that the LINQ query will be converted to
Int.Parse(s1)
    .SelectMany(
        a => Int.Parse(s2),
        (a, b) => a + b)

// 4. using Apply
Some(new Func<int, int, int>((a, b) => a + b))
    .Apply(Int.Parse(s1)
    .Apply(Int.Parse(s2))
```

### 8.4.3 `let`, `where`, and other LINQ clauses

The `let` clause is useful for storing the results of intermediate computations.
`let` relies on `Select`, so no extra work is needed to enable the use of `let`.

Using the `let` clause with `Option`:

```csharp
// Exposes an Option-returning Parse function
using Double = LaYumba.Functional.Double;

// Prompt is a convenience function that reads user input from the console
string s1 = Prompt("First leg:");
string s2 = Prompt("Second leg:");

// A let clause allows you to store intermediate results.
var result = from a in Double.Parse(s1)
             let aa = a * a
             from b in Double.Parse(s2)
             let bb = b * b
             select Math.Sqrt(aa + bb);

WriteLine(result.Match(
    () => "Please enter two valid numbers",
    (h) => $"The hypotenuse is {h}"));
```

`where` clause. This resolves to the `Where` method we've already defined, so no extra work is
necessary in this case.

Using the `where` clause with `Option`:

```csharp
string s1 = Prompt("First leg:")
string s2 = Prompt("Second leg:");

var result = from a in Double.Parse(s1)
             where a >= 0
             let aa = a * a
             from b in Double.Parse(s2)
             where b >= 0
             let bb = b * b
             select Math.Sqrt(aa + bb);

WriteLine(result.Match(
    () => "Please enter two valid, positive numbers",
    (h) => $"The hypotenuse is {h}"));
```

LINQ also contains various other clauses, such as `orderby`. These clauses make sense for
collections but have no counterpart in structures like `Option` and `Either`.

**Summary**.
* For any monad you can implement the LINQ query pattern by providing implementations for:
  1. `Select` (`Map`)
  2. `SelectMany` (`Bind`)
  3. The ternary overload to `SelectMany`.
* Some structures may have other operations that can be included in the query pattern, such as
`Where` in the case of `Option`.

## 8.5 When to use `Bind` vs. `Apply`

### 8.5.1 Validation with smart constructors

Example of using smart constructor (см. главу 3 для более подробной информации):

```csharp
public class PhoneNumber
{
    public static Func<NumberType, CountryCode, Number, PhoneNumber> Create =
        (type, country, number) =>
            new PhoneNumber(type, country, number);

    PhoneNumber(NumberType type, CountryCode country, Number number)
    {
        Type = type;
        Country = country;
        Nr = number;
    }
}
```

Parameters `NumberType`, `CountryCode` and `Number` can be validated independently
(has functions with the next signatures):

```text
validCountryCode : string -> Validation<CountryCode>
validNnumberType : string -> Validation<PhoneNumber.NumberType>
validNumber: string -> Validation<PhoneNumber.Number>
```

### 8.5.2 Harvesting errors with the applicative flow

```csharp
// (1) Lifts the factory function into a Validation
// (2) Supplies arguments, each of which is also wrapped in a Validation
Validation<PhoneNumber> CreatePhoneNumber(string type, string countryCode, string number) =>
    Valid(PhoneNumber.Create)                   // (1)
        .Apply(validNumberType(type))           // (2)
        .Apply(validCountryCode(countryCode))   // (2)
        .Apply(validNumber(number));            // (2)
```

Let's see its behavior, given a variety of different inputs:

```csharp
CreatePhoneNumber("Mobile", "ch", "123456")
// => Valid(Mobile: (ch) 123456)

CreatePhoneNumber("Mobile", "xx", "123456")
// => Invalid([xx is not a valid country code])

CreatePhoneNumber("Mobile", "xx", "1")
// => Invalid([xx is not a valid country code, 1 is not a valid number])
```

Implementation of `Apply` for `Validation`:

```csharp
// (1) If both inputs are valid, the wrapped function is applied to the
//     wrapped argument, and the result is lifted into a Validation in the Valid state.
// (2) If both inputs have errors, a Validation in the Invalid state
//     is returned that collects the errors from both valF and valT.
public static Validation<R> Apply<T, R>(
    this Validation<Func<T, R>> valF, Validation<T> valT) =>
        valF.Match(
            Valid: (f) => valT.Match(
                Valid: (t) => Valid(f(t)),                          // (1)
                Invalid: (err) => Invalid(err)),
            Invalid: (errF) => valT.Match(
                Valid: (_) => Invalid(errF),
                Invalid: (errT) => Invalid(errF.Concat(errT))));    // (2)
```

### 8.5.3 Failing fast with the monadic flow

Validation using a monadic flow (using LINQ):

```csharp
Validation<PhoneNumber> CreatePhoneNumberM(
    string typeStr, string countryStr, string numberStr) =>
        from type in validNumberType(typeStr)
        from country in validCountryCode(countryStr)
        from number in validNumber(numberStr)
        select PhoneNumber.Create(type, country, number);
```

Run this new version with the same test values as before:

```csharp
CreatePhoneNumberM("Mobile", "ch", "123456")
// => Valid(Mobile: (ch) 123456)

CreatePhoneNumberM("Mobile", "xx", "123456")
// => Invalid([xx is not a valid country code])

CreatePhoneNumberM("Mobile", "xx", "1")
// => Invalid([xx is not a valid country code])     // <-- Different result!
```

Implementation of `Bind` for `Validation`:

```csharp
public static Validation<R> Bind<T, R>(
    this Validation<T> val, Func<T, Validation<R>> f) =>
        val.Match(
            Invalid: (err) => Invalid(err),
            Valid: (r) => f(r));
```

Compare the signatures of `Apply` and `Bind`:

```text
Apply : Validation<(T -> R)> -> Validation<T>        -> Validation<R>
Bind  : Validation<T>        -> (T -> Validation<R>) -> Validation<R>
```

**Summary** three ways to use multi-argument functions in the elevated world:
1. The ugly way - Nested calls to `Bind` (лучше всего избегать).
2. `Apply` method
3. Monadic flow with LINQ.

## Summary

* The `Apply` function can be used to perform function application in an elevated
world, such as the world of `Option`.
* Multi-argument functions can be lifted into an elevated world with `Return`, and
then arguments can be supplied with `Apply`.
* Types for which `Apply` can be defined are called *applicatives*. Applicatives are
more powerful than functors, but less powerful than monads.
* Because monads are more powerful, you can also use nested (вложенные) calls to `Bind` to
perform function application in an elevated world.
* LINQ provides a lightweight syntax for working with monads that reads better
than nesting calls to `Bind`.
* To use LINQ with a custom type, you must implement the LINQ query pattern,
particularly providing implementations of `Select` and `SelectMany` with appropriate
signatures.
* For several monads, `Bind` has short-circuiting behavior (the given function won't
be applied in some cases), but `Apply` doesn't (it's not given a function, but rather
an elevated value). For this reason, you can sometimes embed desirable behavior
into applicatives, such as collecting validation errors in the case of `Validation`.
* FsCheck is a framework for property-based testing. It allows you to run a test
with a large number of randomly generated inputs, giving high confidence that
the test's assertions hold for any input.
