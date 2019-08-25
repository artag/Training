<Query Kind="FSharpProgram" />

open System

// Records - список key-value пар, для создания custom types
type customer =
    { Name : string;
    Balance : float }
    
let record_stuff() =
    let bob = { Name = "Bob Smith"; Balance = 101.50 }
    printfn "%s owes us %.2f" bob.Name bob.Balance     // Bob Smith owes us 101.50

record_stuff()