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

- |Returns something? | Has side-effects?
--|-----------|------------------
Statements |Never|Always
Expressions|Always| Rarely

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
