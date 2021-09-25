# Lesson 15. Working with collections in F#

## Mutable DTO

Example:

```fsharp
type TeamSummary = { Name : string; mutable AwayWins : int }
```

### Object Initializer

Create an instance of `IComparer` without having to first define a concrete type:

```fsharp
let comparer =
    { new IComparer<TeamSummary> with
        member this.Compare(x,y) =
            if x.AwayWins > y.AwayWins then -1
            elif x.AwayWins < y.AwayWins then 1
            else 0 }
```

## The collection modules in F#

`List`, `Array`, `Seq`

`Seq` - containing functions designed for querying (and generating) collections.

* Большинство функций запросов в этих коллекциях являются *higher-order function*.
* Основной "шаблон" использования запросов:
  * *Input 1* - определяемая пользователем функция для настройки HOF.
  * *Input 2* - входной list, array, или sequence, к которым каким-либо образом применяется
  функция.
  * *Output* - новый list, array, или sequence с результатом операции.

Примеры:

```fsharp
// (1) - Passing a function into Seq.filter to get USA customers
// (2) - Using an inline lambda function with Array.map
// (3) - Getting UK customers with Seq.filter and pipeline operator

let usaCustomers = Seq.filter areFromUSA sequenceOfCustomers                    // (1)
let numbersDoubled = Array.map (fun number -> number * 2) arrayOfNumbers        // (2)
let customersByCity = List.groupBy (fun c -> c.City) customerList

// Тоже самое, что и предыдущие, но с использованием оператора |>
let ukCustomers = sequenceOfCustomers |> Seq.filter areFromUK                   // (3)
let tripledNumbers = arrayOfNumbers |> Array.map (fun number -> number * 3)
let customersByCountry = customerList |> List.groupBy (fun c -> c.Country)
```

### Transformation pipelines

Pipelines часто реализуются по следущей схеме:

1. *Create a collection*. Create a collection of some sort.
2. *"World" of collections with multiple transformations*. Perform one or many transformations.
3. *Aggregation*. End up with a final collection, or perform an aggregation to leave the
collections world (for example, `sum`, `average`, `first`, or `last`).

>### LINQ and F#
>Можно использовать LINQ в F#: `open` the `System.Linq` namespace.
>Но лучше использовать библиотеки для работы с коллекциями из F#.
>F# also has a `query { }` construct that allows use of `IQueryable` queries
>(в этой книге нет - читать самостоятельно на MSDN).

## Collection types in F#. Более подробно о collection типах

### Sequences

* Most common (наиболее распространенный) collection type.
* Alias for the `IEnumerable<T>` type in the BCL.
* Можно считать взаимозаменяемыми с LINQ-generated sequences.
* Lazily evaluated.
* Don’t cache evaluations (by default).
* *Arrays* and *Lists* implement `IEnumerable<T>` => you can use functions in the
`Seq` module over both of them as well (и над ними обоими тоже).

```fsharp
seq { 1; 2; 3 }     // Create sequence
```

### Arrays

* *Slice* allow you to extract a subset of an array.
* High performance, but ultimately **mutable**. (Хотя можно использовать функции из модуля
`Array` для создания новых arrays прикаждой операции).
* You can iterate over arrays by using `for ... do` syntax as per sequences.
* Arrays are just standard BCL arrays.

```fsharp
let numbersArray = [| 1; 2; 3; 4; 6 |]          // Creating an array by using [| |] syntax
let firstNumber = numbersArray.[0]              // Accessing an item by index
let firstThreeNumbers = numbersArray.[0 .. 2]   // Array-slicing syntax
numbersArray.[0] <- 99                          // Mutating the value of an item in an array
```

### Lists

* Не являются `System.Collections.Generic.List<T>` , a.k.a. `ResizeArray`.
* F# lists are native (родные) to F#.
* Immutable. (After create you can't add or remove items from it).
* Eagerly (энергичное) evaluated.
* Can index into them directly.
* Internally, F# lists are linked lists.

```fsharp
let numbers = [ 1; 2; 3; 4; 5; 6 ]  // Creating a list of six numbers
let numbersQuick = [ 1 .. 6 ]       // Shorthand form of list creation (works for arrays and sequences)
let head :: tail = numbers          // Decomposing a list into head (1) and a tail (2 .. 6)
let moreNumbers = 0 :: numbers      // Creating a new list by placing 0 at the front of numbers
let evenMoreNumbers = moreNumbers @ [ 7 .. 9 ]    // Append [ 7 .. 9 ] to create a new list
```

В дополнение к языковым фичам для работы с list (конструкции `::` и `@`)
есть еще модуль `List` с полезными функциями такими как sorting and filtering.

## Comparing F# sequences, lists, and arrays

| -                        | Seq       | List        | Array  |
|--------------------------|-----------|-------------|--------|
| Eager/lazy               | Lazy      | Eager       | Eager  |
| Forward-only             | Sometimes | Never       | Never  |
| Immutable                | Yes       | Yes         | No     |
| Performance              | Medium    | Medium/High | High   |
| Pattern matching support | None      | Good        | Medium |
| Interop with C#          | Good      | Medium      | Good   |

По оценке производительности не все так однозначно - все зависим от контекста использования.
Например, в *List* можно очень быстро добавлять элементы в head списка, но в tail, возможно,
будет не так быстро.
В *Seq* есть функция `Seq.cache`, которая позволяет избежать повторной evaluation.
