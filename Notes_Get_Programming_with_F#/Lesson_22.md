# Lesson 22. Fixing the billion-dollar mistake

## Mandatory (обязательные) and optional values in C#

| Data type |  Example           | Support for "mandatory" | Support for "optional" |
|-----------|--------------------|-------------------------|------------------------|
| Classes   | String, WebClient  | No                      | Yes                    |
| Structs   | Int, Float         | Yes                     | Partial                |

* *Classes* should always be considered optional, because you can set them to null at
any point in time; it’s impossible in the C# type system to indicate that a string
can never be null.
* *Structs* can never be marked as null, but have a default value instead. For example,
an integer will be set to 0 by default. There *are* some ways that we can handle
optional data, but these are achieved at the library level, and not within the language
and type system.

### Null reference

Вызов `NullReferenceException`. Компилятор не предупредит об ошибке:

```csharp
string x = null;            // Creating a null reference to a string
var length = x.Length;      // Accessing a property on a null object
```

Checking for nulls in C#:

```csharp
public void Foo(string test)    // Input argument-nullable class
{
    if (test == null)           // Manual check if value is null
        return

    // Main logic here...
```

Проблемы от такого использования подхода:

* По хорошему надо все и везде проверять на null перед использованием данных.
* Проверку на null можно забыть.
* Проверки на null загрязняют код.

### Nullable types in .NET

If you go straight to the `Value` property without first checking the `HasValue` property, you
still run the risk of getting an exception.

## Improving matters with the F# type system

### Mandatory data in F#

Все типы в F#: tuples, records и discriminated unions *не допускают* их установку в значение
`null` - компилятор не скомпилирует такой код.

В records нельзя опустить задание значений для каких-то из полей при их создании.

>### Beating the F# type system
>
>You can get null reference exceptions with F# types, using various attributes, and some
>interoperability scenarios. But 99% of the time, you can forget about null exceptions.

### The option type

`Option<T>` is a simple two-case discriminated union: `Some (value)` or `None`.

Sample code to calculate a premium:

```fsharp
let aNumber : int = 10
let maybeANumber : int option = Some 10  // Creating an optional number
let calculateAnnualPremiumUsd score =
    match score with
    | Some 0 -> 250                      // Handling a safety score of (Some 0)
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 150
    | None ->                            // Handling the case when no safety score is found
        printfn "No score supplied! Using temporary premium."
        300

// Calculating a premium with a wrapped score of (Some 250) and then None
calculateAnnualPremiumUsd (Some 250)
calculateAnnualPremiumUsd None
```

В функцию `calculateAnnualPremiumUsd` теперь нельзя просто передать значение 250 - сначала ее
надо wrap as `Some 250`.

## Using the `Option` module

### `Option` properties

* `IsSome` - Better to use matching or helper functions

* `IsNone` - Better to use matching or helper functions

* `Value` - value of the object without even checking whether it exists. **Don’t ever use this!**
Instead, use pattern matching to force you to deal with both `Some` and `None` cases in your
code up front

### Unwrap `Option`

In F# 4.1:

```fsharp
// input - option type
// "" - value on None
Option.defaultValue "" input
// or
input |> Option.defaultValue ""
```

Older F# version:

```fsharp
input |> fun s -> defaultArg s ""
// or
input |> defaultArg <| ""
```

Hand-made function:

```fsharp
module Option =
    let defaultTo defValue opt = 
        match opt with
        | Some x -> x
        | None -> defValue

// Usage:
input |> Option.defaultTo ""
```

### `Option.map` (Mapping)

`Option.map` - higher-order function that takes in an optional value and a mapping function
to act on it, but calls mapping only if the value is `Some`.

If the value is `None`, it does nothing.

```fsharp
type Customer = { Id : int; Name : string; Score : int option }

// Function "describe" not designed to work with optional scores
// int -> string
let describe score =
    match score with
    | 0 -> "Standard Risk"
    | score when score < 0 -> "Safe"
    | score when score > 0 -> "Hard Risk"

// Customer -> string option
let descriptionOne customer =               // Вариант 1
    match customer.Score with
    | Some score -> Some(describe score)
    | None -> None

// Customer -> string option
let descriptionTwo customer =               // Вариант 2
    customer.Score
    |> Option.map(fun score -> describe score)

// Customer -> string option
let descriptionThree customer =             // Вариант 3
    customer.Score |> Option.map describe

// int option -> string option
let optionalDescribe = Option.map describe
```

Another small example:

```fsharp
Some 99 |> Option.map(fun v -> v * 2)       // Some 198
None |> Option.map(fun v -> v * 2)          // None
```

### `Option.iter`

Обычно используется для функций которые выполняют side effects.

```fsharp
None |> Option.iter(fun n -> printfn "Num = %i" n)      // Нет печати
Some 0 |> Option.iter(fun n -> printfn "Num = %i" n)    // Num = 0
Some 1 |> Option.iter(fun n -> printfn "Num = %i" n)    // Num = 1
```

### `Option.bind` (Binding)

`Option.bind` is more or less the equivalent of `List.collect` (or `SelectMany` in LINQ).

It can flatten an `Option<Option<string>>` to `Option<string>`,
just as `collect` can flatten a `List<List<string>>` to `List<string>`.

This is useful if you chain multiple functions together, each of which returns an option.

```fsharp
// int -> Customer option
let tryFindCustomer cId =
    if cId = 10 then Some drivers.[0]
    else None

// Two functions that each return an optional value
// Customer -> int option
let getScore customer = customer.Score

// Binding both functions together
// int -> int option
let score = tryFindCustomer 10 |> Option.bind getScore
```

Такой паттерн вызова функций через `bind` используется во многих местах ФП. Более
подробно об этом можно почитать в статьях Scott Wlaschin про monads (монады). Например, статью
"Railway-Oriented Programming".

### `Option.filter` (Filtering)

`Option.filter` - run a predicate over an optional value.
If the value is `Some`, run the predicate. If it passes, keep the
optional value; otherwise, return `None`.

```fsharp
let test1 = Some 5 |> Option.filter(fun x -> x > 5)     // None
let test2 = None |> Option.filter(fun x -> x = 5)       // None
let test3 = Some 5 |> Option.filter(fun x -> x = 5)     // Some 5
```

### Other Option functions

| Function        | Description                                                                                |
|-----------------|--------------------------------------------------------------------------------------------|
| `Option.count`  | If optional value is `None` , returns 0; otherwise, returns 1.                             |
| `Option.exists` | Runs a predicate over an optional value and returns the result. If `None`, returns `false` |

and the others in the `Option` module...

## Collections and options

### `Option.toList`, `Option.toArray`

Takes in an optional value, and if it’s `Some` value, returns a list/array with that single
value in it. Otherwise, it returns an empty list/array.

### `List.choose`

You can think of it as a specialized combination of `map` and `filter` in one.
It allows you to apply a function that might return a value, and then automatically
strip out (удалить) any of the items that returned `None`.

```fsharp
// int -> string option
let tryLoadCustomer id =
    match id with
    | id when 2 < id && id < 7 -> Some (sprintf "Customer %i" id)
    | _ -> None

[ 1..10 ] |> List.choose(fun id -> tryLoadCustomer id)
// ["Customer 3"; "Customer 4"; "Customer 5"; "Customer 6"]
```

### "Try" functions

В коллекциях встречаются функции, которые начинаются с try:

* `tryFind`
* `tryHead`
* `tryItem`

По функционалу они примерно эквивалентны `OrDefault` из LINQ. Отличие в том, что вместо `null`
они возвращают `Option`: `Some` если что-то было найдено и `None` если нет.
