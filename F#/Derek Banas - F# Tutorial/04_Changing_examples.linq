<Query Kind="FSharpProgram" />

open System

let bind_stuff() =
    // Обычно не принято делать переменные
    let mutable weight = 175
    printfn "Weight : %i" weight
    
    weight <- 170
    printfn "Changed weight : %i" weight
    
    // Использование reference (тоже не принято использовать)
    let change_me = ref 10
    printfn "Before change : %i" ! change_me
    
    change_me := 50
    printfn "After change : %i" ! change_me

bind_stuff()
