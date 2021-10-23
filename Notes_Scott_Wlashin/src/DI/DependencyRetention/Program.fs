open System

let compareTwoStrings () =
    printfn "Enter the first value"
    let str1 = Console.ReadLine()
    printfn "Enter the second value"
    let str2 = Console.ReadLine()
    
    if str1 > str2 then
        printfn "The first value is bigger"
    else if str1 < str2 then
        printfn "The first value is smaller"
    else
        printfn "The values are equal"

[<EntryPoint>]
let main argv =
    compareTwoStrings()
    0