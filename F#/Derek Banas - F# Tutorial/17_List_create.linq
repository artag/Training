<Query Kind="FSharpProgram" />

open System

let create_lists() =
    let list1 = [1; 2; 3; 4]
  
    // Печатает на каждой строке от Num : 1 до Num : 4 
    list1 |> List.iter (printfn "Num : %i")
    printfn ""
    
    printfn "list1 = %A" list1      // list1 = [1; 2; 3; 4]
    
    // Join different values (с помощью ::)
    let list2 = 5::6::7::[]
    printfn "list2 = %A" list2      // list2 = [5; 6; 7]
        
    // Use range (numbers)
    let list3 = [1..5]
    printfn "list3 = %A" list3      // list3 = [1; 2; 3; 4; 5]
    
    // Use range (symbols)
    let list4 = ['a'..'g']
    printfn "list4 = %A" list4      // list4 = ['a'; 'b'; 'c'; 'd'; 'e'; 'f'; 'g']
    
    // Generate list with init and multiply times 2
    let list5 = List.init 5 (fun i -> i * 2)
    printfn "list5 = %A" list5      // list5 = [0; 2; 4; 6; 8]
    
    // Генерация списка от 1 до 5 и возведение каждого числа в квадрат
    let list6 = [ for a in 1..5 do yield (a * a) ]
    printfn "list6 = %A" list6      // list6 = [1; 4; 9; 16; 25]
    
    // Генерация списка от 1 до 20 с выборкой четных чисел 
    let list7 = [ for a in 1..20 do if a % 2 = 0 then yield a ]
    printfn "list7 = %A" list7      // list7 = [2; 4; 6; 8; 10; 12; 14; 16; 18; 20]
    
    let list8 = [ for a in 1..3 do yield! [ a .. a + 2] ]
    printfn "list8 = %A" list8      // list8 = [1; 2; 3; 2; 3; 4; 3; 4; 5]
    // 1 + 0; 2 + 0; 3 + 0;
    // 1 + 1; 2 + 1; 3 + 1;
    // 1 + 2; 2 + 2; 3 + 2;

create_lists()