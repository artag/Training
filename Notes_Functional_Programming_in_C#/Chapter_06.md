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

`Either`, `Left` and `Right` implementation watch [here](Either.md).

```csharp

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

## 6.3 Validation: a perfect use case for `Either`

### 6.3.1 Choosing a suitable representation for errors

A base class for representing failure:

```csharp
namespace LaYumba.Functional
{
    public class Error
    {
        public virtual string Message { get; }
    }
}
```

**Recommended approach** is to *create one subclass for each error type*.

Example. Distinct types capture details about specific errors:

```csharp
namespace Boc.Domain
{
    public sealed class InvalidBic : Error
    {
        public override string Message { get; } =
            "The beneficiary's BIC/SWIFT code is invalid";
    }

    public sealed class TransferDateIsPast : Error
    {
        public override string Message { get; } =
            "Transfer date cannot be in the past";
    }
}
```

Add a static class, `Errors`, that contains factory functions for creating specific
subclasses of `Error`:

```csharp
public static class Errors
{
    public static InvalidBic InvalidBic =>
        new InvalidBic();

    public static TransferDateIsPast TransferDateIsPast =>
        new TransferDateIsPast();
}
```

Преимущества такого описания ошибок:
* Более чистый код в бизнес-части кода.
* Хорошая документация - дает обзор всех конкретных ошибок, определенных для домена.

### 6.3.2 Defining an `Either`-based API

`BookTransfer` - data-transfer object. Receive from the client, and it's the input
data for our workflow.

Workflow should return an `Either<Error, Unit>`.

We need to implement function with signature:

```text
BookTransfer -> Either<Error, Unit>
```

```csharp
public class BookTransferController : Controller
{
    // Defines the high-level workflow: first validate, then persist.
    // (1) Uses Bind to chain two operations that may fail
    Either<Error, Unit> Handle(BookTransfer cmd) =>
        Validate(cmd)
            .Bind(Save);    // (1)

    // Uses Either to acknowledge that validation may fail
    Either<Error, BookTransfer> Validate(BookTransfer cmd) =>
        // TODO: add validation...

    // Uses Either to acknowledge that persisting the request may fail
    Either<Error, Unit> Save(BookTransfer cmd) =>
        // TODO: save the request...
}
```

### 6.3.3 Adding validation logic

Let's validate simple conditions about the request:
* That the date for the transfer is indeed (действительно) in the future
* That the provided BIC code is in the right format 

```csharp
Regex bicRegex = new Regex("[A-Z]{11}");

Either<Error, BookTransfer> ValidateBic(BookTransfer cmd)
{
    // Failure: the error will be wrapped in an Either in the Left state.
    if (!bicRegex.IsMatch(cmd.Bic))
        return Errors.InvalidBic;
    
    // Success: the original request will be wrapped in an Either in the Right state.
    return cmd;
}
```

Chaining several validation functions with `Bind`:

```csharp
public class BookTransferController : Controller
{
    DateTime now;
    Regex bicRegex = new Regex("[A-Z]{11}");
    Either<Error, Unit> Handle(BookTransfer cmd) =>
        Right(cmd)
            .Bind(ValidateBic)
            .Bind(ValidateDate)
            .Bind(Save);

    Either<Error, BookTransfer> ValidateBic(BookTransfer cmd)
    {
        if (!bicRegex.IsMatch(cmd.Bic))
            return Errors.InvalidBic;
        return cmd;
    }

    Either<Error, BookTransfer> ValidateDate(BookTransfer cmd)
    {
        if (cmd.Date.Date <= now.Date)
            return Errors.TransferDateIsPast;
        return cmd;
    }

