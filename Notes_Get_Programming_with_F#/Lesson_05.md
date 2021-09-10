# Lesson 05. Trusting the compiler

Анонимные типы данных не существуют в F#. Но в этом языке есть более "мощные" альтернативы.

### Hypothetical type inference in C#

Если бы в C# можно было определять через `var` все аргументы и выходные типы данных в методах...

```csharp
public static var CloseAccount(var balance, var customer)
{
    // ...
}
```

... но это в C# невозможно сделать, а в F# можно.

## F# type-inference basics

Компилятор F# может вывести типы для:

* Local bindings (as per C#)
* Input arguments for both built-in and custom F# types
* Return types

F# uses a sophisticated algorithm that relies on the Hindley-Milner (HM) type system.

### Explicit type annotations in F#

```fsharp
let add (a:int, b:int) : int =
    let answer:int = a + b
    answer
```

Type signature in F# has three main parts:
* The function name
* All input argument type(s)
* The return type

Omited type annotations:

```fsharp
let add (a, b) =
    let answer = a + b
    answer
```

### Limitations of type inference

1. Type inference works best with types *native* to F# (basic types such as ints, or F# types
that you define yourself). If you try to work with a type from a C# library (and this
includes the .NET BCL), type inference won't work quite as well. Хотя единственной
аннотации будет достаточно для дальнейшего применения.

Пример:

```fsharp
// Doesn't compile - type annotation is required.
// Компилятор не может вывести тип для name, опираясь только на наличие свойства Length.
let getLength name = sprintf "Name is %d letters." name.Length

// Compiles
let getLength (name:string) = sprintf "Name is %d letters." name.Length

// Compiles - "name” argument is inferred to be string, based on the call to getLength()
let foo(name) = "Hello! " + getLength(name)
```

2. In F#, *overloaded* functions **aren't allowed**. You can create (or reference from C#
libraries) classes that contain methods that are overloaded, but functions declared using
the `let` syntax can't be overloaded.

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

### Automatic generalization of a function

```fsharp
// val createList : first:'a * second:'a -> List<'a>
let createList(first, second) =
    let output = List()
    output.Add(first)
    output.Add(second)
    output
```

Value `'a` as the same as `T` in C# (a generic type argument).
