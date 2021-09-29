# Lesson 18. Folding your way to success

## Understanding aggregations and accumulators

Some of the *aggregation* functions in LINQ or F# collections:

* `Sum`
* `Average`
* `Min`
* `Max`
* `Count`

У всех них одинаковая сигнатура: они берут последовательность элементов типа `T` и возвращают один
объект типа `U`.

Some example types of aggregation:

```text
type Sum        = int   seq -> int
type Average    = float seq -> float
type Count<'T>  = 'T    seq -> int
```

### Creating your first aggregation function

Any aggregation, or `fold`, generally requires three things:

* The *input collection*.

* An *accumulator* to hold the state of the output result as it’s built up.

* An *initial (empty) value* for the accumulator to start with.

### Imperative implementation of sum

```fsharp
let sum inputs =    // seq<int> -> int
    let mutable accumulator = 0                 // Empty accumulator
    for input in inputs do                      // Go through every item
        accumulator <- accumulator + input      // Apply aggregation
    accumulator                                 // Return accumulator
```

Imperative loop - not particularly composable (не особо поддается композиции).

Альтернативой является рекурсия. Но у нее есть недостатки - ее может быть сложно понимать и
поддерживать.

Еще одна альтернатива - использование collection-based way - использование функции `fold`.

## fold

`fold` - a higher-order function that allows you to supply an *input collection* you
want to aggregate, a *start state* for the accumulator, and a function that says *how* to
accumulate data.

Signature for `Seq.fold`:

```fsharp
folder:( 'State -> 'T -> 'State) -> state:'State -> source:seq<'T> -> 'State
```

* `folder` - A function that’s passed into fold that handles the accumulation (summing,
averaging, or getting the length, for example).

* `state` - The initial start state

* `source` - The input collection

Implement `sum` by using the `fold` function (put the arguments on different lines to
make clearer):

```fsharp
// (1) Folder function to sum the accumulator and input let sum inputs
let sum inputs =
    Seq.fold
        (fun state input -> state + input)      // (1)
        0                                       // Initial state
        inputs                                  // Input collection
```

>### Some examples of real-world use of aggregations
>
>* Retrieving the total price of a set of orders.
>* Merging a collection of financial transactions in order to determine whether a
>customer is high risk.
>* Aggregating a set of events in an event-driven system over initial data.
>* Showing a single red/amber/green status on the dashboard of an internal website to
>indicate whether all back-end systems are functioning correctly.

### Making fold more readable

Using `|>` (*pipeline*) and `||>` (*double pipeline*) operators:

```fsharp
// By default
Seq.fold (fun state input -> state + input) 0 inputs

// Using pipeline to move "inputs" to the left side
inputs |> Seq.fold (fun state input -> state + input) 0

// Using the double pipeline to move both the initial state and "inputs" to the left side
(0, inputs) ||> Seq.fold (fun state input -> state + input)
```

Последнее выражение можно прочитать так: "Here’s an initial state of 0 and a collection of input
numbers. Fold them both through this function, and give me the answer."

### Using related (смежные) fold functions

* `foldBack` - Same as `fold`, but goes backward from the last element in the collection.

* `mapFold` - Combines `map` and `fold`, emitting a sequence of mapped results and a
final state.

* `reduce` - A simplified version of `fold`, using the first element in the collection
as the initial state, so you don’t have to explicitly supply one. Perfect for simple folds such as `sum` (although it'll throw an exception on an empty input-beware!)

* `scan` - Similar to `fold`, but generates the intermediate results as well as the
final state. Great for calculating running totals.

* `unfold` - Generates a sequence from a single starting state. Similar to the `yield`
keyword.

### Folding instead of while loops

Accumulating through a `while` loop. Example: counts the number of characters in the
file:

```fsharp
open System.IO
let mutable totalChars = 0                              // Initial state
let sr = new StreamReader(File.OpenRead "book.txt")     // Opening a stream to a file

while (not sr.EndOfStream) do                               // Stopping condition
    let line = sr.ReadLine()
    totalChars <- totalChars + line.ToCharArray().Length    // Accumulation function
```

We have an unknown "end" to this stream of data, rather than a fixed, up-front
collection. How can you use `fold` here, which takes in a sequence of items as input?
The answer is to *simulate* a collection by using the `yield` keyword:

```fsharp
open System.IO
let lines : string seq =
    seq {                                   // Sequence expression
        use sr = new StreamReader(File.OpenRead @"book.txt")
        while (not sr.EndOfStream) do
            yield sr.ReadLine() }           // Yielding a row from the StreamReader

(0, lines) ||> Seq.fold(fun total line -> total + line.Length)      // A standard fold
```

* The `seq { }` block is a form of *computation expression*.

* Here, `yield` has the same functionality as in C#. It yields items to *lazily generate*
a sequence.

* `seq` (to create a sequence block) and `yield` (to yield back values).

## Composing functions with fold

### Alias to a specific type. Create and use a list of rules

`fold` - is as a way to *dynamically* compose functions together: given a list of functions that
have the same signature, give me a single function that runs all of them together.

Для примера, требуется валидация строки по следующим параметрам:

* Every string should contain three words.
* The string must be no longer than 30 characters.
* All characters in the string must be uppercase.

Хочется создать *collection of rules* и их скомпоновать в виде *single rule*.

1. Определим a simple function signature for a rule, которая также можно назвать как *alias*:

```fsharp
type Rule = string -> bool * string
```

Input - text as a string, ouput - Boolean (passed or failed) and a string (the error message in
case of failure).

Такая сигнатура будет использоваться для создания списка правил.

>### Type aliases (Псевдонимы типов)
>
>Type aliases позволяют определить тип сигнатуры, который можно использовать вместо другого.
>Alias - это не новый тип. Определение, которому присваивается alias, взаимозаменяемо с ним, и
>alias будет удален в runtime.
>Это просто способ улучшить документацию и читабельность.

2. Creating a list of rules:

```fsharp
type Rule = string -> bool * string     // alias

// All rules provided inline
let rules : Rule list =    // List definition
    [ fun text -> (text.Split ' ').Length = 3, "Must be three words"
      fun text -> text.Length <= 30, "Max length is 30 characters"
      fun text -> text
                  |> Seq.filter Char.IsLetter
                  |> Seq.forall Char.IsUpper, "All letters must be caps" ]
```

### Composing rules manually

Пример "ручной" компоновки всех трех правил для проверки строки:

```fsharp
let validateManual (rules: Rule list) word =
    let passed, error = rules.[0] word      // Testing the first rule
    if not passed then false, error         // Checking whether the rule failed
        else
        let passed, error = rules.[1] word  // Repeat for all remaining rules
        if not passed then false, error
            else
            let passed, error = rules.[2] word
            if not passed then false, error
            else true, ""
```

Так делать некрасиво и нежелательно. Такой подход плохо масштабируется.

### Folding functions together

Альтернативный, более правильный подход - composing a list of rules by using `reduce`
(одна из форм `fold`):

*(похоже на composite pattern в ООП)*

```fsharp
// Rule seq -> Rule
// (string -> bool * string) seq -> (string -> bool * string)
let buildValidator (rules : Rule list) =
    rules
    |> List.reduce(fun firstRule secondRule ->
        fun word ->                                 // Higher-order function
            let passed, error = firstRule word      // Run first rule
            if passed then                          // Passed, move on to next rule
                let passed, error = secondRule word
                if passed then true, ""
                else false, error
            else false, error)                      // Failed, return error

// Использование
let validate = buildValidator rules
let word = "HELLO FrOM F#"
validate word
// val it : bool * string = (false, "All letters must be caps")
```
