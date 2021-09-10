# Get Programming with F#

* [Lesson 02. Creating your first F# program](Lesson_02.md)

* [Lesson 04. Saying a little, doing a lot](Lesson_04.md)

* [Lesson 05. Trusting the compiler](Lesson_05.md)

## Lesson 6

### Mutability basics in F#

Creating immutable values:

```fsharp
let name = "isaac"      // Creating an immutable value
name = "kate"           // the = operator represents equality (as == in C#)
name <- "kate"          // error FS0027: This value is not mutable
```

Creating a mutable variable:

```fsharp
let mutable name = "isaac"      // Defining a mutable variable
name <- "kate"                  // Assigning a new value to the variable
```

Working with mutable objects:

```fsharp
open System.Windows.Forms
let form = new Form()           // Creating the form object
form.Show()
form.Width <- 400               // Mutating the form by using the <- operator
form.Height <- 400
form.Text <- "Hello from F#!"
```

Shorthand for creating mutable objects
(creating and mutating properties of a form in one expression):

```fsharp
open System.Windows.Forms
let form = new Form(Text = "Hello from F#!", Width = 300, Height = 300)
form.Show()
```

## Lesson 7

### Statements and expressions compared

| -          | Returns something? | Has side-effects?
|------------|--------------------|------------------
| Statements | Never              | Always
| Expressions| Always             | Rarely

C# - statement-based language

F# - use expressions as the default way of working

### Explicitly ignoring the result of an expression

`ignore` takes in a value and discards it, before returning `unit`

```fsharp
let writeTextToDisk text =                          // Writes text to disk
    let path = System.IO.Path.GetTempFileName()
    System.IO.File.WriteAllText(path, text)
    path

let createManyFiles() =
    writeTextToDisk "The fox jumped over the lazy dog"    // warning from compiler
    writeTextToDisk "The fox jumped over the lazy dog"    // warning from compiler
    writeTextToDisk "The fox jumped over the lazy dog"

// Using ignore
let createManyFiles() =
    ignore(writeTextToDisk "The fox jumped over the lazy dog")
    ignore(writeTextToDisk "The fox jumped over the lazy dog")
    writeTextToDisk "The fox jumped over the lazy dog"
```

### Forcing statement-based code with unit

```fsharp
let now = System.DateTime.UtcNow.TimeOfDay.TotalHours
if now < 12.0 then Console.WriteLine "It's morning"         // returns unit
elif now < 18.0 then Console.WriteLine "It's afternoon"     // returns unit
elif now < 20.0 then ignore(5 + 5)                          // (1)
else ()                                                     // (2)

// (1) Ignoring an expression to return unit
// (2) else branch here is optional - explicitly returning unit for the final case
```

## Lesson 8

### while, try-catch

```fsharp
while true do
    try                                                 // (1)
        let destination = getDestination()              // (2)
        printfn "Trying to drive to %s" destination
        petrol <- driveTo petrol destination            // (3)
        printfn "Made it to %s! You have %d petrol left" destination petrol
    with ex -> printfn "ERROR: %s" ex.Message           // (4)
0                                                       // (5)

// (1) Start of a try/with exception-handling block
// (2) Get the destination from the user
// (3) Get updated petrol from core code and mutate state
// (4) Handle any exceptions
// (5) Return code
```

### failwith (Throw exception)

```fsharp
let getDistance destination =                   // Function definition
    if destination = "Gas" then 10              // (1)
    // some code here...
    else failwith "Unknown destination!"        // (2)

// (1) - Checking the destination and returning an int as an answer
// (2) - Throwing an exception if you can’t find a match
```

## Lesson 9

### Tuples

* Tuples желательно использовать только для работы с элементами до 3-х штук. Если больше, то лучше
использовать `record`.

* Tuples желательно использовать только локально, в публичных API надо использовать `record`.

```fsharp
let parse (person:string) =
    let parts = person.Split(' ')
    let playername = parts.[0]
    let game = parts.[1]
    let score = Int32.Parse(parts.[2])
    playername, game, score             // Creating a tuple

// Deconstructing a tuple into meaningful values
let playername, game, score = parse "Mary Asteroids 2500"
// val playername : string = "Mary"
// val game : string = "Asteroids"
// val score : int = 2500
```

### Nested (grouped) tuples

You can also nest, or group, tuples together:

```fsharp
// (string * string) * int 
let nameAndAge = ("Joe", "Bloggs"), 28          // Creating a nested tuple
let name, age = nameAndAge                      // Deconstructing a tuple
let (forename, surname), theAge = nameAndAge    // Deconstructing with the nested component
```

### Wildcards

If there are elements of a tuple that you’re not interested in, you can discard them while
deconstructing a tuple by assigning those parts to the underscore symbol:

```fsharp
let nameAndAge = "Jane", "Smith", 25
let forename, surname, _ = nameAndAge       // Discarding the third element
```

### Implicit mapping of out parameters to tuples

```fsharp
var number = "123";
var result = 0;                                     // (1)
var parsed = Int32.TryParse(number, out result);    // (2)
let result, parsed = Int32.TryParse(number);        // (3)

// (1) - Declaring the "out" result variable with a default value
// (2) - Trying to parse number in C#
// (3) - Replacing "out" parameters with a tuple in a single call in F#
```

## Lesson 10

### Creating records

Declaring records on a single line:

```fsharp
 type Address = { Line1 : string; Line2 : string }
```

Constructing a nested record in F#:

```fsharp
// Declaring the Customer record type
type Customer =
    { Forename : string
      Surname : string
      Age : int
      Address : Address
      EmailAddress : string }
// Creating a Customer with Address inline
let customer =
    { Forename = "Joe"
      Surname = "Bloggs"
      Age = 30
      Address =
        { Street = "The Street"
          Town = "The Town"
          City = "The City" }
          EmailAddress = "joe@bloggs.com" }
```

### Providing explicit types for constructing records

```fsharp
// Explicitly declaring the type of the address value
let address : Address =
    { Street = "The Street"
      Town = "The Town"
      City = "The City" }
// Explicitly declaring the type that the Street field belongs to
let addressExplicit =
    { Address.Street = "The Street"
      Town = "The Town"
      City = "The City" }
```

### Copy-and-update record syntax

```fsharp
// Creating a new version of a record by using the 'with' keyword
let updatedCustomer =
    { customer with
        Age = 31
        EmailAddress = "joe@bloggs.co.uk" }
```

### Equality checking

You can safely compare two F# records of the same type with a single `=` for full, deep
**structural** equality checking.

```fsharp
// Все поля address1 структурно равны полям address2
// Structure comparing two records by using the = operator
let isSameAddress = (address1 = address2)                               // true
// Structure comparing
let isSameAddress = address1.Equals address2                            // true
// Comparing by reference(!)
let isSameAddress = System.Object.ReferenceEquals(address1, address2)   // false
```

### Comparing classes and records

| -                          | .NET classes       | F# records            |
|----------------------------|--------------------|-----------------------|
| Default mutability of data | Mutable            | Immutable             |
| Default equality behavior  | Reference equality | Structural equality   |
| Copy-and-update syntax?    | No                 | Rich language support |
| F# type-inference support? | Limited            | Full                  |
| Guaranteed initialization  | No                 | Yes                   |

## Lesson 11

### Comparing methods and functions

| -           | C# methods                         | F# let -bound functions                  |
|-------------|------------------------------------|------------------------------------------|
| Behavior    | Statements or expressions          | Expressions by default                   |
| Scope       | Instance (object) or static (type) | Static (module level or nested function) |
| Overloading | Allowed                            | Not supported                            |
| Currying    | Not supported                      | Native support                           |

### Constraining functions

Explicitly creating wrapper functions in F#:

```fsharp
open System
let buildDt year month day = DateTime(year, month, day)
let buildDtThisYear month day = buildDt DateTime.UtcNow.Year month day
let buildDtThisMonth day = buildDtThisYear DateTime.UtcNow.Month day
```

* **Первый** аргумент curried функции может использоваться для организации wrapper.
* Для такого wrapper не требуется указывать остальные аргументы.

Creating wrapper functions by **currying**:

```fsharp
let buildDt year month day = DateTime(year, month, day)
let buildDtThisYear = buildDt DateTime.UtcNow.Year              // year (used arg)
let buildDtThisMonth = buildDtThisYear DateTime.UtcNow.Mont     // month (used arg)
```

### Pipelines

* **Последний** аргумент curried функции может использоваться для организации pipeline.
* Используется оператор `|>`
* Можно использовать C# методы если они имеют только **один** аргумент

Logical flow:

```text
 ()                        string                   DateTime                  string
----> getCurrentDirectory -------> getCreationTime ----------> checkCreation ------->
```

Calling functions arbitrarily:

```fsharp
let time =
    let directory = Directory.GetCurrentDirectory()
    Directory.GetCreationTime directory
checkCreation time
```

**Плохо**: you have a set of temporary variables that are used to pass data to the next method in
the call.

Simplistic chaining of functions:

```fsharp
checkCreation(
    Directory.GetCreationTime(
        Directory.GetCurrentDirectory()))
```

**Плохо**: read the code is now the opposite of the order of operation.

Chaining three functions together using the **pipeline** operator `|>`:

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

### Composing functions together

* Build a *new* function by plugging a *set* of compatible functions together.
* Используется оператор `>>`
* The output of the first function must be the same type as the input of the second function.

```fsharp
let checkCurrentDirectoryAge =                  // Creating a function by composing
    Directory.GetCurrentDirectory
    >> Directory.GetCreationTime
    >> checkCreation
let description = checkCurrentDirectoryAge()    // Calling the newly created composed function
```

## Lesson 12

### Sets of rules for organizing code

* Place related types together in namespaces.
* Place related stateless functions together in modules.

**Выводы**:

* Use namespaces as in C#, to logically group types and modules.

* Use modules primarily to store functions, and secondly to store types that are tightly related
to those functions.

### Namespaces and Modules

1)
* Namespaces can hold **only** types.
* Namespaces can span multiple files.

2)
* Modules can hold types and functions.
* Module can’t span multiple files.
* Modules can be **private** (*nested module*).

3)
* Modules are like static classes in C#.
* Modules are like namespaces but can also store functions.

Example:

```fsharp
// File domain.fs         (domain.fs must live above dataAccess.fs)
namespace MyApplication.BusinessLogic

type Customer = { ... }
type Account = { .. }

// File dataAccess.fs     (References to domain.fs)
module MyApplication.BusinessLogic.DataAccess

type private DbError = { … }
let private getDbConnection() = ...
let saveCustomer = ...
let loadCustomer = ...

module private Helpers =            // Nested module - visible only in DataAccess module
    let handleDbError ex = ...
    let checkDbVersion conn = ...
```

```fsharp
// Domain.fs     (выше чем Operation.fs)
namespace Domain        // Namespace declaration
// ...

// Operation.fs
module Operations       // Declaring a module
open Domain             // Opening the Domain namespace
// ...
```

### Tips for working with modules and namespaces

#### Access modifiers

* By default, types and functions are always public in F#.

* If you want to use a function within a module (or a nested module) but don’t want to expose it
publicly, mark it as **private**.

#### The global namespace

If you don’t supply a *parent* namespace when declaring namespaces or modules, it’ll
appear in the `global` namespace, which is always open.

#### Automatic opening of modules

Add the `[<AutoOpen>]` attribute on the module.

#### Scripts

You can create `let`-bound functions directly in a script.
This is possible because an implicit module is created for you based on the name of the script
(similar to automatic namespacing).
You can explicitly specify the module in code if you want, but with scripts it’s generally not
needed.

## Lesson 13

### Higher-order function (HOF) in F#. Dependency Injection (DI)

* Dependencies in F# tend to be functions; in C#, they’re interfaces.

```fsharp
type Customer = { Age : int }
let where filter customers =          // filer acts like a dependency injection
    seq {
        for customer in customers do
            if filter customer then   // Calling the filter function with customer as an argument
                yield customer }

let customers = [ { Age = 21 }; { Age = 35 }; { Age = 36 } ]
let isOver35 customer = customer.Age > 35    // filter

// Supplying the isOver35 function into the where function
customers |> where isOver35
// Passing a function inline using lambda syntax
customers |> where (fun customer -> customer.Age > 35)
```

* `seq { }` - This is a type of *computation expression*. Generate a sequence of customers by using
the `yield` keyword.

* `[ ; ; ; ] syntax` - F# list.

## Lesson 14

### Accessing .fs files from a script

```fsharp
#load "Domain.fs"           // Loading .fs files into a script
#load "Operations.fs"
#load "Auditing.fs"

open Capstone2.Operations   // Opening namespaces of .fs files
open Capstone2.Domain
open Capstone2.Auditing
open System
```

