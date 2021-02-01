module Capstone3.Program

open Operations

open System

[<EntryPoint>]
let main argv =
    let name =
        Console.Write "Please enter your name: "
        Console.ReadLine()

    let withdrawWithAudit = auditAs "withdraw" Auditing.printTransaction withdraw
    let depositWithAudit = auditAs "deposit" Auditing.printTransaction deposit

    let openingAccount = { Owner = { Name = name }; Balance = 0M; Id = Guid.Empty }

    let closingAccount =
        // Fill in the main loop here...
        openingAccount

    Console.Clear()
    printfn "Closing Balance:\r\n %A" closingAccount
    Console.ReadKey() |> ignore
    0
