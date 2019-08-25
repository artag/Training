<Query Kind="FSharpProgram" />

open System

let divide_float x y = 
    try
        printfn "%.2f / %.2f = %.2f" x y (x / y)
    with
    | :? System.DivideByZeroException as ex ->
        printfn "Can't Divide by Zero"

divide_float 5.0 4.0        // 5.00 / 4.00 = 1.25
divide_float 5.0 0.0        // 5.00 / 0.00 = Infinity
printfn ""

let divide_int x y =
    try
        if y = 0 then raise(DivideByZeroException "Can't Divide by 0")
        else
            printfn "%i / %i = %i" x y (x / y)
    with
    | :? System.DivideByZeroException as ex ->
        printfn "Can't Divide by Zero"
        
divide_int 5 2          // 5 / 2 = 2
divide_int 5 0          // Can't Divide by Zero