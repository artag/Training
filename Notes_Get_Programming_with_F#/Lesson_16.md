# Lesson 16. Useful collection functions

As in LINQ, most of the methods in F# collections operate on empty collections without a
problem; you'll get back an empty collection again.

## Коллекции C# и F#

*(Из слайдов доклада Андрея Чебукина "Зачем учить F# и как начать...")*

### Коллекции. Поиск

| C#                       | F#                               |
|--------------------------|----------------------------------|
| Single                   | exactlyOne, pick                 |
| SingleOrDefault          | tryPick                          |
| All, TrueForAll          | forall, forall2                  |
| Any, Exists              | exists, exists2                  |
| Contains                 | contains                         |
| ElementAt                | item, nth                        |
| ElementAtOrDefault       | tryItem                          |
| Find                     | find, tryFind                    |
| FindIndex                | findIndex, tryFindIndex          |
| FindLast                 | findBack, tryFindBack            |
| FindLastIndex            | findIndexBack, tryFindIndexBack  |
| First, Last              | head, last                       |
| FirstOrDefault           | tryHead                          |
| IndexOf, LastIndexOf     | нет                              |
| LastOrDefault            | tryLast                          |

### Коллекции. Проекция

| C#                       | F#                               |
|--------------------------|----------------------------------|
| ConvertAll               | map, map2, map3, mapi, mapi2     |
| ForEach                  | iteri, iteri2                    |
| Select                   | Seq.map                          |
| через Select             | pairwise                         |
| нет                      | indexed                          |

### Коллекции. Сортировка

| C#                       | F#                               |
|--------------------------|----------------------------------|
| OrderBy                  | sortBy, sortWith, sort           |
| OrderByDescending        | sortByDescending, sortDescending |

### Коллекции. Группировка

| C#                       | F#                               |
|--------------------------|----------------------------------|
| Reverse                  | Rev                              |
| GroupBy                  | groupBy                          |
| GroupJoin                | нет                              |

### Коллекции. Фильтрация

| C#                       | F#                               |
|--------------------------|----------------------------------|
| Distinct                 | distinct, distinctBy             |
| FindAll                  | filter                           |
| GetRange                 | нет                              |
| OfType                   | через pick                       |
| Skip                     | skip                             |
| SkipWhile                | skipWhile                        |
| Take                     | take                             |
| TakeWhile                | takeWhile                        |
| Where                    | Seq.where                        |
| Select + Where           | choose                           |
| Skip(1)                  | tail                             |

### Коллекции. Аггрегация

| C#        | F#                                                                    |
|-----------|-----------------------------------------------------------------------|
| Aggregate | fold, fold2, foldBack, foldBack2, mapFold, mapFold2, reduce, reduceBack, scan, scanBack |
| Average   | averageBy, average  |
| Max       | max, maxBy          |
| Min       | min, minBy          |
| Sum       | sum, sumBy          |
| нет       | permute             |

### Коллекции. Преобразование

| C#                       | F#                               |
|--------------------------|----------------------------------|
| AsEnumerable             | toSeq                            |
| AsParallel               | нет                              |
| AsQueryble               | нет                              |
| AsReadOnly               | уже такой                        |
| Cast                     | через map                        |
| CopyTo                   | toArray                          |
| ToArray                  | toArray                          |
| ToList                   | нет                              |
| ToDictionary             | Map.ofList                       |
| ToLookup                 | нет                              |

### Коллекции. Слияние

| C#                       | F#                               |
|--------------------------|----------------------------------|
| Concat                   | append                           |
| Except                   | except, Set.difference           |
| Intersect                | Set.intersect                    |
| Join                     | нет                              |
| Union                    | Set.union                        |
| Zip                      | zip, zip3                        |

### Коллекции. Множественное

| C#                       | F#                               |
|--------------------------|----------------------------------|
| SelectMany               | нет                              |
| нет                      | Set.intersectMany                |
| нет                      | Set.unionMany                    |
| нет                      | concat                           |

### Коллекции. Разделение

| C#                       | F#                               |
|--------------------------|----------------------------------|
| нет                      | unzip, unzip3                    |
| нет                      | windowed                         |
| нет                      | partition                        |
| нет                      | splitInto                        |
| нет                      | splitAt                          |

### Коллекции. Инициализация

| C#                       | F#                               |
|--------------------------|----------------------------------|
| нет                      | unfold                           |
| нет                      | init                             |
| Enumerable.Empty         | empty                            |
| Enumerable.Repeat        | replicate                        |
| нет                      | singletone                       |

## Tuples in higher-order functions

F# collection functions can use of tuples to pass pairs or triples of data items around.
Tuples can "unpack" within lambda expressions directly within a higher-order function:

