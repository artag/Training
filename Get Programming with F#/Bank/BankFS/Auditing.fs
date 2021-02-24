/// Аудит.
module BankFS.Auditing

open System

/// Отобразить транзакцию на консоль.
let printToConsole transaction ratedAccount =
    let account = Account.unwrap ratedAccount
    let operation = Transaction.getPrintableOperation transaction
    Console.WriteLine($"Account {account.Owner.Name}: {operation} of {transaction.Amount}")
    Console.WriteLine($"Current balance is ${(account.Balance)}")

/// Сохранить транзакцию в файловую систему.
let saveToFilesystem transaction ratedAccount =
    let account = Account.unwrap ratedAccount
    FileRepository.writeTransaction transaction account

/// Логировать в консоль и в файловую систему.
let logComposite transaction ratedAccount =
    let loggers = [ printToConsole; saveToFilesystem ]
    loggers |> List.iter(fun logger -> logger transaction ratedAccount)
