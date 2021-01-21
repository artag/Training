open Capstone2
open Operations
open System

let withdrawWithAudit = withdraw |> auditAs "withdraw" Auditing.console (*Auditing.filesystem*)
let depositWithAudit = deposit |> auditAs "deposit" Auditing.console (*Auditing.filesystem*)

let enterCustomer() =
    Console.Write "Please enter your name: "
    let name = Console.ReadLine()
    { Name = name }

let enterBalance() =
    Console.Write "Enter opening balance: $"
    Console.ReadLine() |> Decimal.Parse

let openAccount() =
    let customer = enterCustomer()
    let balance = enterBalance()
    { Id = Guid.NewGuid()
      Owner = customer
      Balance = balance }

let action() =
    Console.Write("(d)eposit, (w)ithdraw or e(x)it: ")
    Console.ReadLine()

let amount() =
    Console.Write("Amount: ")
    Console.ReadLine() |> Decimal.Parse

[<EntryPoint>]
let main argv =
    let mutable account = openAccount()
    Console.WriteLine($"Current balance is ${account.Balance}")

    while true do
        let action = action()
        if action = "x" then Environment.Exit 0

        let amount = amount()
        account <-
            if action = "d" then account |> depositWithAudit amount
            elif action = "w" then account |> withdrawWithAudit amount
            else account
    0