```fsharp
[ "Isaac", 30; "John", 25; "Sarah", 18; "Faye", 27 ]
|> List.map(fun (name, age) -> ...)

// Например
type Person = { Name : string; Age : int }
[ "Isaac", 30; "John", 25; "Sarah", 18; "Faye", 27 ]
|> List.map(fun (name, age) -> { Name = name; Age = age})
```

## Mapping functions

*Mapping functions* take a collection of items and return another collection of items.

### map

>`map` <---> `Select` in LINQ.

This function converts all the items in a collection from one shape to another shape.
Always returns the same number of items in the output collection as were passed in.

```fsharp
let numbers = [ 1 .. 10 ]       // Input data
let timesTwo n = n * 2          // Mapping function
let outputFunctional = numbers |> List.map timesTwo
// int list = [2; 4; 6; 8; 10; 12; 14; 16; 18; 20]

let helloGood = 
    let list = [ "a"; "b"; "c" ]
    list |> List.map (fun element -> "hello " + element)
// string list = ["hello a"; "hello b"; "hello c"]

let add1 x = x + 1
[1..5] |> List.map add1
// [2; 3; 4; 5; 6]
```

### map2, map3, mapi, mapi2, indexed

(TIP use the `||>` operator to pipe a tuple as two arguments)

```fsharp
let intList1 = [ 2; 3; 4 ]
let intList2 = [ 5; 6; 7 ]
let intList3 = [ 8; 9; 10 ]

List.map2 (fun i1 i2 -> i1 + i2) intList1 intList2 
// [7; 9; 11]
List.map3 (fun i1 i2 i3 -> i1 + i2 + i3 ) intList1 intList2 intList3
// [15; 18; 21]
(intList1) |> List.mapi (fun index i1 -> index, i1)         // map with index
// [(0, 2); (1, 3); (2, 4)]
(intList1, intList2) ||> List.mapi2 (fun index i1 i2 -> index, i1 + i2) 
// [(0, 7); (1, 9); (2, 11)]
['a' .. 'c' ] |> List.indexed           // "indexed" is a shorter version of above
// [(0, 'a'); (1, 'b'); (2, 'c')]
intList1 |> List.indexed
// [(0, 2); (1, 3); (2, 4)]
```

### iter, iter2, iteri, iteri2

`iter` is essentially the same as `map`, except the function that you pass in must return
`unit`.

This is useful as an end function of a pipeline, such as saving records to a database - for
any function that has side effects.

```fsharp
let intList1 = [ 2; 3; 4 ]
let intList2 = [ 5; 6; 7 ]

intList1 |> List.iter (printf "num = %i; ")
// num = 2; num = 3; num = 4;

let sirs = [{ Name = "Isaac"; Age = 30 }; { Name = "John"; Age = 25 }; { Name = "Peter"; Age = 18 }]
let ladies = [{ Name = "Sarah"; Age = 28 }; { Name = "Amy"; Age = 21 }; { Name = "Mary"; Age = 20 }]
(sirs, ladies) ||> List.iter2 (fun sir lady -> printfn "Pair: %s and %s" sir.Name lady.Name)
// Pair: Isaac and Sarah
// Pair: John and Amy
// Pair: Peter and Mary

intList1 |> List.iteri(printf "(idx = %i num = %i); ")
// (idx = 0 num = 2); (idx = 1 num = 3); (idx = 2 num = 4);

(intList1, intList2) ||> List.iteri2 (fun idx n1 n2 -> printf "(index = %i sum = %i) " idx (n1 + n2))
// (index = 0 sum = 7) (index = 1 sum = 9) (index = 2 sum = 11)
```

### collect

>`collect` <---> `SelectMany` in LINQ.

The `collect` function can has many other names: `SelectMany`, `FlatMap`, `Flatten`, `Bind`.

It takes in a list of items, and a function that
*returns a new collection from each item in that collection* - and then merges them all back
into a *single* list.

```fsharp
type Order = { Id : int }
type Customer = { Id : int; Orders : Order list; Town : string }
let customers = [
    { Id = 1; Orders = [{ Id = 1 }; { Id = 2 }]; Town = "Moscow" }
    { Id = 2; Orders = [{ Id = 39 }]; Town = "Paris" }
    { Id = 4; Orders = [{ Id = 43 }; { Id = 56 }]; Town = "Rome" }
]
let orders = customers |> List.collect(fun c -> c.Orders)
// [{ Id = 1 }; { Id = 2 }; { Id = 39 }; { Id = 43 }; { Id = 56 }]
```

### pairwise

`pairwise` takes a list and returns a new list of *tuple pairs* of the original adjacent
(соседние) items.

