# Chapter 6

## Functional error handling

Функциональный и императивный стили програмиирования очень сильно отличаются в следующем:

* Imperative programming uses special statements like `throw` and `try/catch`,
which disrupt (нарушает) the normal program flow, thus introducing side effects (см. главу 2).

* Functional programming strives (стремится) to minimize side effects, so throwing excep-
tions is **generally avoided**. Instead, if an operation can fail, it should return a
representation of its outcome (резльтат) including an indication of success or failure, as
well as its result (if successful), or some error data otherwise. In other words,
errors in FP are just *payload* (полезный бонус).

Недостатки императивного стиля:
* `throw` has similar semantics to `goto` - (даже так: `throw` is much worse than `goto`).
* Много путаницы когда использовать исключения и когда использовать другие
error-handling technique.

## 6.1 A safer way to represent outcomes

В главе 3 был представлен `Option` (`Some` - все OK, `None` - что-то пошло не так).
`Option` можно использовать для обработки ошибок. Примеры:

* Parsing a number - (`None` - parsing failed).
* Retrieving an item from a collection - (`None` indicate that no suitable item was found).

Но `Option` не содержит информации о том, что пошло не так. Поэтому для более сложных сценариев
надо использовать что-то другое.

### 6.1.1 Capturing error details with `Either`

`Either` type - operation with two possible outcomes: `Left` and `Right`:

* `Left` = "something wrong"
* `Right` = "all right"

`Option` and `Either` can both represent possible failure:

|-               | Failure   | Success    |
|----------------|-----------|------------|
| `Option<T>`    | `None`    | `Some(T)`  |
| `Either<L, R>` | `Left(L)` | `Right(R)` |

Option can be symbolically defined as:

```text
Option<T> = None | Some(T)
```

Either can similarly be defined like this:

```text
Either<L, R> = Left(L) | Right(R)
```

Notice that `Either` has two generic parameters and can be in one of two states:
* `Left(L)` wraps a value of type `L`, capturing details about the error.
* `Right(R)` wraps a value of type `R`, representing a successful result.

`Either`, `Left` and `Right` implementation:

```csharp
public static partial class F
{
    public static Either.Left<L> Left<L>(L l) => new Either.Left<L>(l);
    public static Either.Right<R> Right<R>(R r) => new Either.Right<R>(r);
}

public struct Either<L, R>
{
    internal L Left { get; }
    internal R Right { get; }

    private bool IsRight { get; }
    private bool IsLeft => !IsRight;

    internal Either(L left)
    {
       IsRight = false;
       Left = left;
       Right = default(R);
    }

    internal Either(R right)
    {
       IsRight = true;
       Right = right;
       Left = default(L);
    }

    public static implicit operator Either<L, R>(L left) =>
        new Either<L, R>(left);
    public static implicit operator Either<L, R>(R right) =>
        new Either<L, R>(right);

    public static implicit operator Either<L, R>(Either.Left<L> left) =>
        new Either<L, R>(left.Value);
    public static implicit operator Either<L, R>(Either.Right<R> right) =>
        new Either<L, R>(right.Value);

    public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right) =>
        IsLeft ? Left(this.Left) : Right(this.Right);

    public Unit Match(Action<L> Left, Action<R> Right) =>
        Match(Left.ToFunc(), Right.ToFunc());

    public IEnumerator<R> AsEnumerable()
    {
       if (IsRight) yield return Right;
    }

    public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
}

public static class Either
{
    public struct Left<L>
    {
        internal L Value { get; }
        internal Left(L value) { Value = value; }

        public override string ToString() => $"Left({Value})";
    }

    public struct Right<R>
    {
        internal R Value { get; }
        internal Right(R value) { Value = value; }

        public override string ToString() => $"Right({Value})";

        public Right<RR> Map<L, RR>(Func<R, RR> f) => Right(f(Value));
        public Either<L, RR> Bind<L, RR>(Func<R, Either<L, RR>> f) => f(Value);
    }
}
```

Примеры использования `Either`:

```csharp
// Creates an Either in the Right state
Right(12)           // => Right(12)

// Creates an Either in the Left state
Left("oops")        // => Left("oops")
```

```csharp
// Function that uses Match
string Render(Either<string, double> val) =>
    val.Match(
        Left: l => $"Invalid value: {l}",
        Right: r => $"The result is: {r}");

Render(Right(12d))      // => "The result is: 12"
Render(Left("oops"))    // => "Invalid value: oops"
```

Пример использования `Either` с реализацией взятия квадратного корня:

```csharp
using static System.Math;

Either<string, double> Calc(double x, double y)
{
    if (y == 0)
        return "y cannot be 0";

    if (x != 0 && Sign(x) != Sign(y))
        return "x / y cannot be negative";

    return Sqrt(x / y);
}
```

Test it out:

```csharp
Calc(3, 0)      // => Left("y cannot be 0")
Calc(-3, 3)     // => Left("x / y cannot be negative")
Calc(-3, -3)    // => Right(1)
```

