# Chapter 5

## Designing programs with function composition

## 5.1 Function composition

### 5.1.1 Brushing up on function composition

Function composition in math:

```text
h = f * g
h(x) = (f * g)(x) = f(g(x))
```

Example:

```csharp
static string AbbreviateName(Person p) =>
    Abbreviate(p.FirstName) + Abbreviate(p.LastName);

static string Abbreviate(string s) =>
    s.Substring(0, 2).ToLower();

static string AppendDomain(string localPart) =>
    $"{localPart}@manning.com";
```

Defining a function as the composition of two existing functions:

```csharp
// emailFor is the composition of AppendDomain with AbbreviateName.
Func<Person, string> emailFor =
    p => AppendDomain(AbbreviateName(p));
```

Usage:

```csharp
var joe = new Person("Joe", "Bloggs");
var email = emailFor(joe);
// => jobl@manning.com
```

Важно отметить:

1. Можно compose только функции с совпадающими типами: если composing `(f * g)`, то
выход `g` должен быть assignable для входного типа `f`.

2. В композиции функций, функции идут в обратном порядке в котором они выполняются
(справа налево).

### 5.1.2 Method chaining

Предыдущий пример можно модифицировать следующим образом:

```csharp
// Add the this keyword to make it an extension method.
static string AbbreviateName(this Person p) =>
    Abbreviate(p.FirstName) + Abbreviate(p.LastName);

static string AppendDomain(this string localPart) =>
    $"{localPart}@manning.com";
```

Using method chaining syntax to compose functions:

```csharp
var joe = new Person("Joe", "Bloggs");
var email = joe.AbbreviateName().AppendDomain();
// => jobl@manning.com
```

The extension methods appear in the order in which they will be executed.
This significantly improves readability. 

Method chaining is the preferable way of achieving function
composition in C#.

### 5.1.3 Composition in the elevated world

```csharp
Func<Person, string> emailFor =
    p => AppendDomain(AbbreviateName(p));

var opt = Some(new Person("Joe", "Bloggs"));    // Option<Person>

// Maps the composed functions
var a = opt.Map(emailFor)       // opt.Map(p => emailFor(p))

// Maps AbbreviateName and AppendDomain in separate steps
var b = opt.Map(AbbreviateName)
           .Map(AppendDomain);

Assert.AreEqual(a, b);
```

>#### Не совсем понятно
>More generally, if `h = f * g`, then mapping `h` onto a functor should be equivalent to
>mapping `g` over that functor and then mapping `f` over the result. This should hold for
>any functor, and for any pair of functions — it's one of the *functor laws*, so any
>implementation of `Map` should observe (соблюдать) it.

`Map` должен применять функцию к внутренним значениям functor и ничего больше не делать,
чтобы композиция функций сохранялась при работе с функторами так же, как и с обычными значениями.

Прелесть этого в том, что вы можете использовать любую функциональную библиотеку
на любом языке программирования и использовать любой функтор с уверенностью, что рефакторинг,
такой как изменение для `a` и `b` в предыдущем примере, будет безопасным.

## 5.2 Thinking in terms of data flow

### 5.2.1 Using LINQ’s composable API

Example. Find the average earnings of the richest quartile (the richest 25% of people):

```csharp
static decimal AverageEarningsOfRichestQuartile(List<Person> population) =>
    population
        .OrderByDescending(p => p.Earnings)
        .Take(population.Count / 4)
        .Select(p => p.Earnings)
        .Average();
```

Этот пример представляет собой a linear sequence of instructions.

* First, sort the population (richest at the top).
* Then, only take the top 25%.
* Then, take each person's earnings and average them.

The input data (`List<Person>`) "flows" through four transformative steps and is thus
stepwise transformed into the output value (decimal).

### 5.2.2 Writing functions that compose well

Признаки для функций, что они будут наиболее composable:

* *Pure* - If your function has side effects, it's less reusable.

* *Chainable* - A `this` argument (implicit on instance methods and explicit on
extension methods) makes it possible to compose through chaining.

* *General* - The more specific the function, the fewer cases there will be where it's
useful to compose it.

