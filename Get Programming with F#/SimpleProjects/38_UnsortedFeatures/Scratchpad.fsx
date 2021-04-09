(*
    Active patterns
*)

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

(*
    A custom computation expression
*)
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
        let! first = rateCustomer "isaac"       // int option -> int // 3
        let! second = rateCustomer "mike"       // int option -> int // 2
        return first + second }                 // Some 5

(*
    Lazy computations
*)
// Lazy<int>
let lazyText =
    lazy
        let x = 5 + 5
        printfn "%O: Hello! Answer is %d" System.DateTime.UtcNow x
        x

let text = lazyText.Value    // 09.04.2021 20:17:36: Hello! Answer is 10;   text = 10
let text2 = lazyText.Value   // text2 = 10

(*
    Recursion
*)
let rec factorial number total =
    if number = 1 then total
    else
        printfn "Number %d" number
        factorial (number - 1) (total * number)

let total = factorial 5 1
// Number 5
// Number 4
// Number 3
// Number 2
// val total : int = 120
