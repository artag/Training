<Query Kind="FSharpProgram" />

open System

let hello() =
    printf "Enter your name : "
    
    let name = Console.ReadLine()
    
    printfn "Hello %s" name
    
    // %s - string
    // %i - integer
    // %b - boolean
    // %f - float
    // %A - internal representation of things (tuples and etc.)
    // %O - representation of object
   
hello()

Console.ReadKey() |> ignore
