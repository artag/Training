# Lesson 09. Shaping data with tuples

## The need for tuples

Есть метод, что он должен возвратить?

```csharp
public ??? ParseName(string name)
{
    var parts = name.Split(' ');
    var forename = parts[0];
    var surname = parts[1];
    return ???;
}
```

Возможные возвращаемые значения:

* **DTO**
  * *-* Heavyweight approach for something as small as this one-off function.
  * *-* You'll quickly end up with many DTOs, all of which are similar.
  * *-* Probably have to map between many DTOs.
* **Anonymous type**
  * C# doesn't allow anonymous types to escape method scope.
  * You can return it as a weakly typed object, and then use reflection or a similar process to
  get at the data.
  * *-* Runtime checking for types.
  * *-* Anonymous types are internal, so this solution doesn't work across assemblies.
* **Dynamic type**
  * *-* Runtime checking for types.
* **Array of strings**
  * *-* Type system isn't working for you.
  * *-* If you want to return a mixture of types, you're again stuck.
* **Out parameters**
  * Everyone hates these!

Решение - использовать в качестве возвращаемого значения **tuple**.

*+* Allow you to pass arbitrary bits of data around, temporarily grouped together.
*+* Support equality comparison by default.
*-* Properties show up as `Item1`, `Item2`, `ItemN`.

Returning arbitrary data pairs in C#:

```csharp
public Tuple<string, string> ParseName(string name) {
    string[] parts = name.Split(' ');
    string forename = parts[0];
    string surname = parts[1];
    return Tuple.Create(forename, surname); }

// Calling a method that returns a tuple of string, string
Tuple<string, string> name = ParseName(“Isaac Abraham”);

// Manually deconstructing the tuple into meaningful variables
string forename = name.Item1;
string surname = name.Item2;
```

## Tuples

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

* You can create tuples by separating values with a comma (запятая).
* You can also *deconstruct* a tuple back into separate parts by assigning them to different
values, again with a comma.
* Tuples can also be of arbitrary length and contain a mixture of types.

### When should I use tuples?

* Tuples are great for internal helper functions and for storing intermediary state.
* Using tuple within (внутри) a function as a way to package a few values in
order to easily pass them to another section of code, or as a way of specifying intent
(способ указания намерения). Например, группировка таких параметров как "имя" и "фамилия" или
"идентификатор" и "номер счета" и т.д.

### Tuple helpers: `fst` and `snd`

F# also has two built-in functions for working with two-part tuples: `fst` and `snd`.

These functions take in a two-part tuple and return either just the first or second element in
the tuple.

## More-complex tuples

### Tuple type signatures

A three-part tuple of two strings and an int would be notated as:

```fsharp
string * string * int
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

If there are elements of a tuple that you're not interested in, you can discard them while
deconstructing a tuple by assigning those parts to the underscore symbol:

```fsharp
let nameAndAge = "Jane", "Smith", 25
let forename, surname, _ = nameAndAge       // Discarding the third element
```

The underscore ("_") is a symbol in F# that tells the type system (and the developer) that you
explicitly (явно) don't want to use this value.

### Type inference with tuples

```fsharp
let explicit : int * int = 10, 5    // Explicit type signature
let implicit = 10, 5                // Type inferred to be int * int

// "arguments" inferred (выведены) to be:
// int * int
let addNumbers arguments =
    let a, b = arguments
    a + b
```

### Genericized (обобщенные) functions with tuples

```fsharp
// "arguments" inferred (выведены) to be:
// int * int * 'a * 'b
let addNumbers arguments =
    let a, b, c, _ = arguments      // Deconstructing a four-part tuple
    a + b
```

### Implicit (неявное) mapping of `out` parameters to tuples

C#:
```csharp
// (1) - Declaring the "out" result variable with a default value
// (2) - Trying to parse number in C#
var number = "123";
var result = 0;                                     // (1)
var parsed = Int32.TryParse(number, out result);    // (2)
```

F#:
```fsharp
// Replacing "out" parameters with a tuple in a single call in F#
let result, parsed = Int32.TryParse(number);
```

### When not to use tuples

* Tuples желательно использовать только для работы с элементами до 3-х штук. Если больше, то
лучше использовать `record`.
* Tuples желательно использовать только локально, в публичных API надо использовать `record`.
