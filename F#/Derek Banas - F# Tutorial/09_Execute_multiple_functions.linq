<Query Kind="FSharpProgram" />

open System

// Execute multiple functions
let exec_multiple_func() =
    
    let mult_num x = x * 3
    let add_num y = y + 5
    
    // Сначала умножаем, потом прибавляем
    let mult_add = mult_num >> add_num
    // Сначала прибавляем, потом умножаем
    let add_mult = mult_num << add_num
    
    printfn "mult_add : %i" (mult_add 10)   // mult_add : 35
    printfn "add_mult : %i" (add_mult 10)   // add_mult : 45
    
exec_multiple_func()