```fsharp
['a'..'e'] |> List.pairwise
// [('a', 'b'); ('b', 'c'); ('c', 'd'); ('d', 'e')]

// Вычисление "промежутков" между соседними датами.
[ System.DateTime(2010,5,1)
  System.DateTime(2010,6,1)
  System.DateTime(2010,6,12)
  System.DateTime(2010,7,3) ]           // A list of dates
|> List.pairwise                        // Pairwise for adjacent dates
|> List.map(fun (a, b) -> b - a)        // Subtracting the dates from one another as a TimeSpan
|> List.map(fun time -> time.TotalDays) // Return the total days between the two dates
// [31.0; 11.0; 21.0]
```

### windowed

`windowed` - variation of function `pairwise`.

This function is similar to pairwise but allows you to control how many elements exist in each
window (rather than fixed at two elements).

For example `[ 1;2;3 ]; [2;3;4]; [3;4;5]` and so on.

## Grouping functions

### groupBy

>`groupBy` <---> `GroupBy` in LINQ.

```fsharp
let firstLetter (str:string) = str.[0]
["apple"; "alice"; "bob"; "carrot"] |> List.groupBy firstLetter
// [('a', ["apple"; "alice"]); ('b', ["bob"]); ('c', ["carrot"])]

type Customer = { Name : string; Town : string }
let customers = [
    { Name = "Isaac"; Town = "London" }
    { Name = "Sara"; Town = "Birmingham" }
    { Name = "Tim"; Town = "London" }
    { Name = "Michelle"; Town = "Manchester" } ]
customers |> List.groupBy (fun person -> person.Town)
// [("London", [{ Name = "Isaac"; Town = "London" }
//              { Name = "Tim"; Town = "London" }])
//  ("Birmingham", [{ Name = "Sara"; Town = "Birmingham" }]);
//  ("Manchester", [{ Name = "Michelle"; Town = "Manchester" }])]
```

### countBy

Returns the *number* of items in each group.

```fsharp
[ ("a","A"); ("b","B"); ("a","C") ] |> List.countBy fst
// [("a", 2); ("b", 1)]

[ ("a","A"); ("b","B"); ("a","C") ] |> List.countBy snd
// [("A", 1); ("B", 1); ("C", 1)]

customers |> List.countBy (fun person -> person.Town)
// [("London", 2); ("Birmingham", 1); ("Manchester", 1)]
```

### partition

You supply a *predicate* (a function that returns true or false) and a collection;
it *always* returns two collections, partitioned based on the predicate.

```fsharp
let isEven i = (i % 2 = 0)
[1..10] |> List.partition isEven
// ([2; 4; 6; 8; 10], [1; 3; 5; 7; 9])

// Decomposing the tupled result into the two lists
let londonCustomers, otherCustomers =
    customers |> List.partition(fun c -> c.Town = "London")
```

### chunkBySize, splitInto, splitAt

```fsharp
[1..10] |> List.chunkBySize 3
// [[1; 2; 3]; [4; 5; 6]; [7; 8; 9]; [10]]
[1] |> List.chunkBySize 3
// [[1]]

[1..10] |> List.splitInto 3
// [[1; 2; 3; 4]; [5; 6; 7]; [8; 9; 10]]
[1] |> List.splitInto 3
// [[1]]

['a'..'i'] |> List.splitAt 3
// (['a'; 'b'; 'c'], ['d'; 'e'; 'f'; 'g'; 'h'; 'i'])

['a'; 'b'] |> List.splitAt 3
// InvalidOperationException: The input sequence has an insufficient number of elements.
```

## More on collections

### Aggregation functions (sum, average, max, min)

```fsharp
let numbers = [1.0..10.0]
let total = numbers |> List.sum         // 55.0
let average = numbers |> List.average   // 5.5
let max = numbers |> List.max           // 10.0
let min = numbers |> List.min           // 1.0
```

### Miscellaneous functions

| F#       | LINQ        | Comments                                                                                                            |
|----------|-------------|---------------------------------------------------------------------------------------------------------------------|
| find     | Single()    | Equivalent to the `Single()` overload that takes a predicate; see also `findIndex`, `findback`, and `findIndexBack` |
| head     | First()     | Returns the first item in the collection; see also `last`                                                           |
| item     | ElementAt() | Gets the element at a given index                                                                                   |
| take     | Take()      | The F# take implementation throws an exception if there are insufficient elements in the collection; use `truncate` for equivalent behavior to LINQ’s `Take()`. See also `takeWhile`, `skip`, and `skipWhile` |
| exists   | Any()       | See also `exists2`                                                                                                  |
| forall   | All()       | See also `forall2`                                                                                                  |
| contains | Contains()  |                                                                                                                     |
| filter   | Where()     | See also `where`                                                                                                    |
| length   | Count()     | See also `distinctBy`                                                                                               |
| distinct | Distinct()  |                                                                                                                     |
| sortBy   | OrderBy()   | See also `sort`, `sortByDescending`, `sortDescending`, and `sortWith`                                               |

### find, findIndex, findback, findIndexBack

>`find` <---> `Single` in LINQ.