## Lesson 15

### Mutable DTO

Example:

```fsharp
type TeamSummary = { Name : string; mutable AwayWins : int }
```

### Object Initializer
7
Create an instance of `IComparer` without having to first define a concrete type:

```fsharp
let comparer =
    { new IComparer<TeamSummary> with
        member this.Compare(x,y) =
            if x.AwayWins > y.AwayWins then -1
            elif x.AwayWins < y.AwayWins then 1
            else 0 }
```

### Comparing F# sequences, lists, and arrays

| -                        | Seq       | List        | Array  |
|--------------------------|-----------|-------------|--------|
| Eager/lazy               | Lazy      | Eager       | Eager  |
| Forward-only             | Sometimes | Never       | Never  |
| Immutable                | Yes       | Yes         | No     |
| Performance              | Medium    | Medium/High | High   |
| Pattern matching support | None      | Good        | Medium |
| Interop with C#          | Good      | Medium      | Good   |

### Sequences

* Alias for the `IEnumerable<T>` type in the BCL
* Lazily evaluated
* Don’t cache evaluations (by default)
* *Arrays* and *Lists* implement `IEnumerable<T>` => you can use functions in the
Seq module over both of them as well.

```fsharp
seq { 1; 2; 3 }     // Create sequence
```

### Arrays

* *Slice* allow you to extract a subset of an array
* High performance, but ultimately mutable

```fsharp
let numbersArray = [| 1; 2; 3; 4; 6 |]          // Creating an array by using [| |] syntax
let firstNumber = numbersArray.[0]              // Accessing an item by index
let firstThreeNumbers = numbersArray.[0 .. 2]   // Array-slicing syntax
numbersArray.[0] <- 99                          // Mutating the value of an item in an array
```

### Lists

* Immutable

```fsharp
let numbers = [ 1; 2; 3; 4; 5; 6 ]  // Creating a list of six numbers
let numbersQuick = [ 1 .. 6 ]       // Shorthand form of list creation (works for arrays and sequences)
let head :: tail = numbers          // Decomposing a list into head (1) and a tail (2 .. 6)
let moreNumbers = 0 :: numbers      // Creating a new list by placing 0 at the front of numbers
let evenMoreNumbers = moreNumbers @ [ 7 .. 9 ]    // Append [ 7 .. 9 ] to create a new list
```

## Lesson 16

### map (Select() in LINQ)

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

### Tuples in higher-order functions

F# allows you to "unpack" tuples within lambda expressions directly within a higher-order function:

```fsharp
type Person = { Name : string; Age : int }
[ "Isaac", 30; "John", 25; "Sarah", 18; "Faye", 27 ]
|> List.map(fun (name, age) -> { Name = name; Age = age})
```

### iter, iter2, iteri, iteri2

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

```fsharp
['a'..'e'] |> List.pairwise
// [('a', 'b'); ('b', 'c'); ('c', 'd'); ('d', 'e')]

[ System.DateTime(2010,5,1)
  System.DateTime(2010,6,1)
  System.DateTime(2010,6,12)
  System.DateTime(2010,7,3) ]           // A list of dates
|> List.pairwise                        // Pairwise for adjacent dates
|> List.map(fun (a, b) -> b - a)        // Subtracting the dates from one another as a TimeSpan
|> List.map(fun time -> time.TotalDays) // Return the total days between the two dates
// [31.0; 11.0; 21.0]
```

### groupBy

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

```fsharp
[ ("a","A"); ("b","B"); ("a","C") ] |> List.countBy fst
// [("a", 2); ("b", 1)]

[ ("a","A"); ("b","B"); ("a","C") ] |> List.countBy snd
// [("A", 1); ("B", 1); ("C", 1)]

customers |> List.countBy (fun person -> person.Town)
// [("London", 2); ("Birmingham", 1); ("Manchester", 1)]
```

### partition

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

#### find, findIndex, findback, findIndexBack

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

#### head, last

```fsharp
[2..10] |> List.head    // 2
[2..10] |> List.last    // 10
```

#### item

```fsharp
[3..12] |> List.item(3)      // 6
[3..12] |> List.item(10)     // System.ArgumentException
```

#### take, truncate, takeWhile, skip, skipWhile

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

#### exists, exists2

```fsharp
[1..10] |> List.exists(fun i -> i > 3 && i < 5)    // true
[1..10] |> List.exists(fun i -> i > 5 && i < 3)    // false

let ints1 = [2; 3; 4]
let ints2 = [5; 6; 7]
(ints1, ints2) ||> List.exists2(fun i1 i2 -> i1 + 10 > i2)     // true
```

#### forall, forall2

```fsharp
ints1 |> List.forall(fun i -> i > 5)                        // false
(ints1, ints2) ||> List.forall2(fun i1 i2 -> i1 < i2)       // true
```

#### contains

```fsharp
[1..10] |> List.contains 5      // true
[1..10] |> List.contains 42     // false
```

#### filter, where

`where` is a synonym for `filter`

```fsharp
[1..10] |> List.filter(fun i -> i % 2 = 0)      // [2; 4; 6; 8; 10]
[1..10] |> List.where(fun i -> i % 2 = 0)       // [2; 4; 6; 8; 10]
```

#### length, distinctBy

```fsharp
[1..11] |> List.length      // 11

[(1,"a"); (1,"b"); (1,"c"); (2,"d")] |> List.distinctBy fst    // [(1, "a"); (2, "d")]
```

#### distinct


```fsharp
[1; 1; 1; 2; 3; 3; 1] |> List.distinct      // [1; 2; 3]
```

#### rev, sort, sortBy, sortDescending, sortByDescending, sortWith

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

## Lesson 17

### Standard mutable dictionary in F#

```fsharp
open System.Collections.Generic

let inventory = Dictionary<string, float>()     // Creating a dictionary
inventory.Add("Apples", 0.33)                   // Adding items to the dictionary
inventory.Add("Oranges", 0.23)
inventory.Add("Bananas", 0.45)
inventory.Remove "Oranges"                      // Removing an item from the dictionary
let bananas = inventory.["Bananas"]             // Retrieving an item
let oranges = inventory.["Oranges"]             // Doesn’t exist - exception is raised
```

Generic type inference with Dictionary:

```fsharp
let inventory = Dictionary<_,_>()    // Explicit placeholders for generic type arguments
inventory.Add("Apples", 0.33)
let inventory = Dictionary()         // Omitting generic type arguments completely
inventory.Add("Apples", 0.33)
```

### Immutable dictionaries

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
Next methods throw exceptions:

* `Add`

* `Clear`

* `Remove`

Quickly creating full dictionaries:

```fsharp
[ "Apples", 10; "Bananas", 20; "Grapes", 15 ] |> dict |> Dictionary
```

### The F# Map

The F# `Map` is an immutable key/value lookup.

* Calling `Add` on a Map that already contains the key won’t throw an exception.
Instead, it’ll replace the old value with the new one as it creates the new Map

* You can also safely access a key in a Map by using `TryFind`.
This doesn’t return the value, but a wrapped `option`.

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

Example:

```fsharp
// (1) Two maps, partitioned on cost
// (2) Partition HOF that receives both key (fruit) and value (cost) as arguments.
let cheapFruit, expensiveFruit =                    // (1)
    inventory
    |> Map.partition(fun fruit cost -> cost < 0.3)  // (2)
```

### Dictionaries, dict, or Map?

My advice is as follows:

* Use `Map` as your *default* lookup type. It’s immutable, and has good support for F#
tuples and pipelining.

* Use the `dict` function to quickly generate an `IDictionary` that’s needed for
*interoperability* with other code (for example, BCL code).
The syntax is lightweight and is easier to create than a full `Dictionary`.

* Use `Dictionary` if you need a *mutable* dictionary, or have a block of code with
specific *performance* requirements. Generally, the performance of `Map` will be fine,
but if you’re in a tight loop performing thousands of additions or removals to a
lookup, a `Dictionary` will perform better. As always, optimize as needed, rather
than prematurely.

### Sets

`Set` can’t contain duplicates and will automatically remove repeated items in the set.

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
let allFruits = fruitsILike + fruitsYouLike
```

* `Set.union` <-> operator overloads for addition

* `Set.difference` <-> operator overloads for subtraction

* `Set.difference`

* `Set.intersect`

* `Set.isSubset`

Sample `Set`-based operation:

```fsharp
// Gets fruits in A that are not in B
let fruitsJustForMe = allFruits – fruitsYouLike
// Gets fruits that exist in both A and B
let fruitsWeCanShare = fruitsILike |> Set.intersect fruitsYouLike
// Are all fruits in A also in B?
let doILikeAllYourFruits = fruitsILike |> Set.isSubset fruitsYouLike
```

## Lesson 18

### Creating your first aggregation function

Any aggregation, or `fold`, generally requires three things:

* The *input collection*

* An *accumulator* to hold the state of the output result as it’s built up

* An *initial (empty) value* for the accumulator to start with

### Imperative implementation of sum

```fsharp
let sum inputs =    // seq<int> -> int
    let mutable accumulator = 0                 // Empty accumulator
    for input in inputs do                      // Go through every item
        accumulator <- accumulator + input      // Apply aggregation
    accumulator                                 // Return accumulator
