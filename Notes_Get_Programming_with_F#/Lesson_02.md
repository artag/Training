# Lesson 2. Creating your first F# program

## F# project types

* Library

* Console Application

## Console application entry point

`<EntryPoint>]` - An attribute that tells F# that this is the function to call
when starting the application.

```fsharp
[<EntryPoint>]
let main argv =         // The declaration of the function
    let items = argv.Length
    printfn "Passed in %d items: %A" items argv
    0                   // return an integer exit code
```

Особенности:

* *No class declaration* - using a module as a container for functions inside .fs files, but for
console applications, you can skip this entirely. Instead, the compiler uses the name of the file
as the module implicitly.
* *Use `EntryPoint`* - in C#, you don't need the entry point because you must instead specify it
through the project properties. In F#, this is achieved by marking the function with an attribute
`[<EntryPoint>]`.
* *No return keyword / curly braces / semicolons / type declarations (and so forth)*
* F# is *whitespace-significant* - indentation of code is used to represent blocks.

## F# file types

F# has two files types:

* **.fs** - Equivalent to .cs or .vb, these files are compiled as part of a project by
MSBuild and the filenames end in .dll or .exe.

* **.fsx** - An F# script file that's a standalone piece of code that can be executed without
first needing to be compiled into a .dll.
C# have a similar file type to this known as .csx.

## Array

In F#, the syntax for arrays is as follows:

```fsharp
let items = [| "item"; "item"; "item"; "item" |]
```

The closest equivalent in C# is probably this:

```csharp
var items = new [] { "item", "item", "item", "item" };
```

Аргументы из входной строки консольного приложения автоматически конвертируются в
строковый массив.

## printfn in F#

`printfn` is a useful function (along with its sibling `sprintf`) that allows
you to inject values into strings by using placeholders.

These placeholders are also used to indicate the type of data being supplied:

* `%d` - int
* `%f` - float
* `%b` - Boolean
* `%s` - string
* `%O` - The `.ToString()` representation of the argument
* `%A` - An F# pretty-print representation of the argument that
falls back to `%O` if none exists

Supply the args, space-separated, after the raw string. Don't use brackets or commas
to separate the arguments to `printfn` - only spaces.
