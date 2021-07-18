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
