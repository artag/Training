<Query Kind="FSharpProgram" />

open System

// Generic - позволяет использовать любой тип в функции
let add<'T> x y =
    printfn "%A" (x + y)
    
let generic_stuff() = 
    add<float> 5.5 2.4      // 7.9

    // Или можно использовать integers:
    // Одновременно не получилось использовать функцию с двумя разными типами
    // add<int> 5 2
       
generic_stuff()
