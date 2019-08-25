<Query Kind="FSharpProgram" />

open System

// Map - коллекции из key-value пар
let map_stuff() =
    // Map.empty - создание пустого map
    // Add - добавление пары
    let customers =
        Map.empty.
            Add("Bob Smith", 100.50).
            Add("Sally Marks", 50.25)
            
    printfn "# of Customers %i" customers.Count             // # of Customers 2

    printfn "Customers: %A" customers                       // Customers: map [("Bob Smith", 100.5); ("Sally Marks", 50.25)]

    // Поиск по ключу
    let cust = customers.TryFind "Bob Smith"
    match cust with
    | Some x -> printfn "Balance : %.2f" x                  // Balance : 100.50
    | None -> printfn "Not Found"

    // Поиск по ключу (другой способ)
    if customers.ContainsKey "Bob Smith" then
        printfn "Bob Smith was Found"                       // Bob Smith was Found
        
    printfn "Bobs Balance %.2f" customers.["Bob Smith"]     // Bobs Balance 100.50
    
    // Удалить одну пару и создать новый список    
    let cust2 = Map.remove "Sally Marks" customers
    printfn "# of Customers %i" cust2.Count                 // # of Customers 1

map_stuff()