    Either<Error, Unit> Save(BookTransfer cmd) => //...
}
```

**Summary**: use `Either` to acknowledge that an operation may fail and `Bind` to chain
several operations that may fail.

But if the application internally uses `Either` (or `Option`) to represent outcomes (результаты),
how should it represent outcomes to client applications that communicate with it over
some protocol such as HTTP?

We'll need to define a translation when communicating with other applications.

## 6.4 Representing outcomes to client applications

For `Option` and `Either` uses `Map`, `Bind`, and `Where`. Эти три метода работают в
*within the abstraction* (область Elevated values).

`Match` позволяет перейти из области Elevated values в область Regular values - 
*leave the abstraction*.

As a **general rule**, once you've introduced an abstraction like `Option`, it's best to stick
with it as long as possible. What does "as long as possible" mean? Ideally, it means that
**you'll leave the abstract world when you cross application boundaries**.

Abstractions such as `Option` and `Eithe`r are useful within the application core, but
they may not translate well to the message contract expected by the interacting applications.
Thus, the outer layer is where you need to leave the abstraction and translate
to the representation expected by your client applications.

### 6.4.1 Exposing an `Option`-like interface

Example. Interface for "ticker" (financial instrument, such as MSFT),
returns details about the requested financial instrument:

```csharp
public interface IInstrumentService
{
    Option<InstrumentDetails> GetInstrumentDetails(string ticker);
}
```

Next, let’s expose this data to the outer world - use `Controller`.

The controller effectively acts as an adapter between the application core
and the clients consuming the API.

The API returns, let's say, JSON over HTTP - a format and protocol that doesn't
deal in `Option`s.

Controller is the last point where we can "translate" our `Option` into something that's
supported by that protocol.

Example of Controller. Translating None to status code 404:

```csharp
using Microsoft.AspNet.Mvc;

public class InstrumentsController : Controller
{
    Func<string, Option<InstrumentDetails>> getInstrumentDetails;

    [HttpGet, Route("api/instruments/{ticker}/details")]
    public IActionResult GetInstrumentDetails(string ticker) =>
        getInstrumentDetails(ticker)
            .Match<IActionResult>(
                () => NotFound(),           // None is mapped to a 404.
                (result) => Ok(result));    // Some is mapped to a 200.
}
```

### 6.4.2 Exposing an `Either`-like interface

Example of Controller (using `Either`). Translating Left to status code 400:

```csharp
public class BookTransferController : Controller
{
    private IHandler<BookTransfer> transfers;

    [HttpPost, Route("api/transfers/future")]
    public IActionResult BookTransfer([FromBody] BookTransfer request) =>
        transfers.Handle(request)
            .Match<IActionResult>(
                Left: BadRequest,       // None is mapped to a 400.
                Right: _ => Ok());

    Either<Error, Unit> Handle(BookTransfer cmd) //...
}
```

### 6.4.3 Returning a result DTO

This approach involves:
* always returning a successful status code (the response was correctly received and processed).
* with an arbitrarily rich representation of the outcome in the response body.

Example. A DTO representing the outcome, to be serialized in the response:

```csharp
public class ResultDto<T>
{
    public bool Succeeded { get; }
    public bool Failed => !Succeeded;

    public T Data { get; }
    public Error Error { get; }

    public ResultDto(T data) { Succeeded = true; Data = data; }
    public ResultDto(Error error) { Error = error; }
}
```

We can then define a utility function that translates an Either to a ResultDto:

```csharp
public static ResultDto<T> ToResult<T>(this Either<Error, T> either) =>
    either.Match(
        Left: error => new ResultDto<T>(error),
        Right: data => new ResultDto<T>(data))
```

Now we can just expose (отобразить) the `Result` in our API method, as follows:

```csharp
public class BookTransferController : Controller
{
    [HttpPost, Route("api/transfers/book")]
    public ResultDto<Unit> BookTransfer([FromBody] BookTransfer cmd) =>
        Handle(cmd).ToResult();

    Either<Error, Unit> Handle(BookTransfer cmd) //...
}
```

**Summary**: use `Match` if you're in the skin of the orange;
stay with the juicy abstractions within the core of the orange.

## 6.5 Variations on the `Either` theme

Использование `Either` конечно полезно, но:
* The `Left` type always stays the same, so how can you compose functions that
return an `Either` with a different `Left` type?
* Always having to specify two generic arguments makes the code too verbose (слишком многословным).
* The names `Either`, `Left`, and `Right` are too cryptic. Can't we have something
more user-friendly?

Ответ - использование вариаций типа `Either` (см. далее).

### 6.5.1 Changing between different error representations

Можно определить тип `Map`, который будет для `Either` выполнять преобразование как
для `Right`, так и для `Left`.

В ФП functors такого типа называются *bifunctors*, а методы обычно называются `BiMap`.

```csharp
public static Either<LL, RR> Map<L, LL, R, RR>(
    this Either<L, R> either, Func<L, LL> left, Func<R, RR> right) =>
        either.Match<Either<LL, RR>>(
            l => Left(left(l)),
            r => Right(right(r)));