```

### fold

Signature for `Seq.fold`:

```fsharp
folder:( 'State -> 'T -> 'State) -> state:'State -> source:seq<'T> -> 'State
```

* `folder` - A function that’s passed into fold that handles the accumulation (summing,
averaging, or getting the length, for example).

* `state` - The initial start state

* `source` - The input collection

Implement `sum` by using the `fold` function (put the arguments on different lines to
make clearer):

```fsharp
// (1) Folder function to sum the accumulator and input let sum inputs
let sum inputs =
    Seq.fold
        (fun state input -> state + input)      // (1)
        0                                       // Initial state
        inputs                                  // Input collection
```

### Making fold more readable

Using `|>` (*pipeline*) and `||>` (*double pipeline*) operators:

```fsharp
// By default
Seq.fold (fun state input -> state + input) 0 inputs

// Using pipeline to move "inputs" to the left side
inputs |> Seq.fold (fun state input -> state + input) 0

// Using the double pipeline to move both the initial state and “inputs” to the left side
(0, inputs) ||> Seq.fold (fun state input -> state + input)
```

### Using related fold functions

* `foldBack` - Same as `fold`, but goes backward from the last element in the collection.

* `mapFold` - Combines `map` and `fold`, emitting a sequence of mapped results and a
final state.

* `reduce` - A simplified version of `fold` , using the first element in the collection
as the initial state, so you don’t have to explicitly supply one. Perfect for simple folds such as `sum` (although it’ll throw an exception on an empty input-beware!)

* `scan` - Similar to `fold` , but generates the intermediate results as well as the
final state. Great for calculating running totals.

* `unfold` - Generates a sequence from a single starting state. Similar to the `yield`
keyword.

### Folding instead of while loops

Accumulating through a `while` loop. Example: counts the number of characters in the
file:

```fsharp
open System.IO
let mutable totalChars = 0                              // Initial state
let sr = new StreamReader(File.OpenRead "book.txt")     // Opening a stream to a file

while (not sr.EndOfStream) do                               // Stopping condition
    let line = sr.ReadLine()
    totalChars <- totalChars + line.ToCharArray().Length    // Accumulation function
```

We have an unknown "end" to this stream of data, rather than a fixed, up-front
collection.

**Better**: simulating a collection through sequence expressions:

```fsharp
open System.IO
let lines : string seq =
    seq {                                   // Sequence expression
        use sr = new StreamReader(File.OpenRead @"book.txt")
        while (not sr.EndOfStream) do
            yield sr.ReadLine() }           // Yielding a row from the StreamReader

(0, lines) ||> Seq.fold(fun total line -> total + line.Length)  // A standard fold
```

* The `seq { }` block is a form of *computation expression*.

* Here, `yield` has the same functionality as in C#. It yields items to *lazily generate*
a sequence.

* `seq` (to create a sequence block) and `yield` (to yield back values)

### Alias to a specific type. Create and use a list of rules

Alias example:

```fsharp
type Rule = string -> bool * string
```

Creating a list of rules:

```fsharp
type Rule = string -> bool * string

let rules : Rule list =    // List definition
    [ fun text -> (text.Split ' ').Length = 3, "Must be three words"
      fun text -> text.Length <= 30, "Max length is 30 characters"
      fun text -> text
                  |> Seq.filter Char.IsLetter
                  |> Seq.forall Char.IsUpper, "All letters must be caps" ]
```

**Not true way** - composing rules manually:

```fsharp
let validateManual (rules: Rule list) word =
    let passed, error = rules.[0] word      // Testing the first rule
    if not passed then false, error         // Checking whether the rule failed
        else
        let passed, error = rules.[1] word  // Repeat for all remaining rules
        if not passed then false, error
            else
            let passed, error = rules.[2] word
            if not passed then false, error
            else true, ""
```

**True way** - composing a list of rules by using `reduce`:

*(похоже на composite pattern в ООП)*

```fsharp
// Rule seq -> Rule
// (string -> bool * string) seq -> (string -> bool * string)
let buildValidator (rules : Rule list) =
    rules
    |> List.reduce(fun firstRule secondRule ->
        fun word ->                                 // Higher-order function
            let passed, error = firstRule word      // Run first rule
            if passed then                          // Passed, move on to next rule
                let passed, error = secondRule word
                if passed then true, ""
                else false, error
            else false, error)                      // Failed, return error

// Использование
let validate = buildValidator rules
let word = "HELLO FrOM F#"
validate word
// val it : bool * string = (false, "All letters must be caps")
```

## Lesson 20

### `for .. in` loops

```fsharp
for number in 1 .. 10 do                    // Upward-counting for loop
    printfn "%d Hello!" number
// 1 Hello!
// ...
// 10 Hello!

// 10 = begin; -1 = step; 1 = end
for number in 10 .. -1 .. 1 do              // Downward-counting for loop
    printfn "%d Hello!" number
// 10 Hello!
// 9 Hello!
//  ...
// 1 Hello!

let customerIds = [ 45 .. 99 ]
for customerId in customerIds do            // Typical for-each-style loop
    printfn "%d bought something!" customerId

// 2 = begin; 2 = step; 10 = end
for even in 2 .. 2 .. 10 do                 // Range with custom stepping
    printfn "%d is an even number!" even
// 2 is an even number!
// 4 is an even number!
// ...
// 10 is an even number!
```

### `for ... to` loops

```fsharp
for identifier = start [ to | downto ] finish do
    body-expression
```

```fsharp
// A simple for...to loop.
for i = 1 to 10 do
    printf "%d " i
// 1 2 3 4 5 6 7 8 9 10

// A for...to loop that counts in reverse.
for i = 10 downto 1 do
    printf "%d " i
// 10 9 8 7 6 5 4 3 2 1

// A for...to loop that uses functions as the start and finish expressions.
let beginning x y = x - 2 * y
let ending x y = x + 2 * y

let function1 x y = 
    for i = (beginning x y) to (ending x y) do
         printf "%d " i

function1 10 4
// 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18
```

### `while` loops

```fsharp
// (1) Opening a handle to a text file
// (2) while loop that runs while the reader isn’t at the end of the stream
open System.IO
let reader = new StreamReader(File.OpenRead @"File.txt")    // (1)
while (not reader.EndOfStream) do                           // (2)
    printfn "%s" (reader.ReadLine())
```

### Breaking the loop

* There’s **no** concept of the `break` command.

* To simulate premature exit of a loop, you should consider replacing the loop with a
sequence of values that you `filter` on (or `takeWhile`), and loop over that sequence
instead.

### Comprehensions

The closest equivalent in C# would be the use of the `System.Linq.Enumerable.Range()`.

```fsharp
// Generating an array of the letters of the alphabet in uppercase
let arrayOfChars = [| for c in 'a' .. 'z' -> Char.ToUpper c |]
// char [] = [|'A'; 'B'; ... 'Z'|]

// Generating the squares of the numbers 1 to 10
let listOfSquares = [ for i in 1 .. 10 -> i * i ]
// int list = [1; 4; 9; 16; 25; 36; 49; 64; 81; 100]

// Generating arbitrary strings based on every fourth number between 2 and 20
let seqOfStrings = seq { for i in 2 .. 4 .. 20 -> sprintf "Number %d" i }
// seq ["Number 2"; "Number 6"; "Number 10"; "Number 14"; ...]
```

### `If`/`then` expressions for complex logic

```fsharp
// (1) A simple clause
// (2) Complex clause - AND and OR combined
// (3) Catchall for "good" customers
// (4) Catchall for other customers
let limit =
    if score = "medium" && years = 1 then 500                   // (1)
    elif score = "good" && (years = 0 || years = 1) then 750    // (2)
    elif score = "good" && years = 2 then 1000
    elif score = "good" then 2000                               // (3)
    else 250                                                    // (4)
```

### Pattern-matching

**!** Sequences can’t be pattern matched against; only arrays and lists are supported.

```fsharp
// (1) Implicitly matching on a tuple of rating and years
// (2) If medium score with one-year history, limit is $500
// (3) Two match conditions leading to $750 limit
// (4) Catchall for other customers with "good" score
// (5) Catchall for all other customers
let limit =
    match customer with                 // (1)
    | "medium", 1 -> 500                // (2)
    | "good", 0 | "good", 1 -> 750      // (3)
    | "good", 2 -> 1000
    | "good", _ -> 2000                 // (4)
    | _ -> 250                          // (5)
```

### Pattern-matching. Guards

```fsharp
// Using the when guard to specify a custom pattern
let getCreditLimit customer =
    match customer with
    | "medium", 1 -> 500
    | "good", years when years < 2 -> 750   // (1) "years when years < 2" - guard
    | "good", 2 -> 1000
    | "good", _ -> 2000
    | _ -> 250
```

### Pattern-matching. Nested matches

```fsharp
let getCreditLimit customer =
    match customer with
    | "medium", 1 -> 500
    | "good", years ->          // Matching on "good" and binding years to a symbol
        match years with        // A nested match on the value of years
        | 0 | 1 -> 750          // Single-value match
        | 2 -> 1000
        | _ -> 2000
    | _ -> 250                  // Global catchall
```

### Pattern-matching. Matching against lists example

```fsharp
// (1) Matching against an empty list
// (2) Matching against a list of one customer
// (3) Matching against a list of two customers
// (4) Matching against all other lists
let handleCustomer customers =
    match customers with
    | [] -> failwith "No customers supplied!"                                   // (1)
    | [ customer ] -> printfn "Single customer, name is %s" customer.Name       // (2)
    | [ first; second ] ->
        printfn "Two customers, balance = %d" (first.Balance + second.Balance)  // (3)
    | customers -> printfn "Customers supplied: %d" customers.Length            // (4)
```

Another example:

```fsharp
// Is a specific length
// Is empty
// Has the first item equal to 5
let checkList (numbers : int list) =
    match numbers with
    | numbers when numbers.Length = 7 -> printfn "List has seven numbers"
    | [] -> printfn "List is empty"
    | head::tail when head = 5 -> printfn "The first number is %i" head
    | _ -> printfn "Not match"
```

### Pattern-matching. Matching records example

```fsharp
// (1) Match against Balance field
// (2) Match against Name field
// (3) Catchall, binding Name to name symbol
let getStatus customer =
    match customer with
    | { Balance = 0 } -> "Customer has empty balance!"              // (1)
    | { Name = "Isaac" } -> "This is a great customer!"             // (2)
    | { Name = name; Balance = 50 } -> sprintf "%s has a large balance!" name
    | { Name = name } -> sprintf "%s is a normal customer" name     // (3)
```

### Pattern-matching. Combining multiple patterns

Checking the following three conditions *all at the same time*:

1. The list of customers has three elements.
2. The first customer is called "Tanya".
3. The second customer has a balance of 25.

```fsharp
// Pattern matching against a list of three items with specific fields
match customers with
| [ { Name = "Tanya" }; { Balance = 25 }; _ ] -> "It's a match!"
| _ -> "No match!
```

### When to use `if`/`then` over match

* Use pattern matching **by default**.

* Use `if`/`then` is when you’re working with code that returns `unit`, and you’re
implicitly missing the default branch:

```fsharp
// If/then with implicit default else branch
if customer.Name = "Isaac" then printfn "Hello!"
```

## Lesson 21

### Composition in F#

Моделируют отношение *имеет* (*has-a*) из ООП.

```fsharp
// Defining two record types - Computer is dependent on Disk
type Disk = { SizeGb : int }
type Computer =
    { Manufacturer : string
      Disks: Disk list }

// Creating an instance of a Computer
let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { SizeGb = 100 }
          { SizeGb = 250 }
          { SizeGb = 500 } ] }
```

### Units of measure (UoM) in F#. Example

Specific type of integer: GB and MB, or meters and feet

```fsharp
type Disk = { Size : int<gb> }
```

```fsharp
[<Measure>] type kB

// Single case active pattern to convert from kB to raw Byte value
let (|Bytes|) (x : int<kB>) = int(x * 1024)

// Use pattern matching in the declaration
// val printBytes : int<kB> -> unit
let printBytes (Bytes(b)) = 
    printfn "It's %d bytes" b

printBytes 7<kB>
// "It's 7168 bytes"
```

* Units of measure (UoM) not needed often (but useful).

* UoMs can create a kind of "generic" numerics: so you can have `5<Kilogram>`
as opposed to `5<Meter>`. You can also combine types: `15<Meter/Second>` and so on.

* Compiler will prevent you from accidentally mixing and matching incompatible types.

* UoMs are erased away at compile time, so there’s no runtime overhead.

### Discriminated unions (DU) in F#

Unions моделируют отношение *является* (*is-a*) из ООП. Похожи на `enum` из C#.

```fsharp
// (1) Base type
// (2) Hard Disk subtype, containing two custom fields as metadata
// (3) SolidState - no custom fields
// (4) MMC - single custom field as metadata
type Disk =                             // (1)
| HardDisk of RPM:int * Platters:int    // (2)
| SolidState                            // (3)
| MMC of NumberOfPins:int               // (4)
```

#### Creating discriminated unions in F#:

```fsharp
let myHardDisk = HardDisk(RPM = 250, Platters = 7)  // Explicitly named arguments
let myHardDiskShort = HardDisk(250, 7)              // Lightweight syntax
let myMMC = MMC 5

// Passing all values as a single argument, can omit brackets
let args = 250, 7
let myHardDiskTupled = HardDisk args

// Creating a DU case without metadata 
let mySsd = SolidState
```

### Writing functions for a discriminated union

```fsharp
// (1) Matches on any type of hard disk
// (2) Matches on any type of MMC
let seek disk =
    match disk with
    | HardDisk _ -> "Seeking loudly at a reasonable speed!" // (1)
    | MMC _ -> "Seeking quietly but slowly"                 // (2)
    | SolidState -> "Already found it!"

let mySsd = SolidState
seek mySsd              // Returns “Already found it!”
```

Pattern matching on values:

```fsharp
// (1) Matching a hard disk with 5400 RPM and 5 spindles
// (2) Matching on a hard disk with 7 spindles and binding RPM for usage on the RHS of the case
// (3) Matching an MMC disk with 3 pins
let seek disk =
    match disk with
    | HardDisk(5400, 5) -> "Seeking very slowly!"                       // (1)
    | HardDisk(rpm, 7) -> sprintf "I have 7 spindles and RPM %d!" rpm   // (2)
    | MMC 3 -> "Seeking. I have 3 pins!"                                // (3)
```

### Nested discriminated unions

```fsharp
// Nested DU with associated cases
type MMCDisk =
| RsMmc
| MmcPlus
| SecureMMC

// Adding the nested DU to your parent case in the Disk DU
type Disk =
// ... было ранее
| MMC of MMCDisk * NumberOfPins:int

// Matching on both top-level and nested DUs simultaneously
match disk with
// ... было ранее
| MMC(MmcPlus, 3) -> "Seeking quietly but slowly"
| MMC(SecureMMC, 6) -> "Seeking quietly with 6 pins."
```

### Shared fields in DUs (combination of records and discriminated unions)

```fsharp
// Composite record, starting with common fields
type DiskInfo = {
    Manufacturer : string   // Common field
    SizeGb : int            // Common field
    DiskData : Disk         // DU
}

// Computer record - contains manufacturer and a list of disks
type Computer = { Manufacturer : string; Disks : DiskInfo list }

// Creating a list of disks using [ ] syntax
// Common fields and varying DU as a Hard Disk
let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { Manufacturer = "HardDisks Inc."
            SizeGb = 100
            DiskData = HardDisk(5400, 7) }
          { Manufacturer = "SuperDisks Corp."
            SizeGb = 250
            DiskData = SolidState } ] }
