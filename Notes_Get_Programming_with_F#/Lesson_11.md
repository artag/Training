# Lesson 11. Building composable functions

### Comparing methods and functions

| -           | C# methods                         | F# let -bound functions                  |
|-------------|------------------------------------|------------------------------------------|
| Behavior    | Statements or expressions          | Expressions by default                   |
| Scope       | Instance (object) or static (type) | Static (module level or nested function) |
| Overloading | Allowed                            | Not supported                            |
| Currying    | Not supported                      | Native support                           |

## Partial function application

Functions in two forms:

```fsharp
// Tupled function    int * int -> int
let tupledAdd(a,b) = a + b
let answer = tupledAdd (5,10)

// Curried function    int -> int -> int
let curriedAdd a b = a + b
let answer = curriedAdd 5 10
```

* *Tupled* functions force you to supply all the arguments at once (like C# methods),
and have a signature of `(type1 * type2 ... * typeN) -> result`. F# considers all the arguments
as a *single object*.
* *Curried* functions allow you to supply only *some* of the arguments to a function,
and get back a *new function* that expects the *remaining* arguments. Curried functions have a
signature of `arg1 -> arg2 ... -> argN -> result`.

### Calling a curried function in steps

```fsharp
// Creating a function in curried form
let add first second = first + second

// Partially applying "add" to get back a new function, addFive with signature int -> int
let addFive = add 5

// Calling addFive
let fifteen = addFive 10
```

>### Partial application and currying
>Partial application and currying являются родственными сущностями. A *curried function* is a
>*function* that itself returns a function. Partial application is the *act* of calling that
>curried function to get back a new function.

## Constraining functions

### Currying. Use partial application

1 способ. Обычный, как в C#. Explicitly creating wrapper functions in F#:

```fsharp
open System
let buildDt year month day = DateTime(year, month, day)
let buildDtThisYear month day = buildDt DateTime.UtcNow.Year month day
let buildDtThisMonth day = buildDtThisYear DateTime.UtcNow.Month day
```

2 способ. Использование curried functions:

* **Первый** аргумент curried функции может использоваться для организации wrapper.
* Для такого wrapper не требуется указывать остальные аргументы.
* Partially applied functions work from *left to right*: аргументы начинают применяться
слева.

Creating wrapper functions by **currying**:

```fsharp
let buildDt year month day = DateTime(year, month, day)
let buildDtThisYear = buildDt DateTime.UtcNow.Year              // year (used arg)
let buildDtThisMonth = buildDtThisYear DateTime.UtcNow.Month    // month (used arg)
```

### Pipelines

* **Последний** аргумент curried функции может использоваться для организации pipeline.
* Используется оператор `|>`.
* Можно использовать C# методы если они имеют только **один** аргумент.

Пример.

Logical flow:

1. Get the current directory.
2. Get the creation time of the directory.
3. Pass that time to the function `checkCreation`.
    * If the folder is older than seven days, the function prints *Old* to the console.
    * Otherwise prints *New*.

```text
 ()                        string                   DateTime                  string
----> getCurrentDirectory -------> getCreationTime ----------> checkCreation ------->
```

1. Calling functions arbitrarily (обычный способ):

```fsharp
// (1) - Temporary value to store the directory
// (2) - Using the temporary value in a subsequent method call
let time =
    let directory = Directory.GetCurrentDirectory()     // (1)
    Directory.GetCreationTime directory                 // (2)
checkCreation time
```

* *-* You have a set of temporary variables that are used to pass data to the next method in
the call.
* *-* If the chain was bigger, it'd quickly get unwieldy (быстро становится громоздкой).

2. Simplistic chaining of functions:

```fsharp
// Explicitly nesting method calls
checkCreation(
    Directory.GetCreationTime(
        Directory.GetCurrentDirectory()))
```

* *-* Read the code is now the opposite of the order of operation.

3. Chaining three functions together using the **pipeline** operator `|>`:

```fsharp
Directory.GetCurrentDirectory()     // Returns a string
|> Directory.GetCreationTime        // Takes in a string, returns a DateTime
|> checkCreation                    // Takes in a DateTime, prints to the console
```

### Sample F# pipelines and DSLs (domain-specific languages)

```fsharp
let answer = 10 |> add 5 |> timesBy 2 |> add 20 |> add 7 |> timesBy 3

loadCustomer 17 |> buildReport |> convertTo Format.PDF |> postToQueue    // An example DSL

let customersWithOverdueOrders =
    getSqlConnection “DevelopmentDb”
    |> createDbConnection
    |> findCustomersWithOrders Status.Outstanding (TimeSpan.FromDays 7.0)
```

This might look similar to a feature that already exists in C#: *extension methods*.
But they're not quite the same.

### Extension methods vs. curried functions

| -     | C# extension methods | F# |
|-------|----------------------|----|
| Scope | Methods must be explicitly designed to be extension methods in a static class with the extension point decorated with the `this` keyword. | Any single-argument .NET method (including the BCL) and all curried functions can be chained together. |
| Extension point  | First argument in method signature. | Last argument in function. |
| Currying support | None. | First class. |
| Paradigm | Not always a natural fit for OO paradigm with private state. | Natural fit for stateless functions. |

## Composing functions together

* Build a *new* function by plugging a *set* of compatible functions together.
* Используется оператор `>>` (compose).
* The output of the first function must be the same type as the input of the second function.

```fsharp
// Creating a function by composing
let checkCurrentDirectoryAge =
    Directory.GetCurrentDirectory     // unit -> string
    >> Directory.GetCreationTime      // string -> DateTime
    >> checkCreation                  // DateTime -> string

// Calling the newly created composed function
let description = checkCurrentDirectoryAge()
```