```

Пример использования bifunctor'а для `Left` значения:

```csharp
Either<Error, int> Run(double x, double y) =>
    Calc(x, y)
        .Map(
            left: msg => Error(msg),    // Преобразование string в Error
            right: d => d)              // Без изменений
        .Bind(ToIntIfWhole);

Either<string, double> Calc(double x, double y) => //...
Either<Error, int> ToIntIfWhole(double d) => //...
```

### 6.5.2 Specialized versions of `Either`

Two shortcomings (недостатка) of using `Either` in C#:

1. Having two generic arguments adds noise to the code
(пример - `Either<IEnumerable<Error>, Rates>`).

2. Names `Either`, `Left`, and `Right` are too abstract.

Решение - использование специализированных версий `Either` (плюс, можно сделать свои версии):

* `Validation<T>` - тип подобный `Either`, с ошибками `IEnumerable<Error>`.

  ```text
  Validation<T> = Invalid(IEnumerable<Error>) | Valid(T)
  ```

  Can capture multiple validation errors.

  `Validation` implementation watch [here](Validation.md).

* `Exceptional<T>` - тип подобный `Either`, с ошибками `System.Exception`.

  ```text
  Exceptional<T> = Exception | Success(T)
  ```

  Can be used as a bridge between an exception-based API and functional error handling.

  `Exceptional` implementation watch [here](Exceptional.md).

Table. Some particularized versions of Either and their state names

| Type             | Success case | Failure case | Failure type
|------------------|--------------|--------------|-------------------
| `Either<L, R>`   | `Right`      | `Left`       | `L`
| `Validation<T>`  | `Valid`      | `Invalid`    | `IEnumerable<Error>`
| `Exceptional<T>` | `Success`    | `Exception`  | `Exception`

### 6.5.3 Refactoring to Validation and Exceptional

Refactor previous example with `Exceptional` to `Validation`:

```csharp
DateTime now;

// (1) Wraps an Error in a Validation in the Invalid state.
// (2) Wraps the command in a Validation in the Valid state.
Validation<BookTransfer> ValidateDate(BookTransfer cmd)
{
    if (cmd.Date.Date <= now.Date)
        return Invalid(Errors.TransferDateIsPast);    // (1)

    return Valid(cmd);    // (2)
}
```

### Bridging between an exception-based API and functional error handling

Failure here would indicate a fault in the infrastructure or configuration,
or another technical error.

Translating an Exception -based API to an Exceptional value:

```csharp
string connString;

// (1) The return type acknowledges the possibility of an exception.
// (2) The call to a third-party API that throws an exception is wrapped in a try.
// (3) The exception is wrapped in an Exceptional in the Exception state.
// (4) The resulting Unit is wrapped in an Exceptional in the Success state.
Exceptional<Unit> Save(BookTransfer transfer)    // (1)
{
    try
    {
        ConnectionHelper.Connect(connString, c => c.Execute("INSERT ...", transfer));    // (2)
    }
    catch (Exception ex) { return ex; }     // (3)
    return Unit();                          // (4)
}
```

Примечание: scope of the `try/catch` is as *small as possible*.

### Failed validation and technical errors should be handled differently

Плюсы использования `Validation` и `Exceptional` типов - они имеют четкую семантику (смысл):
* `Validation` indicates that some business rule has been violated.
* `Exception` denotes (обозначает) an unexpected technical error.

Разные типы можно объединять. Пример:

```csharp
public class BookTransferController : Controller
{
    // Combines validation and persistence
    Validation<Exceptional<Unit>> Handle(BookTransfer cmd) =>
        Validate(cmd)
            .Map(Save);

