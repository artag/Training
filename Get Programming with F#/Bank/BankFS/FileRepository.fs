/// Файловый репозиторий.
module BankFS.FileRepository

open System
open System.IO

/// Корневая директория с транзакциями банковских счетов.
let private accountsDirectory =
    let accounts = @"E:\Temp\Accounts"
    Directory.CreateDirectory accounts |> ignore
    accounts

/// Создать путь для транзакций опеределенного банковского счета.
let private buildPath account =
    sprintf @"%s\%s_%O" accountsDirectory account.Owner.Name account.Id

/// Найти директорию с транзакциями банковского счета определенного пользователя.
let private tryFindAccountDirectory owner =
    let searchPattern = sprintf "%s_*" owner.Name
    let findedFolders = Directory.EnumerateDirectories(accountsDirectory, searchPattern) |> Seq.toList
    match findedFolders with
    | folders when not folders.IsEmpty ->
        let firstFolder = folders.Head
        Some firstFolder
    | _ -> None

/// Получить файлы транзакций банковского счета опрелеленного пользователя.
let private tryGetFiles owner =
    let result = tryFindAccountDirectory owner
    match result with
    | None -> None
    | Some dir ->
        let files = Directory.EnumerateFiles(dir)
        Some files

/// Прочитать данные из файлов транзакций банковского счета опрелеленного пользователя.
let private tryReadData owner =
    let files = tryGetFiles owner
    match files with
    | None -> None
    | Some files -> 
        let data = files |> Seq.collect(File.ReadLines)
        Some data

/// Получить id банковского счета определенного пользователя.
let tryGetAccountId owner =
    let folderName = tryFindAccountDirectory owner
    match folderName with
    | None -> None
    | Some folder -> 
        let splitted = folder.Split([|"_"|], StringSplitOptions.RemoveEmptyEntries)
        let id = splitted |> Array.last |> Common.Parser.tryParseGuid
        id

/// Загрузить транзакции банковского счета опрелеленного пользователя.
let tryLoadTransactions owner =
    let loadedData = tryReadData owner
    match loadedData with
    | Some validData ->
        let transactions = Transaction.deserializeAndSortByDate validData
        transactions
    | None -> list.Empty

/// Записать транзакцию для банковского счета.
let writeTransaction transaction account =
    let path = buildPath account
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (transaction.Date.ToFileTimeUtc())
    let operation = Transaction.getPrintableOperation transaction
    let line = sprintf "%O***%s***%M" transaction.Date operation transaction.Amount
    File.WriteAllText(filePath, line)