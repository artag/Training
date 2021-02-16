module Capstone4.Program

open System
open Capstone4.Domain
open Capstone4.Operations

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit
let tryLoadAccountFromDisk = FileRepository.tryFindTransactionsOnDisk >> Operations.loadAccountOptional

[<AutoOpen>]
module UserInput =
    let commands = seq {
        while true do
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            yield Console.ReadKey().KeyChar
            Console.WriteLine() }

    //let getAmount command =
    //    Console.WriteLine()
    //    Console.Write "Enter Amount: "
    //    command, Console.ReadLine() |> Decimal.Parse

    let tryGetAmount command =
        Console.WriteLine()
        Console.Write "Enter Amount: "
        let amount = Console.ReadLine() |> Decimal.TryParse
        match amount with
        | true, amount -> Some(command, amount)
        | false, _ -> None

[<EntryPoint>]
let main _ =
    let openingAccount =
        Console.Write "Please enter your name: "
        let owner = Console.ReadLine()

        match (tryLoadAccountFromDisk owner) with
        | Some account -> account
        | None -> {
            Balance = 0M
            AccountId = Guid.NewGuid()
            Owner = { Name = owner }
        }

    printfn "Current balance is $%M" openingAccount.Balance

    let processCommand account (command, amount) =
        printfn ""
        let account =
            match command with
            | Deposit -> account |> depositWithAudit amount
            | Withdraw -> account |> withdrawWithAudit amount

        printfn "Current balance is $%M" account.Balance
        account

    let closingAccount =
        commands
        |> Seq.choose CommandOperation.tryParseCommand
        |> Seq.takeWhile((<>) Command.Exit)
        |> Seq.choose CommandOperation.tryGetBankOperation
        |> Seq.choose tryGetAmount
        |> Seq.fold processCommand openingAccount

    printfn ""
    printfn "Closing Balance:\r\n %M" closingAccount.Balance
    Console.ReadKey() |> ignore

    0