module Capstone3.Operations

open System

/// Withdraws an amount of an account (if there are sufficient funds)
let withdraw amount account =
    if amount > account.Balance then account
    else { account with Balance = account.Balance - amount }

/// Deposits an amount into an account
let deposit amount account =
    { account with Balance = account.Balance + amount }

/// Runs some account operation such as withdraw or deposit with auditing.
let auditAs operationName (audit : Guid -> string -> Transaction -> unit) operation amount account =
    let updatedAccount = operation amount account

    let accountIsUnchanged = (updatedAccount = account)

    let transaction =
        let transaction = { Operation = operationName; Amount = amount; Accepted = true; Timestamp = DateTime.Now }
        if accountIsUnchanged then { transaction with Accepted = false }
        else transaction

    audit account.Id account.Owner.Name transaction
    updatedAccount

let loadAccount id owner transactions =
    let sortedTransactions =
        transactions
        |> Seq.sortBy(fun t -> t.Timestamp)

    let updateAccount transaction account =
        if transaction.Operation = "deposit" then
            deposit transaction.Amount account
        elif transaction.Operation = "withdraw" then
            withdraw transaction.Amount account
        else
            account

    let account = { Id = id; Owner = owner; Balance = 0M }
    (account, sortedTransactions)||> Seq.fold(fun acc trans -> updateAccount trans acc)