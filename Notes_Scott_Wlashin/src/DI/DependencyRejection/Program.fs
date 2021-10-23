module Program
open System

let program() =
    printfn "Enter the first value"
    let str1 = Console.ReadLine()
    printfn "Enter the second value"
    let str2 = Console.ReadLine()

    let result = PureCode.compareTwoStrings str1 str2

    match result with
    | PureCode.Bigger -> printfn "The first value is bigger"
    | PureCode.Smaller -> printfn "The first value is smaller"
    | PureCode.Equal -> printfn "The values are equal"

[<EntryPoint>]
let main argv =
    program()
    0
