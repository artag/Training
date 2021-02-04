module Capstone3.Commands

open Operations
open System

let isValidCommand command =
    if command = 'd' || command = 'w' || command = 'x' then true
    else false

let isStopCommand command =
    if command = 'x' then true
    else false

let getAmount command =
    if command = 'd' then ('d', 50M)
    elif command = 'w' then ('w', 25M)
    else ('x', 0M)

let getAmountConsole command =
    Console.Write "\nEnter Amount: "
    let readedValue = Console.ReadLine()
    let amount = Decimal.Parse readedValue
    if command = 'd' then ('d', amount)
    elif command = 'w' then ('w', amount)
    else ('x', 0M)

let withdrawWithAudit = auditAs "withdraw" Auditing.composedLogger withdraw
let depositWithAudit = auditAs "deposit" Auditing.composedLogger deposit

let processCommand account (command, amount) =
    let newAccount =
        if command = 'd' then
            depositWithAudit amount account
        elif command = 'w' then
            withdrawWithAudit amount account
        else
            account
    Console.WriteLine($"Current balance is ${newAccount.Balance}")
    newAccount
