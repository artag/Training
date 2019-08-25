<Query Kind="FSharpProgram" />

open System

let hello() =
    // Padding слево и padding вправо
    printfn "%-5s %5s" "a" "b"
    
    // Dynamic padding, 10 - how much padding
    // Будет добавление padding на 10 слева
    printfn "%*s" 10 "Hi"
    
hello()
