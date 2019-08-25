<Query Kind="FSharpProgram" />

open System

let do_cast() = 
    let number = 2
    printfn "Type : %A" (number.GetType())      // Type : System.Int32
    printfn "A Float : %.2f" (float number)     // A Float : 2.00
    printfn "An Int : %i"(int 3.14)             // An Int: 3
    
do_cast()