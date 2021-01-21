module Capstone2.Operations

let deposit amount account =
    { account with
        Balance = account.Balance + amount }

let withdraw amount account =
    if amount <= account.Balance then
        { account with
            Balance = account.Balance - amount }
    else account

let auditAs (operationName:string) (audit:Account->string->unit) (operation:decimal->Account->Account) (amount:decimal) (account:Account) : Account =
    let message = sprintf "Performing a %s operation for $%M..." operationName account.Balance
    audit account message
    let updatedAccount = operation amount account
    if updatedAccount.Balance = account.Balance then
        let message = "Trancaction rejected!"
        audit account message
        account
    else
        let message = sprintf "Transaction accepted! Balance is now $%M" updatedAccount.Balance
        audit account message
        updatedAccount

