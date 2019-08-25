<Query Kind="FSharpProgram" />

open System

let loop_stuff() =
    // Напечатает на каждой строке от Num : 1 до Num : 10
    [1..10] |> List.iter (printfn "Num : %i")   
    printfn ""
    
    // Суммирование всех элементов в списке
    let sum = List.reduce (+) [1..10]       // Sum : 55
    printfn "Sum : %i" sum
    
loop_stuff()