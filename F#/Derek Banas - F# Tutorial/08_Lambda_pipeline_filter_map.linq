<Query Kind="FSharpProgram" />

open System

// Using lambda with pipeline example
let pipeline_example() =
    // filter - оставляет элементы, которые удовлетворяют
    // определенным условиям
    [5; 6; 7; 8]
    |> List.filter (fun v -> (v % 2) = 0)
    |> List.map (fun x -> x * 2)
    |> printfn "Even Doubles : %A"          // Even Doubles : [12; 16]

pipeline_example()
