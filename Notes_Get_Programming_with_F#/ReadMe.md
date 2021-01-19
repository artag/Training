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