```

### Printing out DUs

Print out the contents of a DU in a human-readable form

```fsharp
sprintf "%A"
```

###  Comparing OO hierarchies and discriminated unions

| -                         | Inheritanc           | Discriminated unions                     |
|---------------------------|----------------------|------------------------------------------|
| Usage                     | Heavyweight          | Lightweight                              |
| Complexity                | Hard to reason about | Easy to reason about                     |
| Extensibility             | Open set of types    | Closed set, compile-time, fixed location |
| Useful for plugin models? | Yes                  | No                                       |
| Add new subtypes          | Easy                 | Update all DU-related functions          |
| Add new methods           | Breaking change      | Easy                                     |

* **Not use** discriminated union for:

  * Plugin models (DU fixed at compile time)
  * Unstable (or rapidly changing) extremely large hierarchies (cases)

* **Use** discriminated union for:

  * Fixed (or slowly changing) set of cases

### Creating an enum in F#

```fsharp
type Printer =      // Enum type
| Injket = 0        // Enum case with explicit ordinal value
| Laserjet = 1
| DotMatrix = 2
```

## Lesson 22

### Mandatory and optional values in C#

| Data type |  Example           | Support for "mandatory" | Support for "optional" |
|-----------|--------------------|-------------------------|------------------------|
| Classes   | String, WebClient  | No                      | Yes                    |
| Structs   | Int, Float         | Yes                     | Partial                |

### The option type

`Option<T>` is a simple two-case discriminated union: `Some (value)` or `None`

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
let descriptionOne customer =
    match customer.Score with
    | Some score -> Some(describe score)
    | None -> None

// Customer -> string option
let descriptionTwo customer =
    customer.Score
    |> Option.map(fun score -> describe score)

// Customer -> string option
let descriptionThree customer =
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

```fsharp
None |> Option.iter(fun n -> printfn "Num = %i" n)      // Нет печати
Some 0 |> Option.iter(fun n -> printfn "Num = %i" n)    // Num = 0
Some 1 |> Option.iter(fun n -> printfn "Num = %i" n)    // Num = 1
```

### `Option.bind` (Binding)

`Option.bind` is more or less the equivalent of `List.collect` (or `SelectMany` in LINQ).

It can flatten an `Option<Option<string>>` to `Option<string>`,
just as `collect` can flatten a `List<List<string>>` to `List<string>`.

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

### `Option.toList`, `Option.toArray`

Takes in an optional value, and if it’s `Some` value, returns a list/array with that single
value in it. Otherwise, it returns an empty list/array.

### `List.choose`

You can think of it as a specialized combination of `map` and `filter` in one.
It allows you to apply a function that might return a value, and then automatically
strip out any of the items that returned `None`.

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

## Lesson 23

### Single-case discriminated unions (DU)

```fsharp
// Creating a single-case DU to store a string Address
type Address = Address of string

// Creating an instance of a wrapped Address
let myAddress = Address "1 The Street"

// Comparing a wrapped Address and a raw string won’t compile
let isTheSameAddress = (myAddress = "1 The Street")

// Unwrapping an Address into its raw string as addressData
let (Address addressData) = myAddress
```

### Combining single-case discriminated unions

```fsharp
// Only one of the contact details should be allowed at any point in time.
type ContactDetails =
| Address of string
| Telephone of string
| Email of string

let customer = createCustomer (CustomerId "Nicki") (Email "nicki@myemail.com")
```

### Using optional values within a domain

Adding an option field for optional secondary contact details:

```fsharp
type Customer =
    { CustomerId : CustomerId
      PrimaryContactDetails : ContactDetails
      SecondaryContactDetails : ContactDetails option }
```

### Creating custom types to represent business states

```fsharp
// Single-case DU to wrap around Customer
type GenuineCustomer = GenuineCustomer of Customer

let validateCustomer customer =
    match customer.PrimaryContactDetails with
    | Email e when e.EndsWith "SuperCorp.com" -> Some(GenuineCustomer customer)
    | Address _ | Telephone _ -> Some(GenuineCustomer customer)
    | Email _ -> None

// The sendWelcomeEmail accepts only a GenuineCustomer as input
let sendWelcomeEmail (GenuineCustomer customer) =
    printfn "Hello, %A, and welcome to our site!" customer.CustomerId

// Usage
unknownCustomer                             // Customer
|> validate                                 // Customer -> GenuineCustomer option
|> Option.map(fun c -> sendWelcomeEmail c)  // unit option
```

### When and when not to use marker types

You can use them for all sorts of things (examples):

* email (verified, unverified)
* order (unpaid, paid, dispatched, or fulfilled)
* data (checked, unchecked)

But be careful not to take it too far, as it can become difficult to wade through a sea of
types if overdone.

### Using `Result` (instead of using exceptions)

F# 4.1 contains a `Result` type built into the standard library.

Creating a result type to encode success or failure:

```fsharp
// Defining a simple Result discriminated union
type Result<'a> =
| Success of 'a
| Failure of string
```

```fsharp
// Type signature of a function that might fail
insertCustomer : contactDetails:ContactDetails -> Result<CustomerId>

// Handling both success and failure cases up front
match insertContact (Email "nicki@myemail.com") with
| Success customerId -> printfn "Saved with %A" customerId
| Failure error -> printfn "Unable to save: %s" error
```

Internally in `insertCustomer`, you’d execute the code in a `try/catch` statement;
any caught errors would be returned as a failure.

### When to use `Result` and exceptions.

* If an error occurs and is something that you *don't* want to reason
about (for example, a catastrophic error that leads to an end of the application),
stick to **exceptions**.

* If it's something that you *do* want to reason about (for example, depending
on success or failure, you want to do some custom logic and then resume processing
in the application), a `Result` type is a useful tool to have.

## Lesson 25

### Consuming C# code from F#

```fsharp
<EntryPoint>]
let main argv =
    let tony = CSharpProject.Person "Tony"      // Calling the Person constructor
    tony.PrintName()                            // Calling the PrintName method
    0
```

### F# script commands

| Directive | Description                               | Example usage               |
|-----------|-------------------------------------------|-----------------------------|
| #r        | References a DLL for use within a script  | #r @"C:\source\app.dll"     |
| #I        | Adds a path to the #r search path         | #I @"C:\source\"            |
| #load     | Loads and executes an F# .fsx or .fs file | #load @"C:\source\code.fsx" |

### Consuming C# assemblies from an F# script

```fsharp
// Referencing the CSharpProject from an F# script.
// Relative referenceswork relative to the script location.
#r @"CSharpProject\bin\debug\CSharpProject.dll"

// Standard F# code to utilize the newly referenced types
open CSharpProject
let simon = Person "Simon"
simon.PrintName()
```

### Treating constructors as functions

```fsharp
open CSharpProject

let longhand =
    [ "Tony"; "Fred"; "Samantha"; "Brad"; "Sophie "]
    |> List.map(fun name -> Person(name))               // Calling a ctor explicitly

let shorthand =
    [ "Tony"; "Fred"; "Samantha"; "Brad"; "Sophie "]
    |> List.map Person                                  // Treating a ctor like a standard function
```

### Working with interfaces. Creating implementation and instance of the interface. Operator `:>`

```fsharp
open System.Collections.Generic

type PersonComparer() =                                         // Class definition with default ctor
    interface IComparer<Person> with                            // Interface header
        member this.Compare(x, y) = x.Name.CompareTo(y.Name)    // Implementation of interface

// Creating an instance of the interface
let pComparer = PersonComparer() :> IComparer<Person>
pComparer.Compare(simon, Person "Fred")      // 1
pComparer.Compare(simon, Person "Simon")     // 0
```

Operator `:>` *explicitly upcast* from your `PersonComparer` type to `IComparer<Person>`

### Using object expressions to create an instance of an interface

Object expressions let you create an instance of an interface without creating an intermediary type.

```fsharp
let pComparer =                         // The type of pComparer here is `IComparer<Person>`
    { new IComparer<Person> with                                    // Interface definition
        member this.Compare(x, y) = x.Name.CompareTo(y.Name) }      // Interface implementation
```

### Nulls, nullables, and options

There are few handy combinators in the `Option` module:

```fsharp
Option.ofObj           // C# (object) -> F# (option)
Option.toObj           // F# (option) -> C# (object)
Option.ofNullable      // C# (nullable) -> F# (option)
Option.toNullable      // F# (option)   -> C# (nullable)
```

```fsharp
open System

// Creating a selection of null and non-null strings and value types
let blank:string = null
let name = "Vera"
let number = Nullable 10


let blankAsOption = blank |> Option.ofObj           // None           // Null maps to None
let nameAsOption = name |> Option.ofObj             // Some "Vera"    // Non-null maps to Some

let numberAsOption = number |> Option.ofNullable    // Some 10
// Options can be mapped back to classes or Nullable types
let unsafeName = Some "Fred" |> Option.toObj        // "Fred"
```

## Lesson 26

В проектах F# работа с NuGet такая же как и в проектах C#.

### Working with NuGet with F# scripts

1. Add the NuGet package to the project.

2. In Solution Explorer get properties of the NuGet DLL, copy the entire path into the clipboard.

3. Code in script:

```fsharp
#r @"<path to Humanizer.dll>"   // Referencing an assembly by using #r
open Humanizer
"ScriptsAreAGreatWayToExplorePackages".Humanize()
```

### Loading source files in scripts

```fsharp
// Referencing the Newtonsoft.Json assembly
#r @"<path to Newtonsoft.Json.dll>"
// Loading the Sample.fs source file into the REPL
#load "Library1.fs"
// Executing code from the Sample module
Library1.getPerson()
```

### Loading a source file into a script with a NuGet dependency

```fsharp
// Add the “..\packages\” folder to the search list by using a relative path.
#I @"..\packages\"
// Simplified NuGet package reference
#r @"Humanizer.Core.2.1.0\lib\netstandard1.0\Humanizer.dll"
#r @"Newtonsoft.Json.9.0.1\lib\net45\Newtonsoft.Json.dll"
```

### Paket

*Paket* - open source, flexible, and powerful dependency management client for .NET.
It's backward-compatible with the NuGet service.

#### Issues with the NuGet

* Invalid references across projects

* Updates project file on upgrade

* Hard to reference from scripts

* Difficulty managing (on large solutions or multiple solution-sharing projects)

#### Benefits of Paket

* Dependency resolver

* Easy to reason about

* Source code dependencies

* Fast, Lightweight

### Get Started with Paket

*From https://fsprojects.github.io/Paket/get-started.html*

1. Install .NET Core 3.0 or higher

2. Install and restore Paket as a local tool in the root of your codebase:

```text
dotnet new tool-manifest
dotnet tool install paket
dotnet tool restore
```

This will create a `.config/dotnet-tools.json` file in the root of your codebase.
It must be **checked into source control**.

3. Initialize Paket by creating a dependencies file.

```text
dotnet paket init
```

If you have a build.sh/build.cmd build script, also make sure you add the last two commands
before you execute your build:

```text
dotnet tool restore
dotnet paket restore
# Your call to build comes after the restore calls, possibly with FAKE: https://fake.build/
```

This will ensure Paket works in any .NET Core build environment.

4. Make sure to add the following entries to your `.gitignore`:

```text
#Paket dependency manager
.paket/
paket-files/
```

### Paket core concepts

*From https://fsprojects.github.io/Paket/learn-how-to-use-paket.html*

Paket manages your dependencies with three core file types:

* `paket.dependencies` - specify your dependencies and their versions for your entire codebase.

* `paket.references` - a file that specifies a subset of your dependencies for every project
in a solution.
  
* `paket.lock` - a lock file that Paket generates when it runs.
When you check it into source control, you get reproducible builds.

You edit the `paket.dependencies` and `paket.references` files by hand as needed.
When you run a paket command, it will generate the `paket.lock` file.

All three file types **must be committed to source control**.

### Important paket commands

The most frequently used Paket commands are:

`paket install` - Run this after adding or removing packages from the `paket.dependencies`
file. It will update any affected parts of the lock file that were affected by the changes
in the `paket.dependencies` file, and then refresh all projects in your codebase that
specify paket dependencies to import references.

`paket update` - Run this to update your codebase to the latest versions of all dependent
packages. It will update the `paket.lock` file to reference the most recent versions
permitted by the restrictions in `paket.dependencies`, then apply these changes to all
projects in your codebase.

`paket restore` - Run this after cloning the repository or switching branches.
It will take the current `paket.lock` file and update all projects in your codebase so that
they are referencing the correct versions of NuGet packages.
It should be called by your build script in your codebase, so you should not need to run it
manually.

### Walkthrough

1. Create a `paket.dependencies` file in your solution's root
(you can also create it by hand):

```text
dotnet paket init
```

2. Installing dependencies. For every project in your codebase,
create a `paket.references` file that specifies the dependencies
you want to pull in for that project.

Once you have a `paket.references` file alongside every project in your codebase,
install all dependencies with this command:

```text
dotnet paket install
```

This will automatically generate the `paket.lock` file.

3. Updating packages

To check if your dependencies have updates you can run command:

```text
dotnet paket outdated
```

To update all packages you can use:

```text
dotnet paket update
```

### Common Paket commands

* `(dotnet) paket convert-from-nuget` - converts the solution from NuGet tooling to Paket.

* `(dotnet) paket simplify` - parses the dependencies and strips out any packages
from the `paket.dependencies` file that aren’t top-level ones.
The `paket.lock` file still maintains the full tree of dependencies.

* `(dotnet) paket update` - updates your packages with the latest versions from NuGet.

* `(dotnet) paket restore` - brings down the current version of all dependencies specified in
the lock file (to ensure repeatable builds in CI).

* `(dotnet) paket add` - add a new NuGet package to the overall set of dependencies

  * Example: `dotnet paket add nuget Automapper project NugetFSharp`

* `paket generate-load-scripts` - generates a set of .fsx files that call `#r` on
all assemblies in a package and their dependencies.

