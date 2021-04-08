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