<Query Kind="FSharpProgram" />

open System

// Lambda function
let lambda_example() =
    let rand_list = [1;2;3]
    // map - выполняет действие над каждым элементом списка
    // и возвращает новый список, с измененными элементами
    // Каждый элемент списка умножить на 2
    let rand_list2 = List.map (fun x -> x * 2) rand_list
    
    // %A - internal representation of the list
    printfn "Double List: %A" rand_list2        // Double List: [2; 4; 6]
    
lambda_example()
