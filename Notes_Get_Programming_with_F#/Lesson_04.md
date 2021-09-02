# Lesson 4. Saying a little, doing a lot

Language traits (черты/особенности) compared

| Language   |Type system       | Syntax
|------------|------------------|-------------------------------- 
|C#          | Static, simple   | Curly brace, verbose
| Java       | Static, simple   | Curly brace, extremely verbose
| Scala      | Static, powerful | Curly brace, verbose
| Python     | Dynamic, simple  | Whitespace, very lightweight
| Ruby       | Dynamic, simple  | Whitespace, very lightweight
| JavaScript | Dynamic, simple  | Curly brace, lightweight

F# has a powerful, static type system that has an extremely lightweight syntax.

## Binding values in F#

Sample `let` bindings

```fsharp
// Binding 35 to the symbol age
let age = 35

// Binding a URI to the symbol website
let website = System.Uri "http://fsharp.org"

// Binding a function that addstwo numbers together to the symbol add
let add (first, second) = first + second
```

Здесь:

* *No types*
* *No `new` keyword* -  The `new` keyword is optional, and generally not used except
when constructing objects that implement `IDisposable`. Instead, F# views a constructor
as a function, like any other "normal" function that you might define.
* *No semicolons* - They're optional. The newline is enough.
* *No brackets for function arguments*
  * For *curried form* - not needed
  * For *tupled form* - functions that take a single argument don't *need* round brackets.
  Functions that take in zero or multiple arguments need them.

### `let` isn't `var`

`var` declares variables in C#, `let` binds an *immutable* value to a symbol.

## Scoping values

Scoping in F#:

```fsharp
// Opened up the System namespace so that you can call Console.WriteLine directly.
open System

let doStuffWithTwoNumbers(first, second) =
    // Creation of scope for the doStuffWithTwoNumbers function
    let added = first + second
    Console.WriteLine("{0} + {1} = {2}", first, second, added)
    let doubled = added * 2
    doubled                       // Return value of the function
```

Здесь:

* *No `return` keyword* - assumes that the final expression of a scope is the result of that scope.
* *No accessibility modifier* - `public` is the default for top-level values. In nested
scopes you can hide values effectively without resorting to accessibility modifiers.
* *No `static` modifier* - static is the default way of working in F#.
* *No `protected` modifier* - F# не поддерживает.

### Nested scopes

Tightly bound scope:

```fsharp
// Top-level scope
let estimatedAge =
    // Nested scope
    let age =
        // Value of year visible only within scope of "age" value
        let year = DateTime.Now.Year
        year - 1979
    // Can't access "year" value
    sprintf "You are about %d years old!" age
```

You can think of each of these nested scopes as being *mini classes*.

### Nested functions

Nested (inner) functions:

```fsharp
let estimateAges(familyName, year1, year2, year3) =    // Top-level function
    let calculateAge yearOfBirth =                     // Nested function
        let year = System.DateTime.Now.Year
        year - yearOfBirth

    let estimatedAge1 = calculateAge year1      // Calling the nested function
    let estimatedAge2 = calculateAge year2
    let estimatedAge3 = calculateAge year3

    let averageAge = (estimatedAge1 + estimatedAge2 + estimatedAge3) / 3
    sprintf "Average age for family %s is %d" familyName averageAge
```

### Comparing functional equivalents to core object-oriented class features

Class                              | Function
-----------------------------------|----------------------------------
Constructor / single public method | Arguments passed to the function
Private fields                     | Local values
Private methods                    | Local functions

### Capturing values in F#

Within the body of a nested function (or any nested value), code can access any values
defined in its containing (parent) scope without you having to explicitly supply them as
arguments to the nested function.

### Cyclical dependencies in F#

F# doesn't permit (не разрешает) cyclical dependencies.

*In F#, the order in which types are defined matters.*

Type A can't reference Type B if Type A is declared before Type B, and the same applies to values.

*File order in a project matters.*

Files at the bottom of the project can access types and values defined above them,
but not the other way around. You can manually move files up and down in
VS by selecting the file and pressing `Alt-up arrow` or `Alt-down arrow` (or right-clicking a
file and choosing the appropriate option).
