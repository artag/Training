# Lesson 25. Consuming C# from F#

## Referencing C# code in F#

### Consuming C# code from F#

A simple C# class:

```csharp
// Public read-only
public string Name { get; private set; }
// Constructor
public Person(string name) {
    Name = name; }
// Public method
public void PrintName() {
    Console.WriteLine($"My name is {Name}"); }
```

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
