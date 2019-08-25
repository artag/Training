<Query Kind="FSharpProgram" />

open System

// Tuples - это набор данных разных типов, разделенных запятыми
let tuple_stuff() =
    
    // Calculate average value
    let avg (w, x, y, z) : float =
        let sum = w + x + y + z
        sum / 4.0

    printfn "Avg : %f" (avg(1.0, 2.0, 3.0, 4.0))    // Avg : 2.500000

    // Assign one value
    let my_data = ("Derek", 42, 6.25)
    let (name, age, _) = my_data
    printfn "Name : %s" name                        // Name : Derek
    
tuple_stuff()