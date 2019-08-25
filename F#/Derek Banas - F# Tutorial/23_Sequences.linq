<Query Kind="FSharpProgram" />

open System

// Sequences - позволяет создавать бесконечно большие структуры данных
// Генерация данных идет до тех пор, пока они нужны
let seq_stuff() =

    let seq1 = seq { 1 .. 100 }
    let seq2 = seq { 0 .. 2 .. 50 }  // Generate only even numbers
    let seq3 = seq { 50 .. 1 }
    
    // Печатает только первые 4 значения
    printfn "%A" seq1       // seq [1; 2; 3; 4; ...]
    printfn "%A" seq2       // seq [0; 2; 4; 6; ...]
    printfn "%A" seq3       // seq []
    printfn ""
    
    // Распечатать всю последовательность от 1 до 50
    Seq.toList seq2 |> List.iter (printfn "Num: %i")
    printfn ""
    
seq_stuff()