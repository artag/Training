<Query Kind="FSharpProgram" />

open System

let where_to_go age =
    if age < 5 then
        printfn "Preschool"
    elif age = 5 then
        printfn "Kindergarten"
    elif (age > 5) && (age <= 18) then
        let grade = age - 5
        printfn "Go to Grade %i" grade
    else
        printfn "Go to College"
    
where_to_go 3       // Preschool
where_to_go 5       // Kindergarten
where_to_go 8       // Go to Grade 3
where_to_go 20      // Go to College
printfn ""

// Пример логического OR
let calc_grant gpa income =
    printfn "College Grant : %b" ((gpa >= 3.8) || (income <= 12000))
    printfn ""
    
let gpa = 3.9
let income = 15000
calc_grant gpa income                   // College Grant : true

// Еще можно писать not
printfn "Not True : %b" (not true)      // Not True : false