* *Shape-preserving* - The function preserves (сохраняет) the "shape" of the structure;
so, if it takes an `IEnumerable`, it returns an `IEnumerable`, and so on.

* Functions are more composable than actions (because an `Action` has no output value,
it's a dead end).

Рассмотрим предыдущий пример: `AverageEarningsOfRichestQuartile` composable на 40%:

1) it's pure and has an output value.

2) But. It's not an extension method and it's extremely specific - it's not chainable,
hardly hope to reuse it.

Сделаем **рефакторинг** для этого метода (повтор еще раз):

```csharp
static decimal AverageEarningsOfRichestQuartile(List<Person> population) =>
    population
        .OrderByDescending(p => p.Earnings)
        .Take(population.Count / 4)
        .Select(p => p.Earnings)
        .Average();

// Тест для него
[TestCase(ExpectedResult = 75000)]
public decimal AverageEarningsOfRichestQuartile()
{
    var population = Range(1, 8)
        .Select(i => new Person { Earnings = i * 10000 })
        .ToList();
    return PopulationStatistics
        .AverageEarningsOfRichestQuartile(population);
}
```

Разделим этот метод на two more general functions:

```csharp
public static IEnumerable<Person> RichestQuartile(this List<Person> pop) =>
    pop.OrderByDescending(p => p.Earnings)
       .Take(pop.Count / 4);

public static decimal AverageEarnings(this IEnumerable<Person> population) =>
    population.Average(p => p.Earnings);
```

Тест можно переписать так:

```csharp
List<Person> SamplePopulation =>
    Range(1, 8)
        .Select(i => new Person { Earnings = i * 10000 })
        .ToList();

[TestCase(ExpectedResult = 75000)]
public decimal AverageEarningsOfRichestQuartile() =>
    SamplePopulation
        .RichestQuartile()
        .AverageEarnings();
```

Итог рефакторинга:

1) Более читаемый тест.
2) More composable functions and a more readable interface.

## 5.3 Programming workflows

A workflow is a meaningful sequence of operations leading to a desired result.

Each operation in the workflow can be performed by a function, and these functions can
be composed into *function pipelines* that perform the workflow.

Example of workflow. User requesting to make a money transfer through the Bank:

1. Validate the requested transfer.
2. Load the account.
3. If the account has sufficient funds, debit the amount from the account.
4. Persist the changes to the account.
5. Wire the funds via the SWIFT network.

### 5.3.1 A simple workflow for validation

Simplify entire money transfer:

1. Validate the requested transfer.
2. Book the transfer (all subsequent steps).

ASP.NET MVC Controller implementing this workflow:

```csharp
using Microsoft.AspNetCore.Mvc;

public class MakeTransferController : Controller
{
    IValidator<MakeTransfer> validator;

    // (1) POST requests to this route are routed to this method.
    // (2) The request body will be deserialized into a MakeTransfer.
    [HttpPost, Route("api/MakeTransfer")]                           // (1)
    public void MakeTransfer([FromBody] MakeTransfer transfer)      // (2)
    {
        if (validator.IsValid(transfer))
            Book(transfer);
    }

    void Book(MakeTransfer transfer) =>
        // actually book the transfer...
```

Validation is delegated to a service on which the controller depends,
which implements this interface:

```csharp
public interface IValidator<T>
{
    bool IsValid(T t);
}
```

Эта часть кода контроллера:

```csharp
if (validator.IsValid(transfer))
    Book(transfer);
```

Выглядит плохо: хотя сейчас и используется только один if. Всегда имеется тенденция
к увеличению числа ветвлений, усложнению кода, ухудшению его читаемости и, как-слествие,
увеличению вероятности появления багов.

Решение - использование композиций функций вместо if.

### 5.3.2 Refactoring with data flow in mind

Есть два метода с такими сигнатурами:

* `IsValid()`: `MakeTransfer -> bool`
* `Book()`: `MakeTransfer -> ()`

Как их скомпоновать (compose)? Плюс, надо проверить что валидация проходит и только после этого
выполнять `Book()`.

