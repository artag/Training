<Query Kind="FSharpProgram" />

open System

// Рекомендуют пользоваться именно этой конструкцией вместо if-elif-else
// Возвращаемое значение типа string
// _ - любое другое значение
let where_to_go age : string =
    match age with
    | age when age < 5 -> "Preschool"
    | 5 -> "Kindergarten"
    | age when ((age > 5) && (age <= 18)) -> sprintf "Grade %s" ((age - 5).ToString())
    | _ -> "College"
    
let display_result age =
    printfn "Age %i, go to the %s" age (where_to_go age)
    
display_result 3        // Age 3, go to the Preschool
display_result 5        // Age 5, go to the Kindergarten
display_result 18       // Age 18, go to the Grade 13
display_result 20       // Age 20, go to the College