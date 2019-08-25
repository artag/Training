<Query Kind="FSharpProgram" />

open System

// Example integers adding
let integer_adding() =
    // Типы параметров опциональны
    // Третий int - тип возвращаемого значения
    let get_sum (x : int, y : int) : int = x + y
    
    printfn "5 + 7 = %i" (get_sum(5,7))

integer_adding()