### 6.1.2 Core functions for working with `Either`

We can define `Map`, `ForEach`, and `Bind` in terms of `Match`:

```csharp
public static Either<L, RR> Map<L, R, RR>(this Either<L, R> either, Func<R, RR> f) =>
    either.Match<Either<L, RR>>(
        l => Left(l),
        r => Right(f(r)));

public static Either<L, Unit> ForEach<L, R>(this Either<L, R> either, Action<R> act) =>
    Map(either, act.ToFunc());

public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> either, Func<R, Either<L, RR>> f) =>
    either.Match(
        l => Left(l),
        r => f(r));
```

Замечания:

* Function is applied *only* if the `Either` is `Right`.
* When you use `Map` and `Bind`, the `R` type changes.
The `L` type, on the other hand, remains the same.

Определить `Where` для `Either` невозможно (predicate не может возвратить значение `L`):

```csharp
public static Either<L, R> Where<L, R>(this Either<L, R> either, Func<R, bool> predicate) =>
    either.Match(
        l => either,
        r => predicate(r)
            : either
            ? /* now what? I don't have an L */ );
```

`Where` is less general (менее общий) than `Map` and `Bind`: it can only be
defined for structures for which a *zero* value exists.

Для `IEnumerable` это empty sequence, для `Option` это `None`.

Для `Either<L, R>` нет *zero* value, because `L` is an arbitrary type (произвольный тип).
You can only cause an `Either` to fail by explicitly creating a `Left`, or
by calling `Bind` with a function that may return a suitable `L` value.

### 6.1.3 Comparing `Option` and `Either`

Example. An `Option`-based implementation modeling the recruitment process:

```csharp
Func<Candidate, bool> IsEligible;               // имеет право работать
Func<Candidate, Option<Candidate>> TechTest;    // Some(Candidate) - has passed
Func<Candidate, Option<Candidate>> Interview;   // None - rejection (отказ)

Option<Candidate> Recruit(Candidate c) =>
    Some(c)
        .Where(IsEligible)
        .Bind(TechTest)
        .Bind(Interview)
```

Example. An equivalent `Either`-based implementation:

```csharp
// Rejection - capturing reasons for rejection
Func<Candidate, bool> IsEligible;
Func<Candidate, Either<Rejection, Candidate>> TechTest;
Func<Candidate, Either<Rejection, Candidate>> Interview;

// Turn the predicate into an Either-returning function.
Either<Rejection, Candidate> CheckEligibility(Candidate c)
{
    if (IsEligible(c))
        return c;
    else return new Rejection("Not eligible")
}

Either<Rejection, Candidate> Recruit(Candidate c) =>
    Right(c)
        .Bind(CheckEligibility)
        .Bind(TechTest)
        .Bind(Interview);
```

We choose `Either` when we need to be explicit (когда нам надо более явно знать)
about failure conditions.

## 6.2 Chaining operations that may fail

`Either` lends itself particularly well (особенно хорошо подходит) to representing
a chain of operations where any operation may cause a deviation from the happy path.

Только если все операции успешно закончатся возвращается успешный результат.

Example. Using `Bind` to chain several `Either`-returning functions:

```csharp
Func<Either<Reason, Unit>> WakeUpEarly;                         // Встал рано
Func<Unit, Either<Reason, Ingredients>> ShopForIngredients;     // Закупил продукты
Func<Ingredients, Either<Reason, Food>> CookRecipe;             // Приготовил по рецепту

Action<Food> EnjoyTogether;             // Наслаждаемся вместе (приготовленным ужином)
Action<Reason> ComplainAbout;           // Жалоба на что-то (причина неудачи)
Action OrderPizza;                      // Заказал пиццу

void Start()    // Попытка приготовить ужин
{
    WakeUpEarly()
        .Bind(ShopForIngredients)
        .Bind(CookRecipe)
        .Match(
            Right: dish => EnjoyTogether(dish),  // Все хорошо - наслаждаемся ужином
            Left: reason =>                      // Что-то пошло не так
            {
                ComplainAbout(reason);           // Причина неудачи (информирование кого-либо)
                OrderPizza();                    // Заказ пиццы (ведь есть что-то надо)
            });
}
```

`Left` value just gets passed along (просто передается далее).

A workflow obtained by chaining several `Either`-returning functions can be seen as
a two-track system ("Railway Oriented Programming" - см. ссылку в линках):

* There's a *main track* (the happy path), going from `R1` to `Rn`.

* There's an auxiliary (дополнительный), *parallel* track, on the `Left` side.

* Once you're on the `Left` track, you stay on it until the end of the road.

* If you're on the `Right` track, with each function application, you will either proceed
along (продолжить так же) the `Right` track, or be diverted (направлены) to the `Left` track.

* `Match` is the end of the road, where the disjunction (расхождение/размыкание/разрыв)
of the parallel tracks takes place.

Еще пример. Stateless server (который также придерживается "рельсовой" последовательности):

1. Validate the request.
2. Load the model from the DB.
3. Make changes to the model.
4. Persist changes.
