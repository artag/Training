open System

type Customer = { Name : string }

type Account =
    { Id : System.Guid
      Owner : Customer
      Balance : decimal }

let deposit amount account =
    { account with
        Balance = account.Balance + amount }

let withdraw amount account =
    if amount <= account.Balance then
        { account with
            Balance = account.Balance - amount }
    else account

let account : Account =
    { Id = Guid.NewGuid()
      Owner = { Name = "Petya" }
      Balance = 100M }

account |> deposit 50M |> withdraw 25M |> deposit 10M |> withdraw 300M

let consoleAudit account message =
    Console.WriteLine $"Account {account.Id}: {message}"

let newAccount = account |> withdraw 10M
newAccount.Balance = 90M

consoleAudit account "Test console audit"

let auditAs (operationName:string) (audit:Account->string->unit) (operation:decimal->Account->Account) (amount:decimal) (account:Account) : Account =
    audit account operationName
    operation amount account

let withdrawWithConsoleAudit = auditAs "withdraw" consoleAudit withdraw
let depositWithConsoleAudit = auditAs "deposit" consoleAudit deposit

account
|> depositWithConsoleAudit 100M
|> withdrawWithConsoleAudit 50M