В этом поможет `Option`: можно использовать `Some` как индикатор для valid данных.

Перепишем controller method. Using Option to represent passing/failing validation:

```csharp
public void MakeTransfer([FromBody] MakeTransfer transfer) =>
    Some(transfer)
        .Where(validator.IsValid)
        .ForEach(Book);

void Book(MakeTransfer transfer) =>
    // actually book the transfer...
```

Что сделано:

1) Lift the transfer data into an `Option` (использование `Some`).

2) Apply the `IsValid` predicate with `Where`. `Where` yield a `None` if validation fails,
in which case Book won't be called.

### 5.3.3 Composition leads to greater flexibility

With workflow in place, it becomes easy to make changes such as adding a step to the workflow.

Example. Adding a new step to an existing workflow:

```csharp
public void MakeTransfer([FromBody] MakeTransfer transfer) =>
    Some(transfer)
        .Map(Normalize)     // Plug a new step into the workflow.
        .Where(validator.IsValid)
        .ForEach(Book);

MakeTransfer Normalize(MakeTransfer request) => // ...
```

## 5.4 An introduction to functional domain modeling

Domain modeling means creating a representation for the entities and behaviors specific
to the business domain in question.

Fundamental differences between the OO and functional approaches.

In *OOP*, data and behavior live in the same object, and methods in the object can typically
modify the object's state:

```csharp
public class Account
{
    public decimal Balance { get; private set; }

    public Account(decimal balance) { Balance = balance; }

    public void Debit(decimal amount)
    {
        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds");
        Balance -= amount;
    }
}
```

Implementation of `Debit` is full of side effects: exceptions if business validation fails,
and state mutation. 

In *FP* data is captured with "dumb" data objects while behavior is encoded in functions,
so we'll separate the two:

```csharp
// Only contains data
public class AccountState
{
    // No setters, so AccountState is immutable.
    public decimal Balance { get; }

    public AccountState(decimal balance) { Balance = balance; }
}

public static class Account
{
    // (1) None here signals that the debit operation failed.
    public static Option<AccountState> Debit(this AccountState acc, decimal amount) =>
        (acc.Balance < amount)
            ? None    // (1)
            : Some(new AccountState(acc.Balance - amount));
}
```

Implementation of `Debit`:

1) pure function. 
2) uses `None` to signal an invalid state, and skip the following computations.
3) returns a value, which can be used as input to the next function in the chain.

## 5.5 An end-to-end server-side workflow

We still need to implement the `Book` function, which should do the following:

1. Load the account.
2. If the account has sufficient funds, debit the amount from the account.
3. Persist the changes to the account.
4. Wire the funds via the SWIFT network.

Let's define two services that capture DB access and SWIFT access:

```csharp
interface IRepository<T>
{
    Option<T> Get(Guid id);
    void Save(Guid id, T t);
}

interface ISwiftService
{
    void Wire(MakeTransfer transfer, AccountState account);
}
```

Full implementation of the end-to-end workflow in the controller:

```csharp
public class MakeTransferController : Controller
{
    IValidator<MakeTransfer> validator;
    IRepository<AccountState> accounts;
    ISwiftService swift;

    public void MakeTransfer([FromBody] MakeTransfer transfer) =>
        Some(transfer)
            .Map(Normalize)
            .Where(validator.IsValid)
            .ForEach(Book);

    void Book(MakeTransfer transfer) =>
        accounts.Get(transfer.DebitedAccountId)
            .Bind(account => account.Debit(transfer.Amount))
            .ForEach(account =>
            {
                accounts.Save(transfer.DebitedAccountId, account);
                swift.Wire(transfer, account);
            });
}
```

Let's look at the newly added Book method:

* `accounts.Get` returns an `Option` (`None` - no account was found with the given ID )
* `Debit` returns an `Option` (`None` - there were insufficient funds)
* `accounts.Get` и `Debit` can compose with `Bind`.
* `ForEach` perform the side effects we need:
  * saving the account with the new, lower balance
  * wiring the funds to SWIFT 

Недостатки в текущей реализации `Book`:

* Несколько `Option` не дают ответ почему и на каком этапе что-то пошло не так
(в главе 6 будет решение - использование `Either`).

* saving the account and wiring the funds should be done atomically:
if the process fails in the middle, we could have debited the funds without sending them to SWIFT.

### 5.5.1 Expressions vs. statements

>### Expressions (выражения), statements (операторы), declarations (декларирование)
>
>*Expressions* include anything that produces a value, such as these:
>* literals (константы, литералы), such as `123` or `"something"`
>* variables, such as `x`
>* invocations (вызовы), such as `"hello".ToUpper()` or `Math.Sqrt(Math.Abs(n) + m)`
>* operators and operands, such as `a || b`, `b ? x : y` or `new object()`
>
>Expressions can be used wherever a value is expected: for example, as arguments
>in function invocations or as return values of a function.
>
>*Statements* are instructions to the program, such as assignments, conditionals
>(`if`/`else`), loops, and so on.
>*Declarations* (of classes, methods, fields, and so on) are often considered statements,
>but for the purpose of this discussion are best thought of as a category in their own
>right (сама по себе). Whether you prefer statements or expressions, declarations are
>equally necessary (одинаково необходимы), so they're best left out (исключить) of the >"statements vs. expressions" >argument.

* Functional code relies on expressions. 
* Expressions have a value (statements don't).
* Expressions such as function calls *can* have side effects (statements *only* have side effects).
* Expressions compose (statements don't).
* Functions like `ForEach` don't have a useful return value, so that's where the pipeline ends
(This helps to isolate side effects, even visually).

**Recommendation** - try coding using just expressions. It doesn't guarantee good
design, but it certainly promotes better design. (Tip: since C# 6 there's a giveaway - if
you have braces, it's a statement).

### 5.5.2 Declarative vs. imperative

In FP our code becomes more declarative:
* It "declares" what's being computed
* It's higher-level, and closer to the way in which we communicate with other human beings.
* Code is closer to language, and hence easier to understand and to maintain. 

Comparing the imperative and declarative styles

| Imperative                                 | Declarative                                                  |
|--------------------------------------------|--------------------------------------------------------------|
| Tells the computer what to do; for example, "Add this item to this list." | Tells the computer what you want; for example, "Give me all the items that match a condition." |
| Relies mainly on statements. | Relies mainly on expressions. |
| Side effects are ubiquitous. | Side effects naturally gravitate toward the end of the expression evaluation. |
| Statements can be readily translated into machine instructions. | There is more indirection (hence, potentially more optimizations) in the process of translating expressions to machine instructions. |

### 5.5.3 The functional take on layering

Приведены пара видов layering, distinguishing a hierarchy of high- to low-level
components, where the highest level components are entry points into the application
(in our example, the controller), and the lowest are exit points (in our example, the
repository and SWIFT service).

Рекомендуемый подход:

A higher-level component can depend on any lower-level component, but not
vice versa - this is a more flexible and effective approach to layering.

In our example, there's a top-level workflow that composes functions
exposed by lower-level components. There are a couple of advantages here:

* You get a clear, synthetic overview of the workflow within the top-level component
(but note that this doesn't preclude you from defining subworkflows
within a lower-level component).

* Mid-level components can be pure.

```text
Controller | Validator | Account | Repository | Swift
<--------->|           |         |            |
<------------------------------->|            |
<--------------------->|         |            |
-------------------------------->|            |
--------------------------------------------->|
```

## Summary

* Function composition means combining two or more functions into a new function,
and it's widely used in FP.

* In C#, the extension method syntax allows you to use function composition by
chaining methods.

* Functions lend themselves (поддаются) to being composed if they are pure, chainable, and
shape-preserving.

* Workflows are sequences of operations that can be effectively expressed in your
programs through function pipelines: one function for each step of the workflow,
with the output of each function fed into the next.

* The LINQ library has a rich set of easily composable functions to work with
`IEnumerable`'s, and you can use it as inspiration to write your own APIs.

* Functional code prefers expressions over statements, unlike imperative code.

* Relying on expressions leads to your code becoming more declarative, and
hence (следовательно) more readable.
