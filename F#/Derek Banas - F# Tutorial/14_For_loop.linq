<Query Kind="FSharpProgram" />

open System

let loop_stuff() =
    // От 1 до 10 (включая)
    for i = 1 to 10 do          // 1 2 3 4 5 6 7 8 9 10 
        printf "%i " i
    printfn ""
    
    // От 10 до 1 (включая)
    for i = 10 downto 1 do      // 10 9 8 7 6 5 4 3 2 1
        printf "%i " i
    printfn ""
    
    // Используя range
    for i in [11..20] do
        printf "%i " i          // 11 12 13 14 15 16 17 18 19 20  
    printfn ""
    
    for i in [-5..5] do
        printf "%i " i          // -5 -4 -3 -2 -1 0 1 2 3 4 5 
    printfn ""
    
loop_stuff()