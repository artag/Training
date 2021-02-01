# Get Programming with F#

## Lesson 2

### Console application entry point

`<EntryPoint>]` - An attribute that tells F# that this is the function to call
when starting the application.

```fsharp
[<EntryPoint>]
let main argv =         // The declaration of the function
    let items = argv.Length
    printfn "Passed in %d items: %A" items argv
    0                   // return an integer exit code
```

### Array

In F#, the syntax for arrays is as follows:

```fsharp
let items = [| "item"; "item"; "item"; "item" |]
```

### printfn in F#

`printfn` is a useful function (along with its sibling `sprintf`) that allows
you to inject values into strings by using placeholders.

These placeholders are also used to indicate the type of data being supplied:

* `%d` - int
* `%f` - float
* `%b` - Boolean
* `%s` - string
* `%O` - The .ToString() representation of the argument
* `%A` - An F# pretty-print representation of the argument that
falls back to `%O` if none exists

## Lesson 4

### Comparing functional equivalents to core object-oriented class features

Class | Function|
------|---------|
Constructor / single public method | Arguments passed to the function
Private fields | Local values
Private methods | Local functions

## Lesson 5

### Type-inferred generics

You can either use an underscore ( `_` ) to specify a placeholder for the generic type
argument, or omit the argument completely.

```fsharp
open System.Collections.Generic

// Creating a generic List, but omitting the type argument.
let numbers = List<_>()
numbers.Add(10)
numbers.Add(20)

// This syntax is also legal.
let otherNumbers = List()
otherNumbers.Add(10)
otherNumbers.Add(20)
```

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