## Lesson 27

### Using F# Records in C#

* A default constructor *won't* normally be generated, so it won’t be possible to create the
record in an uninitialized (or partially initialized) state.

* Each field will appear as a *public getter-only property*.

* The class will implement various interfaces in order to allow *structural equality*
checking.

* Triple-slash comments show in tooltips.

```fsharp
/// A standard F# record of a Car.
type Car = {
    /// The number of wheels on the car.
    Wheels : int
    /// The brand of the car.
    Brand : string
    }
```

```csharp
using Model;

var car = new Car(4, "Supacars", new Tuple<double, double>(1.5, 3.4));
var brand = car.Brand;      // get-only
var wheels = car.Wheels;    // get-only
```

### Using F# Tuples and Discriminated unions (DU) in C#

**Tuples**:

* Tuples in F# are instances of the standard `System.Tuple` type.

* Tuples appear in C# as standard `Item1`, `Item2`, and `ItemN` properties.

* The standard .NET tuple type supports up to *only* 8 items.

* Strongly recommended *avoid more than three items* in tuples.

**Discriminated unions**:

* DU represents class hierarchy in C#:

  * DU as abstract class

  * Types of DU as nested classes

* В C# используется автогенерация методов-фабрик для внутренних классов DU.

* В C# используется автогенерация bool свойств для проверки типа внутреннего класса для cast.

* Чтобы использовать в C# внутренние классы из DU, надо делать cast.

```fsharp
/// A standard F# record of a Car.
type Car = {
    /// The number of wheels on the car.
    Wheels : int
    /// The brand of the car.
    Brand : string
    /// The x/y of the car in meters. (Tuple).
    Dimensions : float * float
    }

/// A vehicle of some sort.
type Vehicle =
    /// A car is a type of vehicle.
    | Motorcar of Car
    /// A bike is also a type of vehicle.
    | Motobike of Name : string * EngineSize : float
```

```csharp
using Model;

// Создание классов-подтипов Vehicle. Vehicle виден как abstract класс.
Car carItem = new Car(4, "Unknown", new Tuple<double, double>(1.7, 4));
Vehicle motorcar = Vehicle.NewMotorcar(carItem);
Vehicle motorbike = Vehicle.NewMotobike("Motor", 1.2);

// Использование motorcar.
if (motorcar.IsMotorcar)    // Проверка. IsMotorcar сгенеренное get-only свойство.
{
    var castedMotorcar = (Vehicle.Motorcar) motorcar;     // cast
    Car innerCar = castedMotorcar.Item;                   // get-only
    string innerCarBrand = innerCar.Brand;                // get-only
    int motorcarTag = castedMotorcar.Tag;                 // 0

// Использование motorbike.
if (motorbike.IsMotobike)    // Проверка. IsMotobike сгенеренное get-only свойство.
{
    var castedMotobike = (Vehicle.Motobike) motorbike;          // cast
    string motobikeName = castedMotobike.Name;                  // get-only
    double motobikeEngineSize = castedMotobike.EngineSize;      // get-only
    int motobikeTag = castedMotobike.Tag;                       // 1
}
```

### Using F# namespaces and modules in C#

#### F# namespaces in C#

Эквивалентны.

#### Modules in C#

* A module is rendered in C# as a static class.

* Any simple values on the module such as an integer or a record value will show
as a public property.

* Functions will show as methods on the static class.

* Types will show as nested classes within the static class.

### Using F# functions in C#

```fsharp
module Functions =
    /// Creates a car
    let createCar wheels brand x y =
        { Wheels = wheels; Brand = brand; Dimensions = x, y }

    /// Creates a car with four wheels. (Curried function).
    let createsFourWheeledCar = createCar 4
```

```csharp
using Model;

// Использование функции из F#.
// Calling a standard F# function from C#
var someCar = Functions.createCar(4, "SomeBrand", 1.5, 3.5);
// Calling a partially applied F# function from C#
var fourWheeledCar = Functions.createsFourWheeledCar
    .Invoke("Supacars")
    .Invoke(1.5)
    .Invoke(3.5);
```

*Recommendation*: try to avoid providing partially applied functions to C#.
If you absolutely must, wrap such functions in a "normal" F# function that
explicitly takes in all arguments required by the partially applied version,
and supplies those arguments manually.

### Summarizing F# to C# interoperability

| Element              | Render as                    | C# compatibility |
|----------------------|------------------------------|------------------|
| Records              | Immutable class              | High             |
| Tuples               | System.Tuple                 | Medium/high      |
| Discriminated unions | Classes with builder methods | Medium/low       |
| Namespaces           | Namespaces                   | High             |
| Modules              | Static classes               | High             |
| Functions            | Static methods               | High/medium      |

### F# incompatible types in C#

* Unit of measure

* Type providers

### Create an F# record from C# in an uninitialized state. (Use CLI Mutable attribute)

Сreate an F# record from C# in an uninitialized state.

Place the `[<CLIMutable>]` attribute on a record.

```fsharp
[<CLIMutable>]
type Person = {
    Name : string
    Age : int
    }
```

```csharp
// Создание и использование Record в неинициализированном состоянии.
var nonInitPerson = new Person();   // default сгенеренный конструктор
var name = nonInitPerson.Name;      // null
var age = nonInitPerson.Age;        // 0

var initPerson = new Person("Sam", 18);     // Второй сгенеренный конструктор
var knownName = initPerson.Name;
var knownAge = initPerson.Age;
```

### Using F# Option in C#

You can consume F# option types in C#. But as with other discriminated unions,
they're not particularly idiomatic to work with in C#.

Adding a few well-placed extension methods that remove the need for supplying type
arguments can help.

### Accessibility modifiers in F#

F# supports accessibility modifiers:

* `public`

* `private`

* `internal`

* All things are *public* by default in F#

* If you want to make a function or value hidden from C# code, mark it as `internal`.

### F# Collections in C#

* F# `array` is standard .NET array.

* F# `seq` appear as `IEnumerable<T>` in C# code.

* F# `list` not fully compatible with C#. In C# `list` can be used as `IEnumerable<T>`.

* C# `List` appear as `ResizeArray` in F# code.

*Advice*: avoid exposing F# `list` to C# clients.

## Lesson 28

### Accepting data from external systems

```text
Unsafe   |                 F# World                 (Safe)
external |
systems  |                                        | Bounded Context
         |                     Validation         |
    C# --|--> Simple API ----> (Transformation) --|--> F#
         |                     Layer              |
```

It’s relatively simple to go from a weaker model to a stronger model.
At the entrance to your F# module, you accept the weak model,
but immediately validate and transform it over to your stronger model.

#### 1. A simple domain model for use within C# (on scheme: Simple API)

Set of implicit rules that are documented through code comment.
Weaker model.

```fsharp
type OrderItemRequest = { ItemId : int; Count : int }
type OrderRequest =
    { OrderId : int
      CustomerName : string    // mandatory
      Comment : string         // optional
      /// One of (email or telephone), or none
      EmailUpdates : string
      TelephoneUpdates : string
      Items : IEnumerable<OrderItemRequest> }
```

#### 2. Modeling the same domain in F# (on scheme: Bounded Context)

```fsharp
type OrderId = OrderId of int
type ItemId = ItemId of int
type OrderItem = { ItemId : ItemId; Count : int }
type UpdatePreference =
    | EmailUpdates of string
    | TelephoneUpdates of string
type Order =
    { OrderId : OrderId
      CustomerName : string
      ContactPreference : UpdatePreference option
      Comment : string option
      Items : OrderItem list }
```

#### 3. Validating and transforming data (on scheme: Validation/Transformation layer)

Perform checks before entering your "safe" F# world:

* Null check on a string.

* Convert from a string to an optional string.

* Confirm that the source request has a valid state;
if the incorrect mix of fields is populated, the request is rejected.

```fsharp
let validate orderRequest =
    { CustomerName =
          match orderRequest.CustomerName with
              | null -> failwith "Customer name must be populated"
              | name -> name
      Comment = orderRequest.Comment |> Option.ofObj
      ContactPreference =
          var emailUpd = Option.ofObj orderRequest.EmailUpdates
          var telephoneUpd = Option.ofObj orderRequest.TelephoneUpdates
          match emailUpd, telephoneUpd with
              | None, None -> None
              | Some email, None -> Some(EmailUpdates email)
              | None, Some phone -> Some(TelephoneUpdates phone)
              | Some _, Some _ ->
                  failwith "Unable to proceed - only one of telephone and email should be supplied" }
```

#### Working with strings in F#

* If string can be null:

  * Convert it to an option type by using `Option.ofObj`.

* If string shouldn't ever be null:

  * Check at the F# boundary.

  * Reject the object if it's null.

  * If it's not null, leave it as a string.

### C# interoperability example

Специальный класс в F#. Именно с экземплярами этого класса работает C#.

Особенности:

