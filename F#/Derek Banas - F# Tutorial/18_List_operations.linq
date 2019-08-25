<Query Kind="FSharpProgram" />

open System

let list_stuff() =
    let list = [1; 2; 3; 4]
   
    // Размер списка
    printfn "Length : %i" list.Length               // Length : 4
    
    // Пустой ли список
    printfn "Empty : %b" list.IsEmpty               // Empty : false
    
    // Второй элемент списка
    printfn "Index 2 : %i" (list.Item(2))           // Index 2 : 3
    
    // Первый элемент списка (голова)
    printfn "Head : %i" list.Head                   // Head : 1
    
    // Все элементы списка кроме первого (хвост)
    printfn "Tail : %A" list.Tail                   // Tail : [2; 3; 4]
    
    // Отфильтровать элементы списка (только четные числа) 
    let list3 = list |> List.filter (fun x -> x % 2 = 0)
    printfn "list3 = %A" list3                      // list3 = [2; 4]
    
    // Новый список, где каждый элемент из предыдущего списка возведен в квадрат
    let list4 = list |> List.map (fun x -> (x * x))
    printfn "list4 = %A" list4                      // list4 = [1; 4; 9; 16]
    
    // Сортировка списка (по возрастанию)
    printfn "Sorted : %A" (List.sort [5; 6; 4; 3])  // Sorted : [3; 4; 5; 6]
    
    // Суммирование значений в списке
    printfn "Sum: %i" (List.fold (fun sum elem -> sum + elem) 0 [1; 2; 3])  // Sum: 6
    
list_stuff()