<Query Kind="FSharpProgram" />

open System

let do_math() = 
    printfn "5 + 4 = %i" (5 + 4)            // 9
    printfn "5 - 4 = %i" (5 - 4)            // 1
    printfn "5 * 4 = %i" (5 * 4)            // 20
    printfn "15 / 4 = %i" (15 / 4)          // 3
    printfn "15 %% 4 = %i" (15 % 4)         // 3
    printfn "5 ** 2 = %f" (5.0 ** 2.0)      // 25.000000
    printfn "5 ** 2 = %.1f" (5.0 ** 2.0)    // 25.0

    // Also cos, sin, tan, acos, asin, atan, cosh, sinh, tanh
    printfn "abs 4.5 : %i" (abs -3)             // abs 4.5 : 3
    printfn "ceil 4.5 : %f" (ceil 4.5)          // ceil 4.5 : 5.000000
    printfn "floor 4.5 : %f" (floor 4.5)        // floor 4.5 : 4.000000
    printfn "log 2.71828 : %f" (log 2.71828)    // log 2.71828 : 0.999999
    printfn "log10 1000: %f" (log10 1000.0)     // log10 1000: 3.000000
    printfn "sqrt 25 : %f" (sqrt 25.0)          // sqrt 25 : 5.000000

do_math()