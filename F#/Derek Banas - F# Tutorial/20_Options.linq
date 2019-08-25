<Query Kind="FSharpProgram" />

open System

// Опции используются когда функция не возвращает какого-либо значения
let divide x y = 
    match y with
    | 0 -> None
    | _ -> Some(x / y)

// Использование if (из видео)
let do_division x y =
    if (divide x y).IsSome then
        printfn "%i / %i = %A" x y ((divide x y ).Value)
    elif (divide x y).IsNone then
        printfn "Can't Divide by Zero"
    else
        printfn "Something happened"
        
// Использование match
let do_division2 x y =
    let result = divide x y

    match result with
    | result when result.IsSome -> printfn "%i / %i = %A" x y ((divide x y ).Value)
    | result when result.IsNone -> printfn "Can't Divide by Zero"
    | _ -> printfn "Something happened"

do_division 5 2     // 5 / 2 = 2
do_division 5 0     // Can't Divide by Zero
printfn ""

do_division2 9 2    // 9 / 2 = 4
do_division2 9 0    // Can't Divide by Zero