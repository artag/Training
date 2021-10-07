# Get Programming with F#

* Lesson 01. The Visual Studio experience.

  Настройка Visual Studio 2015. Для более новых версий уже неактуально.

* [Lesson 02. Creating your first F# program](Lesson_02.md)

* Lesson 03. The REPL - changing how we develop.

  Про The Read Evaluate Print Loop (REPL). Еще один подход к написанию программ.

* [Lesson 04. Saying a little, doing a lot](Lesson_04.md)

* [Lesson 05. Trusting the compiler](Lesson_05.md)

* [Lesson 06. Working with immutable data](Lesson_06.md)

* [Lesson 07. Expressions and statements](Lesson_07.md)

* [Lesson 08. Capstone 1](Lesson_08.md)

* [Lesson 09. Shaping data with tuples](Lesson_09.md)

* [Lesson 10. Shaping data with records](Lesson_10.md)

* [Lesson 11. Building composable functions](Lesson_11.md)

* [Lesson 12. Organizing code without classes](Lesson_12.md)

* [Lesson 13. Achieving code reuse in F#](Lesson_13.md)

* [Lesson 14. Capstone 2](Lesson_14.md)

* [Lesson 15. Working with collections in F#](Lesson_15.md)

* [Lesson 16. Useful collection functions](Lesson_16.md)

* [Lesson 17. Maps, dictionaries, and sets](Lesson_17.md)

* [Lesson 18. Folding your way to success](Lesson_18.md)

* [Lesson 19. Capstone 3](Lesson_19.md)

* [Lesson 20. Program flow in F#](Lesson_20.md)

* [Lesson 21. Modeling relationships in F#](Lesson_21.md)

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
