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
