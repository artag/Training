# Lesson 20. Program flow in F#

## `for .. in` loops

```fsharp
for number in 1 .. 10 do                    // Upward-counting for loop
    printfn "%d Hello!" number
// 1 Hello!
// ...
// 10 Hello!

// 10 = begin; -1 = step; 1 = end
for number in 10 .. -1 .. 1 do              // Downward-counting for loop
    printfn "%d Hello!" number
// 10 Hello!
// 9 Hello!
//  ...
// 1 Hello!

let customerIds = [ 45 .. 99 ]
for customerId in customerIds do            // Typical for-each-style loop
    printfn "%d bought something!" customerId

// 2 = begin; 2 = step; 10 = end
for even in 2 .. 2 .. 10 do                 // Range with custom stepping
    printfn "%d is an even number!" even
// 2 is an even number!
// 4 is an even number!
// ...
// 10 is an even number!
```

### `for ... to` loops

```fsharp
for identifier = start [ to | downto ] finish do
    body-expression
```

```fsharp
// A simple for...to loop.
for i = 1 to 10 do
    printf "%d " i
// 1 2 3 4 5 6 7 8 9 10

// A for...to loop that counts in reverse.
for i = 10 downto 1 do
    printf "%d " i
// 10 9 8 7 6 5 4 3 2 1

// A for...to loop that uses functions as the start and finish expressions.
let beginning x y = x - 2 * y
let ending x y = x + 2 * y

let function1 x y = 
    for i = (beginning x y) to (ending x y) do
         printf "%d " i

function1 10 4
// 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18
```

### `while` loops

```fsharp
// (1) Opening a handle to a text file
// (2) while loop that runs while the reader isn’t at the end of the stream
open System.IO
let reader = new StreamReader(File.OpenRead @"File.txt")    // (1)
while (not reader.EndOfStream) do                           // (2)
    printfn "%s" (reader.ReadLine())
```

### Breaking the loop

* There’s **no** concept of the `break` command.

* To simulate premature exit of a loop, you should consider replacing the loop with a
sequence of values that you `filter` on (or `takeWhile`), and loop over that sequence
instead.

### Comprehensions

The closest equivalent in C# would be the use of the `System.Linq.Enumerable.Range()`.

```fsharp
// Generating an array of the letters of the alphabet in uppercase
let arrayOfChars = [| for c in 'a' .. 'z' -> Char.ToUpper c |]
// char [] = [|'A'; 'B'; ... 'Z'|]

// Generating the squares of the numbers 1 to 10
let listOfSquares = [ for i in 1 .. 10 -> i * i ]
// int list = [1; 4; 9; 16; 25; 36; 49; 64; 81; 100]

// Generating arbitrary strings based on every fourth number between 2 and 20
let seqOfStrings = seq { for i in 2 .. 4 .. 20 -> sprintf "Number %d" i }
// seq ["Number 2"; "Number 6"; "Number 10"; "Number 14"; ...]
```

### `If`/`then` expressions for complex logic

```fsharp
// (1) A simple clause
// (2) Complex clause - AND and OR combined
// (3) Catchall for "good" customers
// (4) Catchall for other customers
let limit =
    if score = "medium" && years = 1 then 500                   // (1)
    elif score = "good" && (years = 0 || years = 1) then 750    // (2)
    elif score = "good" && years = 2 then 1000
    elif score = "good" then 2000                               // (3)
    else 250                                                    // (4)
```

### Pattern-matching

**!** Sequences can’t be pattern matched against; only arrays and lists are supported.

```fsharp
// (1) Implicitly matching on a tuple of rating and years
// (2) If medium score with one-year history, limit is $500
// (3) Two match conditions leading to $750 limit
// (4) Catchall for other customers with "good" score
// (5) Catchall for all other customers
let limit =
    match customer with                 // (1)
    | "medium", 1 -> 500                // (2)
    | "good", 0 | "good", 1 -> 750      // (3)
    | "good", 2 -> 1000
    | "good", _ -> 2000                 // (4)
    | _ -> 250                          // (5)
```

### Pattern-matching. Guards

```fsharp
// Using the when guard to specify a custom pattern
let getCreditLimit customer =
    match customer with
    | "medium", 1 -> 500
    | "good", years when years < 2 -> 750   // (1) "years when years < 2" - guard
    | "good", 2 -> 1000
    | "good", _ -> 2000
    | _ -> 250
```

### Pattern-matching. Nested matches

```fsharp
let getCreditLimit customer =
    match customer with
    | "medium", 1 -> 500
    | "good", years ->          // Matching on "good" and binding years to a symbol
        match years with        // A nested match on the value of years
        | 0 | 1 -> 750          // Single-value match
        | 2 -> 1000
        | _ -> 2000
    | _ -> 250                  // Global catchall
```

### Pattern-matching. Matching against lists example

```fsharp
// (1) Matching against an empty list
// (2) Matching against a list of one customer
// (3) Matching against a list of two customers
// (4) Matching against all other lists
let handleCustomer customers =
    match customers with
    | [] -> failwith "No customers supplied!"                                   // (1)
    | [ customer ] -> printfn "Single customer, name is %s" customer.Name       // (2)
    | [ first; second ] ->
        printfn "Two customers, balance = %d" (first.Balance + second.Balance)  // (3)
    | customers -> printfn "Customers supplied: %d" customers.Length            // (4)
```

Another example:

```fsharp
// Is a specific length
// Is empty
// Has the first item equal to 5
let checkList (numbers : int list) =
    match numbers with
    | numbers when numbers.Length = 7 -> printfn "List has seven numbers"
    | [] -> printfn "List is empty"
    | head::tail when head = 5 -> printfn "The first number is %i" head
    | _ -> printfn "Not match"
```

### Pattern-matching. Matching records example

```fsharp
// (1) Match against Balance field
// (2) Match against Name field
// (3) Catchall, binding Name to name symbol
let getStatus customer =
    match customer with
    | { Balance = 0 } -> "Customer has empty balance!"              // (1)
    | { Name = "Isaac" } -> "This is a great customer!"             // (2)
    | { Name = name; Balance = 50 } -> sprintf "%s has a large balance!" name
    | { Name = name } -> sprintf "%s is a normal customer" name     // (3)
```

### Pattern-matching. Combining multiple patterns

Checking the following three conditions *all at the same time*:

1. The list of customers has three elements.
2. The first customer is called "Tanya".
3. The second customer has a balance of 25.

```fsharp
// Pattern matching against a list of three items with specific fields
match customers with
| [ { Name = "Tanya" }; { Balance = 25 }; _ ] -> "It's a match!"
| _ -> "No match!
```

### When to use `if`/`then` over match

* Use pattern matching **by default**.

* Use `if`/`then` is when you’re working with code that returns `unit`, and you’re
implicitly missing the default branch:

```fsharp
// If/then with implicit default else branch
if customer.Name = "Isaac" then printfn "Hello!"
```
