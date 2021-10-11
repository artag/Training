# Appendix E. Other F# language features

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
