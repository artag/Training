//type Command =
//| Withdraw = 0
//| Deposit = 1
//| Exit = 2

//tryParse 'a'
//tryParse 'b'
//tryParse 'd'
//tryParse 'w'
//tryParse 'x'
//tryParse 'e'

//let isValidCommand cmd =
//    tryParse cmd |> Option.isSome

//isValidCommand 'a'
//isValidCommand 'b'
//isValidCommand 'd'
//isValidCommand 'w'
//isValidCommand 'x'
//isValidCommand 'e'

open System

type Customer = {
    Name : string
}

type Account = {
    AccountId : Guid
    Owner : Customer
    Balance : decimal
}


type BankOperation = Deposit | Withdraw
type Command = AccountCommand of BankOperation | Exit
let tryGetBankOperation cmd =
    match cmd with
    | Exit -> None
    | AccountCommand op -> Some op

let tryParseCommand symbol =
    match symbol with
    | 'd' -> Some (AccountCommand Deposit)
    | 'w' -> Some (AccountCommand Withdraw)
    | 'x' -> Some Exit
    | _ -> None


let getAmount cmd =
    match cmd with
    | Deposit -> (Deposit, 50M)
    | Withdraw -> (Withdraw, 25M)

let openingAccount =
    { Owner = { Name = "Isaac" }; Balance = 0M; AccountId = Guid.Empty }

let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

/// Deposits an amount into an account
let deposit amount account =
    { account with Balance = account.Balance + amount }

let processCommand account (command, amount) =
    printfn ""
    let account =
        match command with
        | Deposit -> account |> deposit amount
        | Withdraw -> account |> withdraw amount

    printfn "Current balance is $%M" account.Balance
    account

['a'; 'b'; 'd'; 'w'; 'd'; 'x'; 'e'; 'w']
|> Seq.choose tryParseCommand
|> Seq.takeWhile((<>) Command.Exit)
|> Seq.choose tryGetBankOperation
|> Seq.map getAmount
|> Seq.fold processCommand openingAccount
