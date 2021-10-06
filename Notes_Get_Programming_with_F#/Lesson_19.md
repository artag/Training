# Lesson 19. Capstone 3

## Creating a sequence of user-generated inputs

```fsharp
// (1) A sequence block
// (2) Yielding out keys sourced from the console
let consoleCommands = seq {                     // (1)
    while true do
        Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
        yield Console.ReadKey().KeyChar }       // (2)
```

This sequence will execute *forever*; every time the pipeline pulls another item from the
sequence of commands.

## Пример

```fsharp
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
```
