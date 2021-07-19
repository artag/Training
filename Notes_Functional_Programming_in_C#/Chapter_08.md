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
