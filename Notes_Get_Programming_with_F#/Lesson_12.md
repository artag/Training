# Lesson 12. Organizing code without classes

### Sets of rules for organizing code

Work in F# is much, much simpler than in C#:

* We're not so worried about classes or inheritance or behaviors and state.
* We're typically using stateless functions operating over immutable data.
So we can use alternative sets of rules for organizing code.

By default, follow these simple rules:

* Place related types together in namespaces.
* Place related stateless functions together in modules.

That's pretty much it for many applications.

## Namespaces and Modules

### Namespaces in F#

Namespaces can't hold are functions - only types.

Namespaces in F# are essentially identical to those in C# in terms of functionality:

* Namespaces uses to logically organize data types, such as records as well as modules.
* Namespaces can be nested underneath (вложены в) other namespaces in a hierarchy.
* You can open namespaces in order to avoid having to fully qualify types or modules.
* You can share namespaces across multiple files.
* You can manually access functions through a fully qualified namespace ...
* ... Or you can `open` the namespace, after which you can access the static class `File`
directly.

```fsharp
let file = @"C:\users\isaac\downloads\foo.txt"
System.IO.File.ReadLines file

open System.IO
File.ReadAllLines file
```

### Modules in F#

* Use modules in F# to hold `let`-bound functions.
* Modules can also be used like namespaces in that they can store types as well.

Depending on your point of view, you can think of F# modules in one of two ways:
* Modules are like static classes in C#.
* Modules are like namespaces but can also store functions.

* You can create a module for a file by using the `module <my module>` declaration at the top of
the file. Any types or functions declared underneath this line will live in this module.

### Example of namespaces and modules

```fsharp
// File domain.fs         (domain.fs must live above dataAccess.fs)
namespace MyApplication.BusinessLogic

type Customer = { ... }
type Account = { .. }

// File dataAccess.fs     (References to domain.fs)
module MyApplication.BusinessLogic.DataAccess

type private DbError = { ... }
let private getDbConnection() = ...
let saveCustomer = ...
let loadCustomer = ...

module private Helpers =            // Nested module - visible only in DataAccess module
    let handleDbError ex = ...
    let checkDbVersion conn = ...
```

1. 2 *files*, that will be compiled into a single *assembly*. Оба делят the same logical
namespace `MyApplication.BusinessLogic`.

2. Shared domain types are stored in a single file `domain.fs`.
Data access is stored in the `DataAccess` module in the same namespace (file `dataAccess.fs`).

3. In `dataAccess.fs`, you don't need to explicitly open `MyApplication.BusinessLogic` to get
access to the `Customer` and `Account` types, because the module lives in that namespace.

4. The nested module, `Helpers` lives inside the `DataAccess` module. It might help to think of
this as an inner (nested) static class.

5. You can use nested modules as a way of grouping functions if you find your modules getting
too large.

### Opening modules

```fsharp
// Opening the CustomerFunctions module
open CustomerFunctions

let isaac = newCustomer "isaac"
// Unqualified access to functions from within the module
isaac |> activate |> setCity “London” |> generateReport
```

### Namespaces vs. modules

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

```fsharp
// Domain.fs     (выше чем Operation.fs)
namespace Domain        // Namespace declaration
// ...

// Operation.fs
module Operations       // Declaring a module
open Domain             // Opening the Domain namespace
// ...
```

Что-то типа **рекомендаций**:

* Use namespaces as in C#, to logically group types and modules.

* Use modules primarily to store functions, and secondly to store types that are tightly related
to those functions.

## Tips for working with modules and namespaces

### Access modifiers

* By default, types and functions are always **public** in F#.

* If you want to use a function within a module (or a nested module) but don’t want to expose it
publicly, mark it as **private**.

### The global namespace

If you don’t supply a *parent* namespace when declaring namespaces or modules, it’ll
appear in the `global` namespace, which is always open.

### Automatic opening of modules

Add the `[<AutoOpen>]` attribute on the module.

With this attribute applied, opening the parent namespace in the module will automatically open
access to the module as well.

### Scripts

You can create `let`-bound functions directly in a script.
This is possible because an implicit module is created for you based on the name of the script
(similar to automatic namespacing).
You can explicitly specify the module in code if you want, but with scripts it’s generally not
needed.
