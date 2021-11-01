# Lesson 40. Unit testing in F#

## Knowing when to unit test in F#

Types of unit testing

| Type of test | Example                                            | Typical languages |
|--------------|----------------------------------------------------|-------------------|
| Simple type  | Is the value of the `Age` property an integer?     | JavaScript        |
| Complex type | Is the value of the `Postcode` field a postcode?   | JavaScript, C#    |
| Simple rule  | Only an in-credit customer can withdraw funds.     | JavaScript, C#    |
| Complex rule | Complex rules engine with multiple compound rules. | JS, C#, F#        |

The point here is that the stronger the type system, the *fewer* (меньше) tests you should need.

Для F# unit test могут писаться для complex rules or situations where the type system doesn't
protect you. Examples:

* *Complex business rules, particularly with conditionals (особенно с условиями)*.
* *Complex parsing* - parsing code can be tricky (сложный).
* *A list that must have a certain (определенное) number of elements in it*.

Примеры механизмов в F#, которые отменяют, сокращают и упрощают написание unit test'ов:

* *Expressions* - тестирование выражений, которые принимают одно значение и возвращают
другое. Работа производится с незменяемыми данными.
* *Exhaustive (полный) pattern matching* - F# compiler will tell you that you've missed
cases for conditional logic.
* *Single-case discriminated unions* - these provide you with confidence that you
haven't accidentally mixed up fields of the same type.
* *Option types* - not having null in the type system.

## Performing basic unit testing in F#

F# может работать с MSTest, NUnit и xUnit (т.к. F# компилируется в статические классы C#).
Также рекомендуется попробовать F#-specific unit-testing library called *Expecto*.

Небольшой модуль, который будет тестироваться:

```fsharp
module BusinessLogic

type Employee = { Name : string; Age : int }
type Department = { Name : string; Team : Employee list }

let isLargeDepartment department =
    department.Team.Length > 10

let isLessThanTwenty person =
    person.Age < 20

let isLargeAndYoungTeam department =
    department |> isLargeDepartment
    && department.Team |> List.forall isLessThanTwenty
```

Тест для этого модуля в xUnit:

```fsharp
module ``Business Logic Tests``
open BusinessLogic
open Xunit

let department = {
        Name = "Super Team"
        Team = [ for i in 1..15 -> { Name = $"Person %d{i}"; Age = 19 } ]
    }

// A simple wrapper around Assert.True
let isTrue (b:bool) = Assert.True b

[<Fact>]
let ``Large, young teams are correctly identified``() =
    // department |> isLargeAndYoungTeam |> Assert.True     // Можно так проверить
    department |> isLargeAndYoungTeam |> isTrue             // Или так
```

Особенности:

* module и метод можно назвать как угодно, используя (``).
* `isTrue` - пример вспомогательной функции для улучшения читаемости.

### FsUnit

*FsUnit* is a NuGet package that takes the preceding approach for a DSL so that you can
easily make fluent pipelines of conditions as tests.

FsUnit существует для NUnit и xUnit. Подключать nuget-пакет `FSUnit.XUnit`.

Пример. sing FsUnit to create human-readable tests:

```fsharp
open FsUnit.Xunit

// (1) - FsUnit's custom language functions for equality checking
// (2) - Custom checks for "greater than"
[<Fact>]
let ``FSUnit makes nice DSLs!``() =
    department
    |> isLargeAndYoungTeam
    |> should equal true            // (1)

    department.Team.Length
    |> should be (greaterThan 10)   // (2)
```

Еще примеры:

```fsharp
// function for string comparisons
isaac" |> should startWith "isa"

// collection tests
[ 1 .. 5 ] |> should contain 3
```

### Unquote

*Unquote* is a test framework wrapper with a difference. Works with both xUnit and NUnit.
It provides a way to easily assert whether the result of a comparison is *true* or
*false* - so, to check whether two values are equal to each other.

Пример. Using Unquote’s custom comparison operator `=!`

```fsharp
open Swensen.Unquote

// The custom =! operator fails if the values on both sides aren't equal.
[<Fact>]
let ``Unquote has a simple custom operator for equality``() =
    department |> isLargeAndYoungTeam =! true
```

You can compare whether two lists are equal, or two records are the same.

With Unquote you can use *code quotation*. Code quotation - a block of code in which code is
treated (рассматривается) as data that can be programmed against.

Пример. Evaluating a quotation `<@ @>` with Unquote:

```fsharp
// (1) - Wrapping a condition within a quitation block
[<Fact>]
let ``Unquote can parse quotations for excellent diagnostics``() =
    let emptyTeam = { Name = "Super Team"; Team = [] }
    test <@ emptyTeam.Name.StartsWith "S" @>            // (1)
```
