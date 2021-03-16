/// Provides access to the banking API.
module Capstone5.Api

open Capstone5.Domain
open Capstone5.Operations
open System

let private createNewAccount (customer:Customer) =
    InCredit(CreditAccount { AccountId = Guid.NewGuid()
                             Balance = 0M
                             Owner = customer })

/// Loads an account from disk. If no account exists, an empty one is automatically created.
let LoadAccount (customer:Customer) : RatedAccount =
    let loadedData =
        customer.Name
        |> FileRepository.tryFindTransactionsOnDisk
        |> Option.map Operations.loadAccount
    let loadedAccount =
        loadedData
        |> Option.defaultValue (createNewAccount customer)
    loadedAccount

/// Deposits funds into an account.
let Deposit (amount:decimal) (customer:Customer) : RatedAccount =
    let loadedAccount = LoadAccount customer
    let accountId = loadedAccount.GetField (fun a -> a.AccountId)
    auditAs "deposit" Auditing.composedLogger deposit amount loadedAccount accountId customer

/// Withdraws funds from an account that is in credit.
let Withdraw (amount:decimal) (customer:Customer) : RatedAccount =
    let loadedAccount = LoadAccount customer
    match loadedAccount with
    | InCredit (CreditAccount account as creditAccount) -> auditAs "withdraw" Auditing.composedLogger withdraw amount creditAccount account.AccountId customer
    | loadedAccount -> loadedAccount

/// Loads the transaction history for an owner. If no transactions exist, returns an empty sequence.
let LoadTransactionHistory(customer:Customer) : Transaction seq =
    customer.Name
    |> FileRepository.tryFindTransactionsOnDisk
    |> Option.map(fun (_, _, tr) -> tr)
    |> Option.defaultValue Seq.empty
