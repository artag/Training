<Query Kind="FSharpProgram" />

open System

// Recursive function
let recursive_example() =
    let rec factorial x =
        if x < 1 then 1
        else x * factorial (x - 1)
        
    printfn "Factorial 4 : %i" (factorial 4)    // 24
    // 1st: result = 4 * factorial(3) = 4 * 6 = 24
    // 2nd: result = 3 * factorial(2) = 3 * 2 = 6
    // 3rd: result = 2 * factorial(1) = 2 * 1 = 2
    
recursive_example()
