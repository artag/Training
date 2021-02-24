namespace BankFS

open System

/// Транзакция.
type TransactionType = {
    Date : DateTime
    Operation : AccountOperationType
    Amount : decimal
}

/// Транзакция.
module Transaction =
    open Common

    /// Создать транзакцию.
    let create date operation amount = {
        Date = date
        Operation = operation
        Amount = amount }

    /// Создать пустую транзакцию.
    let createEmpty = create DateTime.MinValue Deposit 0M

    /// Десериализовать строку в транзакцию.
    let deserialize (record : string) =
        let splitted = record.Split([| "***" |], StringSplitOptions.RemoveEmptyEntries)
        let dateOption = splitted |> Array.tryGetString 0 |> Parser.tryParseDateTimeOption
        let operationOption = splitted |> Array.tryGetString 1 |> AccountOperation.tryParseStringOption
        let amountOption = splitted |> Array.tryGetString 2 |> Parser.tryParseDecimalOption
        match dateOption, operationOption, amountOption with
        | Some date, Some operation, Some amount ->
            create date operation amount
        | _, _, _ ->
            createEmpty

    /// Десериализовать строки в транзакции. Сортировать транзакции по возрастанию даты.
    let deserializeAndSortByDate (records : string seq) =
        let dateTime transaction = transaction.Date
        records
        |> Seq.map(deserialize)
        |> Seq.toList
        |> List.sortBy dateTime

    /// Преобразовать транзакцию в операцию пополнения или снятия денег с баланса банковского счета.
    let parse account transaction =
        match transaction.Operation with
        | Deposit -> Account.deposit transaction.Amount account
        | Withdraw ->
            match account with
            | InCredit acc ->
                let updatedAccount = Account.withdraw transaction.Amount acc
                updatedAccount
            | Overdrawn _ ->
                account

    /// Получить тип операции из транзакции в виде, пригодном для вывода куда-либо.
    let getPrintableOperation transaction =
        match transaction.Operation with
        | Deposit -> "deposit"
        | Withdraw -> "withdraw"