* Имена методов с большой буквы (как в коде C#).

* `CLIEvent` attribute, which is needed to expose events to the C# world.
Unfortunately, you can't have CLIEvents on modules (only on namespace).

```fsharp
namespace Monopoly

// Some modules using by Controller

/// Manages a game.
type Controller() =
    let onMovedEvent = new Event<MovementEvent>()
    
    (* A CLI Event is an event that can be consumed by e.g. C# *)
    /// Fired whenever a move occurs.
    [<CLIEvent>]
    member __.OnMoved = onMovedEvent.Publish
    
    /// Plays the game of Monopoly
    member __.PlayGame turnsToPlay =
        let random = Random()
        let rollDice = Functions.createDiceThrow random
        let pickCard = Functions.createPickCard random
        Functions.playGame rollDice pickCard onMovedEvent.Trigger turnsToPlay
```

#### Calling F# from a C# view mode

```csharp
// Creating an instance of the F# controller class
var controller = new Controller();
// Adding a standard event handler to capture game events
controller.OnMoved += (o, e) =>
    positionLookup[e.MovementData.CurrentPosition.ToString()Increment();
// Having the F# code play 50,000 turns on a background thread
Task.Run(() => controller.PlayGame(50000)
```

#### Using deterministic functions for exploration

```fsharp
let rollDice() = 3, 4       // Always roll 3, 4.
let pickCard() = 5          // Always pick card 5.
```

## Lesson 29

### Shown XML comments (from F# triple-slash declarations) outside the F# library

Ensure that you have the *XML Documentation File* selected in the *Build* tab of the *Properties*
pane of the F# project.

### Creating a member field on a discriminated union (DU)

Полезно при использовании в C#. Иногда полезно (но не обязательно) в F#.

```fsharp
type RatedAccount =
    | InCredit of CreditAccount
    | Overdrawn of Account
    member this.Balance =       // Member declaration
        match this with         // Self-matching to access nested fields
        | InCredit (CreditAccount account) -> account.Balance
        | Overdrawn account -> account.Balance

    // Или. Более универсальный способ получения значения какого-либо поля Account
    // (Account -> 'a)
    member this.GetField getter =
    match this with
    | InCredit (CreditAccount account) -> getter account
    | Overdrawn account -> getter account

// Пример. Использование универсального поля
let accountId = loadedAccount.GetField(fun a -> a.AccountId)
```

### Encapsulation

`internal` or `private` на module ограничивают область его видимости
(полезно для пользователей библиотеки на F#).

Пример:

```fsharp
module internal Capstone5.Operations
```

### Naming conventions. Attribute `[<CompiledName>]`

Для API модулей F#, которые видны и вызываются из C#, принято использовать наименование
с *большой* буквы (Pascal).

Поэтому:

* Либо функции в API на F# писать с большой буквы.

* Либо использовать на функциях атрибут `[<CompiledName>]`, to change the
function name post-compile.

Пример:

```fsharp
[<CompiledName "ClassifyAccount">]
```

## Lesson 30

### Working with CSV files using FSharp.Data

#### Working with CSV files from scripts using Paket and `CsvProvider`

1. Добавить Paket в переменную окружения PATH (требуется единожды)

2. Запустить build.cmd (требуется единожды, при создании новой директории со скриптом)

3. Можно работать со скриптом. Для загрузки `*.csv` используется `CsvProvider`,
из `FSharp.Data` namespace.

```text
paket.bootstrapper                      // Скачивает (если требуется) новую версию paket
paket init                              // Инициализация (если требуется) paket
paket add FSharp.Data                   // Добавление nuget-пакетов в paket
paket add XPlot.GoogleCharts
paket add Google.DataTable.Net.Wrapper  // (?) Возможно, добавится при добавлении XPlot.GoogleCharts
paket generate-load-scripts             // Формирует файлы *.fsx для загрузки nuget-пакетов.
```

```fsharp
#I @"..\.paket\load\netstandard2.0"
// Referencing the FSharp.Data assembly
#load "FSharp.Data.fsx"
#load "XPlot.GoogleCharts.fsx"

open FSharp.Data
open XPlot.GoogleCharts

// Для CsvProvider в скриптах надо указывать префикс FSharp.Data,
// иначе пишет "CsvProvider not found" (? - глюк Intellisence)
// Connecting to the CSV file to provide types based on the supplied file
type Football = FSharp.Data.CsvProvider< @"E:\Temp\FootballResults.csv">
// Loading in all data from the supplied CSV file
let data = Football.GetSample().Rows |> Seq.toArray

// Select row
data.[0].``Full Time Home Goals``.ToString()

// Select data with visualizing data
data
|> Seq.filter(fun row ->
    row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
// countBy generates a sequence of tuples (team vs. number of wins).
|> Seq.countBy(fun row -> row.``Home Team``)
|> Seq.sortByDescending snd
|> Seq.take 10
// Converting the sequence of tuples into an XPlot Column Chart
|> Chart.Column
// Showing the chart in a browser window
|> Chart.Show
```

### Backtick members ``

Установка Double backtick (знак `` ) в начале и конце member definition позволяет указывать
в имени member буквы, пробелы, цифры и прочие символы (см. предыдущий пример).

## Lesson 31

### Data Providers. Opening a remote JSON data source with `JsonProvider`

```fsharp
// Referencing FSharp.Data
#r @"..\..\packages\FSharp.Data\lib\net40\FSharp.Data.dll"
open FSharp.Data
// Creating the TVListing type based on a URL
type TvListing =
  JsonProvider<"http://www.bbc.co.uk/programmes/genres/comedy/schedules/upcoming.json">
// Creating an instance of the type provider
let tvListing = TvListing.GetSample()
let title = tvListing.Broadcasts.[0].Programme.DisplayTitles.Title
```

### Data Providers. Opening HTML data source with `HtmlProvider`

Show the number of films acted in over time by Robert DeNiro from Wikipedia.

```fsharp
open FSharp.Data
open XPlot.GoogleCharts

type Films = FSharp.Data.HtmlProvider<"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">
let deNiro = Films.GetSample()
deNiro.Tables.FilmsEdit.Rows
|> Array.countBy(fun row -> string row.Year)
|> Chart.SteppedArea
|> Chart.Show
```

### Examples of live schema type providers

* *JSON type provider* - Provides a typed schema from JSON data sources
* *HTML type provider* - Provides a typed schema from HTML documents
* *Swagger type provider* - Provides a generated client for a Swagger-enabled HTTP
endpoint, using Swagger metadata to create a strongly typed model
* *Azure Storage type provider* - Provides a client for blob/queue/table storage assets
* *WSDL type provider* - Provides a client for SOAP-based web services

###  Avoiding problems with live schemas

* Large data sources

  Объем данных может быть слишком большим (500 MB и более).

* Inferred schemas

  (CSV файл с нужным полем, которое заполнено только в 9 999 строке).

* Priced schemas

  Некоторые ресурсы взымают деньги за доступ к данным.

* Connectivity

  Требуется постоянное соединение с ресурсом для генерации типов.

### Redirecting type providers to new data. Использование `Load` в type provider'ах.

Идея: используется локальный файл данных (обычно хранится в системе управления версиями)
как часть исходного кода во время компиляции. Этот файл данных представляет схему
и используется type provider'ом для генерации типов.

Во время runtime можно переключиться на реальный источник данных.

Пример. Подсчет количества скачиваний для трех nuget-пакетов:

```fsharp
// Using local file to create scheme for type provider
type Package = HtmlProvider< @"..\data\sample-package.html">

// Load in data from a live URI
let nunit = Package.Load "https://www.nuget.org/packages/nunit"
let entityFramework = Package.Load "https://www.nuget.org/packages/entityframework"
let newtonsoftJson = Package.Load "https://www.nuget.org/packages/newtonsoft.json"

// Creating a list of package statistics values
[entityFramework; nunit; newtonsoftJson]
// Merging all rows from each package into a single sequence
|> Seq.collect(fun package -> package.Tables.``Version History``.Rows)
|> Seq.sortByDescending(fun package -> package.Downloads)
//...
```

## Lesson 32

### SqlClient

SqlClient - data access layer (Micro ORM) designed specifically for MSSQL.
NuGet package `FSharp.Data.Sql`.

#### Querying data with the `SqlCommandProvider`

* `[<Literal>]` attribute mark connection string value as a *compile-time constant*,
which is needed when passing values as arguments to type providers.

* Название `Conn` с большой буквы, т.к. это compile-time constant.

* В качестве SQL запросов можно использовать помимо SELECT запросов более сложные:
  * Joins
  * Common table expressions
  * Stored procedures—even table valued functions.

* Но SqlClient поддерживает не все команды SQL (подробности см. в официальной документации).

```fsharp
// A standard SQL connection string
let [<Literal>] Conn =
    "Server=(localdb)\MSSQLLocalDb;Database= AdventureWorksLT;Integrated Security=SSPI"
// Creating a strongly typed SQL command
type GetCustomers =
    SqlCommandProvider<"SELECT * FROM SalesLT.Customer", Conn>
// Executing the command to return a dataset
let customers =
    GetCustomers.Create(Conn).Execute() |> Seq.toArray
// Get record from dataset
let customer = customers.[0]
```

Запрос с параметром:

```fsharp
type GetProductCategory =
    SqlCommandProvider<"SELECT * FROM SalesLT.ProductCategory WHERE Name = @Name", Conn>
let findProductCatergory productName =
    GetProductCategory.Create(Conn).Execute(productName) |> Seq.toArray
```

#### Inserting data. `SqlProgrammabilityProvider`

* `Update()` - create a DataAdapter and the appropriate insert command.

* `BulkInsert()` - insert data by using SQL Bulk Copy functionality.
Extremely efficient and great for large one-off inserts of data.

* You can also use the data table for updates and deletes, or via T-SQL commands.

```fsharp
type AdventureWorks = SqlProgrammabilityProvider<Conn>
type ProductCategory = AdventureWorks.SalesLT.Tables
// Get table from db
let productCategory = new AdventureWorks.SalesLT.Tables.ProductCategory()
// Inserting data into the table
productCategory.AddRow("Mittens", Some 3, Some (Guid.NewGuid()), Some DateTime.Now)
productCategory.AddRow("Long Short", Some 3, Some (Guid.NewGuid()), Some DateTime.Now)
productCategory.AddRow("Wooly Hats", Some 4, Some (Guid.NewGuid()), Some DateTime.Now)
// Create a DataAdapter and insert data
productCategory.Update()
```

#### Working with reference data. `SqlEnumProvider`

Reference data - static (or relatively stable) sets of lookup data: categories, country lists,
regions that need to be referenced both in code and data.

You’ll normally have a C# enum and/or class with constant values
that matches a set of items scripted into a database.

```fsharp
// Generating a Categories type for all product categories
type Categories =
    SqlEnumProvider<"SELECT Name, ProductCategoryId FROM SalesLT.ProductCategory", Conn>
// Accessing the Wooly Hats integer ID
let woolyHats = Categories.``Wooly Hats``
printfn "Wooly Hats has ID %d" woolyHats
```

### SQLProvider

SQLProvider - ORM. Can work with many ODBC data sources (MSSQL, Oracle, SQLite, Postgres, MySQL, ...).
NuGet package `SQLProvider` (плюс, мне потребовалась ссылка на nuget `System.Data.SqlClient`).

#### Querying data

* Query expressions can be used in F# over any `IQueryable` data source,
so you can use them anywhere you'd write a LINQ query in C#.

* `query { }` expressions are another form of computation expression,
similar to `seq { }` and `async { }`

```fsharp
open FSharp.Data.Sql

let [<Literal>] Conn =
    "Server=(localdb)\MSSQLLocalDB;Database=AdventureWorksLT;Integrated Security=SSPI"

// Creating an AdventureWorks type by using the SqlDataProvider
// UseOptionTypes=true - generate option types for nullable columns
// UseOptionTypes=false - default value will be generated for nullable columns
type AdventureWorks = SqlDataProvider<ConnectionString = Conn, UseOptionTypes = true>
// Getting a handle to a sessionized data context
let context = AdventureWorks.GetDataContext()
// Writing a query against the Customer table (get the first 10 customers)
let customers =
    query {
        for customer in context.SalesLt.Customer do
        take 10
    } |> Seq.toArray

// Get customer
let customer = customers.[0]
```

More complex query:

```fsharp
// More complex query
let customersFromFriendlyBikeShop =
    query {
        for customer in context.SalesLt.Customer do
        where (customer.CompanyName = Some "Friendly Bike Shop")   // A filter condition
        select (customer.FirstName, customer.LastName)             // Map to tuples as result
        distinct    // Selecting a distinct list of results
    }
```

#### Inserting data

Adding data to the database is simple:

1. Create new entities through the data context (`Create()`)
2. Set properties (with `<-`)
3. Save changes with (`SubmitUpdates()`)

```fsharp
// Creating a new entity attached to the ProductCategory table
let category = context.SalesLt.ProductCategory.Create()
// Mutating properties on the entity
category.ParentProductCategoryId <- Some 3
category.Name <- "Scarf"
// Calling SubmitUpdates to save the new data
context.SubmitUpdates()
```

Примечания:

* All entities track their own states and have a `_State` property on them.
* On create a new entity, you'll see that its initial state is `Created`.
* After calling `SubmitUpdates()`, its state changes to `Unchanged`.
* Updates are performed by first loading the data from the database, mutating
the records, and then calling `SubmitChanges()`.

#### Working with reference data

* Every table on the context has a property `Individuals`, which
will generate a list of properties that match the rows in the database - essentially the same
as the Enum Provider.

* You also have subproperties underneath that allow you to choose
which column acts as the "text" property (for example, `As Name` or `As ModifiedDate`).

Example:

```fsharp
let mittens =
        context.SalesLt.ProductCategory     // Table
            .Individuals
            .``As Name``                    // Text
            .``42, Mittens``                // Selected row

// Using
printfn "%s %O" mittens.Name mittens.ModifiedDate   // Mittens 11.03.2021 23:14:36
```

## Lesson 33

### Reasons for not exposing provided types over an API

* Данные, предоставляемые type provider'ом не всегда совпадают с бизнес-моделью (доменной моделью).

* Данные, создаваемые type provider'ом не являеются record или discriminated union,
что ограничивает их использование в коде.

* Данные, создаваемые type provider'ом не могут быть использованы "снаружи" F# (например, из
других проектов на C#).

* Данные, создаваемые type provider'ом чаще всего затираются во время runtime'а.

## Lesson 34

### Supplying connection details to a type provider via config

`app.config`:

```text
<configuration>
    <connectionStrings>
        <add name="AdventureWorks"
             connectionString="Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"/>
    </connectionStrings>
</configuration>
```

Using with SqlClient:

```fsharp
// Supplying the connection string name (AdventureWorks) to the SQL Client type provide
type GetCustomers = SqlCommandProvider<"SELECT TOP 50 * FROM SalesLT.Customer", "name=AdventureWorks">

[<EntryPoint>]
let main _ = 
    // Removed Conn value frome Create() call.
    let customers = GetCustomers.Create();
    customers.Execute()
    |> Seq.iter (fun c -> printfn "%A: %s %s" c.CompanyName c.FirstName c.LastName)
    0
```

### Separating retrieval of live connection string from application code.

*Переопределение connection string в runtime.*

Единственный способ использовать connection string в скриптах
(у меня в скрипте не заработало).

1. Создание type provider - с использованием hardcoded строки соединения `[<Literal>]`.
Файл `CustomerRepository.fs`:

```fsharp
let [<Literal>] private CompileTimeConnection =
    "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
type private GetCustomers =
    SqlCommandProvider<"SELECT TOP 50 * FROM SalesLT.Customer", CompileTimeConnection>

let printCustomers(runtimeConnection:string) =
  let customers = GetCustomers.Create(runtimeConnection)
  customers.Execute()
  |> Seq.iter (fun c -> printfn "%A: %s %s" c.CompanyName c.FirstName c.LastName)
```

2. Использование type provider с переопределением строки соединения из файла `app.config`.
Файл `Program.fs`:

```fsharp
open System.Configuration

[<EntryPoint>]
let main argv =
    let runtimeConnectionString =
        ConfigurationManager
            .ConnectionStrings
            .["AdventureWorks"]     // Connection string name in the app.config file
            .ConnectionString
    CustomerRepository.printCustomers(runtimeConnectionString)  // Usage
    0
```

3. Скрипт `DataAccessThroughScript.fsx` (у меня не заработал):

```fsharp
#I "C:/Users/USER_NAME/.nuget/packages"
#r "fsharp.data.sqlclient/2.0.7/lib/netstandard2.0/FSharp.Data.SqlClient.dll"
#load "CustomerRepository.fs"

// Нужная нам строка соединения с БД
let scriptConnectionString =
    "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
// Запрос к БД
CustomerRepository.printCustomers(scriptConnectionString)
```

### Configuring type providers

|Compile time   | Runtime           | Effort    | Best for                                    |
|---------------|-------------------|-----------|---------------------------------------------|
|Literal values | Literal values    | Very easy | Simple systems, scripts, fixed data sources |
|app.config     | app.config        | Easy      | Simple redirection, improved security       |
|Literal values | Function argument | Medium    | Script drivers, large teams, full control   |

## Lesson 36

### Several ways of performing background work in .NET

* *Thread* - lowest primitive for allocating background work.

* *Task* - higher-level abstraction over thread.

* *I/O-bound workloads* - background tasks that you want to execute that don’t need a thread
to run on. These are typically used when communicating and waiting for work from an external
system to complete

Examples of I/O- and CPU-bound workloads:

| Type | Example                                           |
|------|---------------------------------------------------|
| CPU  | Calculating the average of a large set of numbers |
| CPU  | Running a set of rules over a loan application    |
| I/O  | Downloading data from a remote web server         |
| I/O  | Loading a file from disk                          |
| I/O  | Executing a long-running query on SQL             |

### async block in F#

Example:

```fsharp
// A conventional, synchronous sequential set of instructions
printfn "Sync. Loading data!"
System.Threading.Thread.Sleep(5000)
printfn "Sync. Loaded Data!"
printfn "Sync. My name is Simon 1."

// Wrapping a portion of code in an async block
async {
    printfn "Async. Loading data!"
    System.Threading.Thread.Sleep(5000)
    printfn "Async. Loaded Data!" }
|> Async.Start      // Starting the async block in the background

printfn "Sync. My name is Simon 2."
```

Output:

```text
Sync. Loading data!
Sync. Loaded Data!
Sync. My name is Simon 1.
Sync. My name is Simon 2.
Async. Loading data!
Async. Loaded Data!
```

### Returning the result from an async block

* Result of an async expression must be prefixed with the `return` keyword.

* Unwrap an `Async<_>` value by calling `Async.RunSynchronously` (грубое подобие `Task.Result`).

* `async` block doesn’t automatically start the work. To start it call
`RunSynchronously` or `Start`.

* Every call `RunSynchronously` on an async block, it will re-execute the code every time.

```fsharp
// Returning a value from an async block
let asyncHello : Async<string> = async { return "Hello" }       // val asyncHello : Async<string>

// Compiler error when trying to access a property of an async value
let length = asyncHello.Length      // <-- Error

// Executing and unwrapping an asynchronous block on the current thread
let text = asyncHello |> Async.RunSynchronously                 // val text : string = "Hello"
let lengthTwo = text.Length                                     // val lengthTwo : int = 5
```

More complex example

```fsharp
open System.Threading

let printThread text = printfn "THREAD %d: %s" Thread.CurrentThread.ManagedThreadId text

let doWork() =
    printThread "Starting long running work!"           // 5
    Thread.Sleep 5000
    "HELLO"

let asyncLength : Async<int> =
    printThread "Creating async block"                  // 1
    let asyncBlock =
        async {
            printThread "In block!"                     // 4
            let text = doWork()                         // 5
            return (text + " WORLD").Length }
    printThread "Created async block"                   // 2
    asyncBlock

printThread "Run async block"                           // 3
let length = asyncLength |> Async.RunSynchronously
printThread "Done!"                                     // 6
printfn "Length: %i" length                             // 7
```

Output:

```text
THREAD 1: Creating async block
THREAD 1: Created async block
THREAD 1: Run async block
THREAD 7: In block!
THREAD 7: Starting long running work!
THREAD 1: Done!
Length: 11
```

### Creating a continuation by using `let!`

* `let!` - is valid only when inside the `async` block (you can't use it outside)

* `let!` waits for `asyncWork` to complete in the background
(it doesn't block a thread), unwraps the value and then continues.

* `Async.Start` - perfect if you want to start the task that has no specific end result.

* Value `text` is a type string.

* `async` blocks also allow you to perform `try/with` blocks around a `let!` computation;
you can nest multiple computations together and use .NET IDisposables without a problem.

```fsharp
let getTextAsync = async { return "HELLO" }
let printHelloWorld =
    async {
        // Using the let! keyword to asynchronously unwrap the result
        let! text = getTextAsync
        // Continuing work with the unwrapped string
        return printf "%s WORLD" text }

// Starting the entire workflow in the background
printHelloWorld |> Async.Start
```

### Using fork/join with `Async.Parallel`

* `Async.Parallel` объединяет коллекцию асинхронных рабочих процессов в один
комбинированный процесс.

* `Async.Parallel` similar to `Task.WhenAll`

Example 1:

```fsharp
let random = System.Random()
let pickANumberAsync =                                          // Async<int>
    async { return random.Next(10) }
let createFiftyNumbers =                                        // Async<unit>
    // Creating 50 asynchronous computations
    let workflows = [ for i in 1 .. 50 -> pickANumberAsync ]    // Async<int> list
    async {
        // Executing all computations in parallel and unwrapping the collated results
        let! numbers = workflows |> Async.Parallel    // Async<int> list |> Async<int[]> -> int[]
        printfn "Total is %d" (numbers |> Array.sum) }

createFiftyNumbers |> Async.Start
```

Example 2. Asynchronously downloading data over HTTP in parallel.

```fsharp
let downloadData (url : string) = async {   // string -> Async<int>
        let wc = new WebClient()
        printfn "Downloading data on thread %d" Thread.CurrentThread.ManagedThreadId
        let! data = wc.AsyncDownloadData(Uri url)
        return data.Length }

let downloadBytes urls =                    // string[] -> int[]
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.RunSynchronously

let printResult downloadedBytes =           // int[] -> unit
    printfn "You downloaded %d characters" (Array.sum downloadedBytes)

[|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]
|> downloadBytes
|> printResult
```

###  Interoperating with `Task`

* `Async.AwaitTask` converts a task into an async workflow.
* `Async.StartAsTask` converts an async workflow into a task.

Example:

```fsharp
let downloadData (url : string) = async {   // string -> Async<int>
    let wc = new WebClient()
    // Using the AwaitTask combinator to convert from Tasks to Async
    let! data = wc.DownloadDataTaskAsync(Uri url) |> Async.AwaitTask
    return data.Length }

let downloadBytes urls =                    // string[] -> Task<int[]>
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.StartAsTask    // Using the StartAsTask combinator to convert from Async to Task

let printResult (downloadedBytes : Task<int[]>)=
    printfn "You downloaded %d characters" (Array.sum downloadedBytes.Result)

[|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]
|> downloadBytes
|> printResult
```

### Comparing tasks and async

In F# code recommended to use async workflows wherever possible.

| -                       | Task and async await                      | F# async workflows
|-------------------------|-------------------------------------------|--------------------
| Native support in F#    | Via async combinators                     | Yes
| Allows status reporting | Yes                                       | No
| Clarity                 | Hard to know where async starts and stops | Very clear
| Unification             | `Task` and `Task<T>` types                | Unified `Async<T>`
| Statefulness            | Task result evaluated only once           | Infinite

### Useful async keywords

| Command                | Usage
|------------------------|-------------------------------------------------------------------
| `let!`                 | Used within an `async` block to unwrap an `Async<T>` value to `T`
| `do!`                  | Used within an `async` block to wait for an `Async<unit>` to complete
| `return!`              | Used within an `async` block as a shorthand for `let!` and `return`
| `Async.AwaitTask`      | Converts `Task<T>` to `Async<T>`, or `Task` to `Async<unit>`
| `Async.StartAsTask`    | Converts `Async<T>` to `Task<T>`
| `Async.RunSychronously`| Synchronously unwraps `Async<T>` to `<T>`
| `Async.Start`          | Starts an `Async<unit>` computation in the background (fire-and-forget)
| `Async.Ignore`         | Converts `Async<T>` to `Async<unit>`
| `Async.Parallel`       | Converts `Async<T>` array to `Async<T array>`
| `Async.Catch`          | Converts `Async<T>` into a two-case DU of `T` or `Exception`

### Handle an exception raised in an async block by using the Async.Catch

*(External resource)*:

```fsharp
// exception handling in async using Async.Catch
let fetchAsync (name, url:string) =
    async {
        let uri = new System.Uri(url)
        let webClient = new WebClient()
        let! html = Async.Catch (webClient.AsyncDownloadString(uri))
        match html with
        | Choice1Of2 html -> printfn "Read %d characters for %s" html.Length name
        | Choice2Of2 error -> printfn "Error! %s" error.Message
    } |> Async.Start

// exception handling in async using regular try/with
let fetchAsync2 (name, url:string) =
    async {
        let uri = new System.Uri(url)
        let webClient = new WebClient()
        try
            let! html = webClient.AsyncDownloadString(uri)
            printfn "Read %d characters for %s" html.Length name
        with error -> printfn "Error! %s" error.Message
    } |> Async.Start

fetchAsync2 ("blah", "http://asdlkajsdlj.com")
```

## Other F# Language Features

### Object-oriented support. Basic classes

```fsharp
// new : age:int * firstname:string * surname:string -> Person
// Type definition with public constructor
type Person(age, firstname, surname) =
    // Private field based on constructor args
    let fullName = sprintf "%s %s" firstname surname

    // Public method
    member __.PrintFullName() =
        printfn "%s is %d years old" fullName age

    // Public getter-only property
    member this.Age = age
    // Public getter-only property
    member that.Name = fullName
    // Mutable, public property
    member val FavouriteColour = "Green" with get, set

// Usage
let person = Person(12, "Ivan", "Ivanov")   // ctor
let name = person.Name                      // "Ivan Ivanov"
let age = person.Age                        // 12
let colour = person.FavouriteColour         // "Green"
person.FavouriteColour <- "Blue"            // Set FavouriteColour to "Blue"
person.PrintFullName()                      // "Ivan Ivanov is 12 years old"
```

* Member methods and properties can access constructor arguments without the need to set them
as private backing fields first.

* If a member doesn’' need to access other members, you can omit the `this` and
replace it with `_`.

* (Not common practice) you can also place members on records and discriminated unions.

### Object-oriented support. Interfaces

```fsharp
// Defining an interface in F#
type IQuack =
    abstract member Quack : unit -> unit

// Creating a type that implements an interface
type Duck (name:string) =
    interface IQuack with
        member this.Quack() = printfn "QUACK!"

// Create object, cast it to the interface, use interface method
let duck = Duck "Donald"
let quackableDuck = duck :> IQuack
quackableDuck.Quack()                   // "QUACK!"

// 2. Creating an instance of an interface through an object expression
let quacker =
    { new IQuack with
        member this.Quack() = printfn "What type of animal am I?" }

// Use interface method
quacker.Quack()                         // "What type of animal am I?"s
```

### Object-oriented support. Inheritance

* Mark the abstract class (type) with the `[<AbstractClass>]` attribute.

```fsharp
// Creating an abstract class
[<AbstractClass>]
type Employee(name:string) =
    // Public property, get-only
    member __.Name = name
    // Defining an abstract method
    abstract member Work : unit -> string
    // Calling an abstract method from a base class
    member this.DoWork() =
        printfn "%s is working hard: %s!" name (this.Work())

type ProjectManager(name:string) =
    // Defining an inheritance hierarchy
    inherit Employee(name)
    // Overriding a virtual or abstract method
    override this.Work() = "Creating a project plan"

let manager = ProjectManager "Peter"    // ctor
manager.Work()                          // "Creating a project plan"
manager.DoWork()                        // "Peter is working hard: Createing a project plan!"
```

### Exception handling

* Старайтесь избегать исключений как способа передачи сообщений и используйте их только в
действительно исключительных случаях.

* F# use pattern matching on the type of exception in order to create separate handlers.

```fsharp
// Throwing a specific exception by using the raise() function
let riskyCode() =
    raise(ApplicationException("From risky code!"))
    ()

let runSafely() =
    try
    riskyCode()     // Placing code within a try block
    with
    // Multiple catch handlers based on different Exception subtypes
    | :? ApplicationException as ex -> printfn "App exception! %O" ex
    | :? MissingFieldException as ex -> printfn "Missing field! %O" ex
    | ex -> printfn "Got some other type of exception! %O" ex

runSafely()
// App exception! System.ApplicationException: From risky code!
```

### Resource management

F# has two ways to work with automatic disposal of objects:

* `use` keyword

* `using` block

```fsharp
// A function that creates a disposable object
let createDisposable() =
    printfn "Created!"
    { new IDisposable with member __.Dispose() = printfn "Disposed!"}

// The use keyword with implicit disposal of resources
let foo() =
    use x = createDisposable()
    printfn "inside!"

// The using keyword with explicit disposal of resources
let bar() =
    using (createDisposable()) (fun disposableObject -> printfn "inside!")

foo()
bar()
// Created!         <-- Оба метода выведут это
// inside!
// Disposed!
```

### Casting

```text
             Object              ^  upcast ( :> )
               ^                 |
               |
   -------------------------     |  downcast ( :?> )
   |                       |     V
String  <------------  Exception
           Illegal!        ^
                           |
                  ApplicationException
```

* Upcast `:>` - **safely** upcast to a parent type in the type hierarchy.

* Downcast `:?>` - unsafely downcast from one type to another,
but only if the compiler knows that this is possible

* F# can safely pattern match on types (as seen with exception handling).
You can safely try to cast and handle incompatibilities.

```fsharp
let anException = Exception()

// Safely upcasting to Object
let upcastToObject = anException :> obj                             // Success

// Trying to safely upcast to an incompatible type (error)
let upcastToAppException = anException :> ApplicationException      // Error
// Display:
// Type constraint mismatch. The type 'Exception' is not compatible with type 'ApplicationException'

// Unsafely downcasting to an ApplicationException
let downcastToAppException = anException :?> ApplicationException   // InvalidCastException
// Display:
// System.InvalidCastException: Unable to cast object of type 'System.Exception' to type 'System.ApplicationException'.

let downcastToString = anException :?> string                       // Error
// Display:
// Type constraint mismatch. The type 'string' is not compatible with type 'Exception'
```

### Active patterns

Active patterns - form of lightweight discriminated unions.
A way to categorize the *same* value in *different* ways.

Two more sophisticated forms of active patterns are (не рассматриваются здесь):

* *partial* active patterns

* *parameterized* active patterns

```fsharp
// Defining the active pattern
let (|Long|Medium|Short|) (value:string) =      // string -> Choice<unit,unit,unit>
    if (value.Length < 5) then Short
    elif value.Length < 10 then Medium
    else Long

// Using the pattern within a pattern match
let measure word =
    match word with
    | Short -> "This is a short string!"
    | Medium -> "This is a medium string!"
    | Long -> "This is a long string!"

// Usage example
measure "Hi"                // "This is a short string!"
measure "Hello"             // "This is a medium string!"
measure "Good afternoon"    // "This is a long string!"
```

### Computation expressions

Computation expressions allow you to create language support for a specific abstraction,
directly in code - whether that's asynchronous work, optional objects, sequences, or cloud
computations. F# also allows you to create your *own* computation expressions to capture a type of
behavior that you want to abstract away, with your own "versions" of `let!`.

Example of a computation expression for working with options:

```fsharp
type Maybe() =
    // 'b option * ('b -> 'c option)
    member this.Bind(opt, func) = opt |> Option.bind func
    // 'a -> 'a option
    member this.Return v = Some v

let maybe = Maybe()

// string -> int option
let rateCustomer name =
    match name with
    | "isaac" -> Some 3
    | "mike" -> Some 2
    | _ -> None

// int option
let answer =
    // Creating a maybe { } block
    maybe {
        // Safely "unwrapping" an option type
        let! first = rateCustomer "isaac"    // int option -> int    (Return 3)
        let! second = rateCustomer "mike"    // int option -> int    (Return 2)
        return first + second }              // Some 5
```

F# automatically maps `let!` to `Bind()`, `return` to `Return()`.

### Code quotations

Essentially the equivalent of C#'s expression trees, code quotations allow you to wrap a
block of code inside a `<@ quotation block @>` and then programmatically interrogate the
abstract syntax tree (AST) within it. F# has two forms of quotations: *typed* and *untyped*.
You won’t find yourself using these in everyday code, but if you ever need to do low-level
meta programming or write your own type provider, you’ll come into contact with these.

### Lazy computations

F# create `System.Lazy` values by wrapping any expression in a `lazy` scope:

```fsharp
// Lazy<int>
let lazyText =
    // Creating a lazy scope
    lazy
        let x = 5 + 5
        printfn "%O: Hello! Answer is %d" System.DateTime.UtcNow x
        x

// Explicitly evaluating the result of a lazy computation
let text = lazyText.Value    // 09.04.2021 20:17:36: Hello! Answer is 10;   text = 10

// Returning the result without re-executing the computation
let text2 = lazyText.Value   // text2 = 10
```

### Recursion

F# support *tail recursion* (the ability to call a recursive function without risking
stack overflow).

To create a recursive function, prefix it with the `rec` keyword:

```fsharp
// Specifying that a function can be called recursively
let rec factorial number total =
    if number = 1 then total
    else
        printfn "Number %d" number
        factorial (number - 1) (total * number)   // Making a recursive function call

// Calling a recursive function
let total = factorial 5 1
// Number 5
// Number 4
// Number 3
// Number 2
// val total : int = 120
```

## Must-visit F# resources

### Websites

* https://fsharp.org/
The official home of F#.

* https://c4fsharp.net/
Information about events in the F# world. Contains a list of webinars and recorded user group
talks demonstrating various aspects of F#.

* http://fsharpforfunandprofit.com/
**F# for Fun and Profit** website by Scott Wlaschin.

* https://sergeytihon.com/category/f-weekly/
**F# Weekly** performs an excellent news aggregation function for F#.

### Social networks

* https://twitter.com/hashtag/fsharp
`#fsharp` hashtag is your friend on Twitter.

* https://fsharp.slack.com/
The F# Software Foundation slack channel.

* https://functionalprogramming.slack.com/
An unofficial F# channel.

* https://www.reddit.com/r/fsharp/
The F# subreddit contains many news items and discussion topics.

* https://groups.google.com/forum/#!forum/fsharp-opensource
The F# mailing list.

### Projects and language

* https://github.com/dotnet/fsharp
The F# compiler, F# core library, and F# editor tools.

* https://github.com/fsharp/fslang-suggestions
F# Language and Core Library Suggestions.

* http://fsprojects.github.io/
FS projects - semiofficial list of popular F# projects that are on NuGet and GitHub.

## Must-have F# libraries

### Build and DevOps

* http://fsharp.github.io/FAKE
**FAKE** (F# Make) is a build automation system with capabilities that are similar
to make and rake.

* http://fsprojects.github.io/ProjectScaffold/
**ProjectScaffold** helps you get started with a new .NET/Mono project solution with everything
needed for successful organizing of code, tools, and publishing.

### Data

* http://fsprojects.github.io/ExcelProvider/
**ExcelProvider**. Type provider to work seamlessly with CSV, JSON, or XML files.

* http://bluemountaincapital.github.io/Deedle/
**Deedle** is an easy-to-use library for data and time-series manipulation and for scientific
programming. (Equivalent of R's DataFrames, or Python's Pandas).

* https://fslab.org/
**FsLab** is a collection of libraries for data science. It provides a rapid development
environment that lets you write advanced analysis with a few lines of production-quality code.

* https://fslab.org/FSharp.Charting/
**FSharp.Charting** uses the charting components built into .NET to create charts.

### Web

* https://fable.io/
**Fable** is a compiler F# into JavaScript.

* https://websharper.com/
**WebSharper**. Develop microservices, client-server web applications, reactive SPAs, and more
in C# or F#.

* https://freya.io/, https://github.com/xyncro/freya
**Freya** - F#-first web programming framework, just like Suave.

* http://fsprojects.github.io/FSharp.Formatting/
**F# Formatting** - generate HTML documentation based on F# scripts or a combination of Markdown
files with embedded F#.

### Cloud

* https://github.com/fsprojects/FSharp.Azure.Storage
**FSharp.Azure.Storage** provides a pleasant F# DSL on top of the table service, with support
for easy insertion, updates, and queries of data directly from F# records.

* http://fsprojects.github.io/AzureStorageTypeProvider/
The **Azure Storage Type Provider** gives you a full type provider over the three main
Azure Storage services: Blobs, Tables, and Queues.

* https://github.com/fsprojects/FSharp.AWS.DynamoDB
**FSharp.AWS.DynamoDB** an F# wrapper over the standard Amazon.DynamoDB library.

* http://mbrace.io/
**MBrace.Core** is a simple programming model for scalable cloud data scripting and programming with F# and C#. With MBrace.Azure, you can script Azure for large-scale compute and data processing, directly from your favourite editor.

### Desktop

* http://fsprojects.github.io/FsXaml/, https://github.com/fsprojects/FsXaml
**FsXaml** - F# Tools for working with XAML Projects. Library that removes the need for the
code-behind code generation through a type provider.

* https://github.com/fsprojects/FSharp.ViewModule
**FSharp.ViewModule**. Library providing MVVM and `INotifyPropertyChanged` support for
F# projects.

### Miscellaneous

* http://fsprojects.github.io/Argu/
**Argu** -  parse configuration arguments for a console application.

* http://fsprojects.github.io/FSharp.Management/
The **FSharp.Management** project contains various type providers for the management
of the machine: File System, Registry, Windows Management Instrumentation (WMI),
PowerShell, SystemTimeZonesProvider.

* http://fsprojects.github.io/FsReveal/
**FsReveal** allows you to write beautiful slides in Markdown and brings C# and F# to the
`reveal.js` web presentation framework.

* http://fsprojects.github.io/FSharp.Configuration/
**FSharp.Configuration** is a set of easy-to-use type providers that support the reading of
various configuration file formats: AppSettings, ResX, Yaml, INI.

* http://fsprojects.github.io/Chessie/
**Chessie**. Brings railway-oriented programming to .NET.

## The F# toolchain. Comparing alternative technology stacks on .NET and F#

| Function               | Microsoft stack              | Pure F# stack
|------------------------|------------------------------|-------------------------------------
| Complex build process  | MS Build custom tasks        | FAKE script with MSBuild
| Continuous integration | TeamCity, TFS pipeline, etc. | FAKE script on TeamCity, TFS, etc.
| Dependency management  | NuGet                        | Paket with NuGet + GitHub dependencies
| Project system         | Solution and projects        | Standalone scripts and/or project + solution
| Ad hoc processing      | Console applications         | Standalone scripts
| Test libraries         | xUnit, NUnit                 | Expecto, QuickCheck, Unquote, FsTest
| SQL ORM                | Entity Framework             | SQLProvider
| SQL micro-ORM          | Dapper                       | FSharp.Data SQLClient
| Server-side web        | Full-blown Web API project   | Bare-bones NET Web API OWIN, or Suave
| Front-end web          | ASP .NET MVC, TypeScript     | F# with Fable
| IDE                    | Visual Studio                | VSCode, Emacs, Visual Studio, and so on

## Links

* https://msdn.microsoft.com/en-gb/visualfsharpdocs/conceptual/fsharp-language-reference
Справочник по языку F#.

* https://github.com/fsprojects/FSharp.TypeProviders.SDK
How to write your own type providers.

* http://fsprojects.github.io/FSharp.Data.SqlClient
Official documentation for SqlClient.
(There are a few SQL commands that the TypeProvider doesn't support).

* http://fsprojects.github.io/SQLProvider/
SQLProvider - type provider. ORM. Can work with many ODBC data sources.

* https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/query-expressions
The full list of query-expressions.

* http://tomasp.net/blog/csharp-async-gotchas.aspx/
Tomas Petricek has a great post on async/await versus async workflows.