```fsharp
let listOfTuples = [ (1,"a"); (2,"b"); (3,"b"); (4,"a"); ]
listOfTuples |> List.find(fun (x,y) -> y = "b")
// (2, "b")

listOfTuples |> List.findIndex(fun (x,y) -> y = "b")
// 1

listOfTuples |> List.findBack(fun (x,y) -> y = "b")
// (3, "b")

listOfTuples |> List.findIndexBack(fun (x,y) -> y = "b")
// 2
```

### head, last

>`head` <---> `First` in LINQ.

```fsharp
[2..10] |> List.head    // 2
[2..10] |> List.last    // 10
```

### item

>`item` <---> `ElementAt` in LINQ.

```fsharp
[3..12] |> List.item(3)      // 6
[3..12] |> List.item(10)     // System.ArgumentException
```

### take, truncate, takeWhile, skip, skipWhile

>`take` <---> `Take` in LINQ.

```fsharp
[1..10] |> List.take 3          // [1; 2; 3]
[1..10] |> List.take 11         // InvalidOperationException

[1..5] |> List.truncate 4       // [1; 2; 3; 4]
[1..5] |> List.truncate 11      // [1; 2; 3; 4; 5]

[1..10] |> List.takeWhile(fun i -> i < 3)    // [1; 2]
[1..10] |> List.takeWhile(fun i -> i < 1)    // []

[1..10] |> List.skip 3      // [4; 5; 6; 7; 8; 9; 10]
[1..10] |> List.skip 11     // ArgumentException

[1..5] |> List.skipWhile (fun i -> i < 2)    // [2; 3; 4; 5]
[1..5] |> List.skipWhile (fun i -> i < 6)    // []
```

### exists, exists2

>`exists` <---> `Any` in LINQ.

```fsharp
[1..10] |> List.exists(fun i -> i > 3 && i < 5)    // true
[1..10] |> List.exists(fun i -> i > 5 && i < 3)    // false

let ints1 = [2; 3; 4]
let ints2 = [5; 6; 7]
(ints1, ints2) ||> List.exists2(fun i1 i2 -> i1 + 10 > i2)     // true
```

### forall, forall2

>`forall` <---> `All` in LINQ.

```fsharp
ints1 |> List.forall(fun i -> i > 5)                        // false
(ints1, ints2) ||> List.forall2(fun i1 i2 -> i1 < i2)       // true
```

### contains

>`contains` <---> `Contains` in LINQ.

```fsharp
[1..10] |> List.contains 5      // true
[1..10] |> List.contains 42     // false
```

### filter, where

>`filter` <---> `Where` in LINQ.

`where` is a synonym for `filter`

```fsharp
[1..10] |> List.filter(fun i -> i % 2 = 0)      // [2; 4; 6; 8; 10]
[1..10] |> List.where(fun i -> i % 2 = 0)       // [2; 4; 6; 8; 10]
```

### length, distinctBy

>`length` <---> `Count` in LINQ.

```fsharp
[1..11] |> List.length      // 11

[(1,"a"); (1,"b"); (1,"c"); (2,"d")] |> List.distinctBy fst    // [(1, "a"); (2, "d")]
```

### distinct

>`distinct` <---> `Distinct` in LINQ.

```fsharp
[1; 1; 1; 2; 3; 3; 1] |> List.distinct      // [1; 2; 3]
```

### rev, sort, sortBy, sortDescending, sortByDescending, sortWith

>`sortBy` <---> `OrderBy` in LINQ.

```fsharp
[1..5] |> List.rev
// [5; 4; 3; 2; 1]

[2; 4; 1; 3; 5] |> List.sort
// [1; 2; 3; 4; 5]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortBy fst
// [("a", "3"); ("b", "2"); ("c", "1")]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortBy snd
// [("c", "1"); ("b", "2"); ("a", "3")]

[2; 4; 1; 3; 5] |> List.sortDescending
// [5; 4; 3; 2; 1]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortByDescending fst
// [("c", "1"); ("b", "2"); ("a", "3")]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortByDescending snd
// [("a", "3"); ("b", "2"); ("c", "1")]
```

`sortWith`:

```fsharp
// example of a comparer
let tupleComparer tuple1 tuple2  =
    if tuple1 < tuple2 then 
        -1 
    elif tuple1 > tuple2 then 
        1 
    else
        0

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortWith tupleComparer
// [("a", "3"); ("b", "2"); ("c", "1")]
```

### Converting between collections

* Each module has functions to easily convert to and from each collection type.

* There are functions in all three modules that begin with `of` or `to`.

```fsharp
let numberOne =
    [ 1 .. 5 ]          // Construct an int list.
    |> List.toArray     // Convert from an int list to an int array.
    |> Seq.ofArray      // Convert from an int array to an int sequence.
    |> Seq.head
```
