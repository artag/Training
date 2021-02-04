module Capstone3.Program

open Commands

open System

[<EntryPoint>]
let main argv =
    let name =
        Console.Write "Please enter your name: "
        Console.ReadLine()

    let accountId, transactions = FileRepository.findTransactionsOnDisk name 
    let owner = { Name = name }
    let openingAccount = Operations.loadAccount accountId owner transactions
    Console.WriteLine $"Current Balance is ${openingAccount.Balance}"

    let closingAccount =
        let consoleCommands = seq {
            while true do
                Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
                yield Console.ReadKey().KeyChar }

        consoleCommands
        |> Seq.filter isValidCommand
        |> Seq.takeWhile (not << isStopCommand)
        |> Seq.map getAmountConsole
        |> Seq.fold processCommand openingAccount

    Console.Clear()
    printfn "Closing Balance: $%M" closingAccount.Balance
    Console.ReadKey() |> ignore
    0