    // Top-level validation function combining various validations
    Validation<BookTransfer> Validate(BookTransfer cmd) =>
        ValidateBic(cmd)
            .Bind(ValidateDate);

    Validation<BookTransfer> ValidateBic(BookTransfer cmd) => // ...
    Validation<BookTransfer> ValidateDate(BookTransfer cmd) => // ...

    Exceptional<Unit> Save(BookTransfer cmd) => // ...
}
```

Результирующий тип у функции `Handle` - `Validation<Exceptional<Unit>>`.

`Handle` подтверждает, что операция может закончится неудачей как по причине бизнес логики,
так и по технической причине путем "stacking" the two *monadic effects*.

Теперь контроллер, который используется как точка входа/выхода из внешнего мира:

```csharp
public class BookTransferController : Controller
{
    ILogger<BookTransferController> logger;

    // (1) Unwraps the value inside the Validation
    // (2) If validation failed, sends a 400 (400 - Bad Request)
    // (3) Unwraps the value inside the Exceptional
    // (4) If persistence failed, sends a 500 (500 - Internal Server Error)
    [HttpPost, Route("api/transfers/book")]
    public IActionResult BookTransfer([FromBody] BookTransfer cmd) =>
        Handle(cmd).Match(                      // (1)
            Invalid: BadRequest,                // (2)
            Valid: result => result.Match(      // (3)
                Exception: OnFaulted,           // (4)
                Success: _ => Ok()));

    IActionResult OnFaulted(Exception ex)
    {
        logger.LogError(ex.Message);
        return StatusCode(500, Errors.UnexpectedError);
    }

    Validation<Exceptional<Unit>> Handle(BookTransfer cmd) => //...
}
```

Here we use two nested calls to `Match` to first unwrap the value inside the `Validation`,
and then the value inside the `Exceptional`:

* If validation failed, we send a 400, which will include the full details of the validation
errors, so that the user can address them.

* If persistence failed, on the other hand, we don't want to send the details to the
user. Instead we return a 500 with a more generic error type; this is also a good
place to log the exception.

**Summary**:

1. `Either` gives you an explicit, functional way to handle errors without introducing side effects.
2. Using specialized versions of `Either`, such as `Validation` and `Exceptional`,
leads to an even more expressive and readable implementation.

### 6.5.4 Leaving exceptions behind?

Использование exceptions приводит к:
1. Disrupts (нарушение) normal program flow, introducing side effects.
2. It makes your code more difficult to maintain.

Исключения все еще могут применяться в следующих случаях:
* *Developer errors* - попытка удаления элемента из пустого списка, передача null-значения в функцию.
Такие исключения не перехватываются, они означают ошибку логики приложения.

* *Configuration errors* - например, работа приложения полностью зависит от message bus или от
соединения с БД. Исключения при ошибках/отсутствиях такого рода настроек должны 
выбрасываться при инициализации приложения и не должны перехватываться. Они должны аварийно
завершать работу программы.

## Summary

* Use `Either` to represent the result of an operation with two different possible
outcomes (результаты), typically success or failure. An `Either` can be in one of two states:
  * `Left` indicates failure and contains error information for an unsuccessful operation.
  * `Right` indicates success and contains the result of a successful operation.
* Interact with `Either` using the equivalents of the core functions already seen
with `Option` :
  * `Map` and `Bind` apply the mapped/bound function *if* the `Either` is in the
  `Right` state; otherwise they just pass along the `Left` value.
  * `Match` works similarly to how it does with `Option`, allowing you to handle the
  `Right` and `Left` cases differently.
  * `Where` is not readily applicable, so `Bind` should be used in its stead (вместо него)
  for filtering, while providing (обеспечивая) a suitable (подходящее) `Left` value.
* `Either` is particularly useful (особено полезен) for combining several validation
functions with `Bind`, or, more generally, for combining several operations, each of which can
fail.
* Because `Either` is rather abstract, and because of the syntactic overhead of its
two generic arguments, in practice it’s better to use a particularized version of
`Either`, such as `Validation` and `Exceptional`.
* When working with functors and monads, prefer using functions that stay
within the abstraction, like `Map` and `Bind`. Use the downward-crossing `Match`
function as little or as late as possible.
