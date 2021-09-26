# Lesson 17. Maps, dictionaries, and sets

## Standard mutable dictionary in F#. Generic.Dictionary

Тип из C# `System.Collections.Generic.Dictionary` можно использовать в F#.

```fsharp
open System.Collections.Generic

let inventory = Dictionary<string, float>()     // Creating a dictionary
inventory.Add("Apples", 0.33)                   // Adding items to the dictionary
inventory.Add("Oranges", 0.23)
inventory.Add("Bananas", 0.45)
inventory.Remove "Oranges"                      // Removing an item from the dictionary
let bananas = inventory.["Bananas"]             // Retrieving an item
let oranges = inventory.["Oranges"]             // Doesn't exist - exception is raised
```

Generic type inference with Dictionary:

```fsharp
let inventory = Dictionary<_,_>()    // Explicit placeholders for generic type arguments
inventory.Add("Apples", 0.33)
let inventory = Dictionary()         // Omitting generic type arguments completely
inventory.Add("Apples", 0.33)
```

*Недостаток* использования: изменяемость.

## Immutable dictionaries. IDictionary

В F# есть классная встроенная функция `dict` для создания неизменямого `IDictionary`.

Чтобы создать неизменяемый словарь, надо на вход `dict` подать список tuples, представляющих
собой пару ключ/значение:

```fsharp
// (1) Creating a (string * float) list of your inventory
// (2) Creating an IDictionary from the list
let inventory : IDictionary<string, float> =
    [ "Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45 ]    // (1)
    |> dict                                                 // (2)

let bananas = inventory.["Bananas"]     // Retrieving an item
inventory.Add("Pineapples", 0.85)       // System.NotSupportedException thrown
inventory.Remove("Bananas")             // System.NotSupportedException thrown
```

We implemented `IDictionary` as immutable.
*Недостаток*: Next methods throw exceptions:

* `Add`

* `Clear`

* `Remove`

Решение: использование другого типа в F#: `Map`.

>### Quickly creating full dictionaries
>Стандартный тип `Dictionary` не позволяет лего создать словарь с начальным набором данных так,
>как это может сделать `dict`. Но можно сделать так, через создание `IDictionary`:
>
>```fsharp
>[ "Apples", 10; "Bananas", 20; "Grapes", 15 ] |> dict |> Dictionary
>```

## The F# Map

The F# `Map` is an immutable key/value lookup.

* `Map` (как и `dict`) offers the ability to quickly create a lookup based on a sequence of
tuples.

* `Map` allows you to safely add or remove items. Создает новую копию `Map` с изменением.

* Calling `Add` on a Map that already contains the key won't throw an exception.
Instead, it'll replace the old value with the new one as it creates the new Map

* You can also safely access a key in a Map by using `TryFind`.
This doesn't return the value, but a wrapped `option`.

```fsharp
// (1) Creating a (string * float) list of your inventory
// (2) Converting the list into a Map for quick lookups
let inventory =
    [ "Apples", 0.33; "Oranges", 0.23; "Bananas", 0.45 ]    // (1)
    |> Map.ofList                                           // (2)

let apples = inventory.["Apples"]           // Retrieving an item
let pineapples = inventory.["Pineapples"]   // KeyNotFoundException thrown

let newInventory =
    inventory
    |> Map.add "Pineapples" 0.87    // Copying the map with a new item added
    |> Map.remove "Apples"          // Copying the map with an existing item removed
```

### Useful Map functions

`Map` module has other useful functions that are similar in nature to those in the `List`, `Array`, and `Seq` module. The most popular:

* add
* remove
* map
* filter
* iter
* partition

Отличия HOF функций, которые принимает `Map` от аналогичных в `List`, `Array` и `Seq` в том,
что они принимают пару ключ/значение.

Example:

```fsharp
// (1) Two maps, partitioned on cost
// (2) Partition HOF that receives both key (fruit) and value (cost) as arguments.
let cheapFruit, expensiveFruit =                    // (1)
    inventory
    |> Map.partition(fun fruit cost -> cost < 0.3)  // (2) parameters "fruit cost" - not tuple!
```

*Примечание*: the key and value aren't passed as a tuple but as a curried function, which is
why `fruit` and `cost` are separated by a space, and not a comma.

## Dictionaries, dict, or Map?

My advice is as follows:

* Use `Map` as your *default* lookup type. It's immutable, and has good support for F#
tuples and pipelining.

* Use the `dict` function to quickly generate an `IDictionary` that's needed for
*interoperability* with other code (for example, BCL code).
The syntax is lightweight and is easier to create than a full `Dictionary`.

* Use `Dictionary` if you need a *mutable* dictionary, or have a block of code with
specific *performance* requirements. Generally, the performance of `Map` will be fine,
but if you're in a tight loop performing thousands of additions or removals to a
lookup, a `Dictionary` will perform better. As always, optimize as needed, rather
than prematurely.

## Sets

Используются редко, хотя могут быть очень полезными в некоторых ситуациях.
Содержат только уникальные значения.

`Set` can't contain duplicates and will automatically remove repeated items in the set.

Creating a set from a sequence:

```fsharp
// Input data
let myBasket = [ "Apples"; "Apples"; "Apples"; "Bananas"; "Pineapples" ]
let fruitsILike = myBasket |> Set.ofList        // Converting to a set
// val fruitsILike : Set<string> = set ["Apples"; "Bananas"; "Pineapples"]
```

```fsharp
let yourBasket = [ "Kiwi"; "Bananas"; "Grapes" ]
// Combining the two baskets by using @, then distinct
let allFruitsList = (fruits @ otherFruits) |> List.distinct
// Creating a second set
let fruitsYouLike = yourBasket |> Set.ofList
// "Summing" two Sets together performs a Union operation
let allFruits = fruitsILike + fruitsYouLike     // (можно использовать Set.union)
```

Функции в `Set`:

* `Set.union` <-> operator overloads for addition (равнозначен операции сложения `+`)

* `Set.difference` <-> operator overloads for subtraction (равнозначен операции вычитания `-`)

* `Set.intersect`

* `Set.isSubset`

* `Set.map`

* `Set.filter`

* Можно трансформировать `Set` в/из `List`, `Seq`, `Array`

Sample `Set`-based operation:

```fsharp
// Gets fruits in A that are not in B
let fruitsJustForMe = allFruits – fruitsYouLike
// Gets fruits that exist in both A and B
let fruitsWeCanShare = fruitsILike |> Set.intersect fruitsYouLike
// Are all fruits in A also in B?
let doILikeAllYourFruits = fruitsILike |> Set.isSubset fruitsYouLike
```
