/// �������� �����������.
module BankFS.FileRepository

open System
open System.IO

/// �������� ���������� � ������������ ���������� ������.
let private accountsDirectory =
    let accounts = @"E:\Temp\Accounts"
    Directory.CreateDirectory accounts |> ignore
    accounts

/// ������� ���� ��� ���������� �������������� ����������� �����.
let private buildPath account =
    sprintf @"%s\%s_%O" accountsDirectory account.Owner.Name account.Id

/// ����� ���������� � ������������ ����������� ����� ������������� ������������.
let private tryFindAccountDirectory owner =
    let searchPattern = sprintf "%s_*" owner.Name
    let findedFolders = Directory.EnumerateDirectories(accountsDirectory, searchPattern) |> Seq.toList
    match findedFolders with
    | folders when not folders.IsEmpty ->
        let firstFolder = folders.Head
        Some firstFolder
    | _ -> None

/// �������� ����� ���������� ����������� ����� ������������� ������������.
let private tryGetFiles owner =
    let result = tryFindAccountDirectory owner
    match result with
    | None -> None
    | Some dir ->
        let files = Directory.EnumerateFiles(dir)
        Some files

/// ��������� ������ �� ������ ���������� ����������� ����� ������������� ������������.
let private tryReadData owner =
    let files = tryGetFiles owner
    match files with
    | None -> None
    | Some files -> 
        let data = files |> Seq.collect(File.ReadLines)
        Some data

/// �������� id ����������� ����� ������������� ������������.
let tryGetAccountId owner =
    let folderName = tryFindAccountDirectory owner
    match folderName with
    | None -> None
    | Some folder -> 
        let splitted = folder.Split([|"_"|], StringSplitOptions.RemoveEmptyEntries)
        let id = splitted |> Array.last |> Common.Parser.tryParseGuid
        id

/// ��������� ���������� ����������� ����� ������������� ������������.
let tryLoadTransactions owner =
    let loadedData = tryReadData owner
    match loadedData with
    | Some validData ->
        let transactions = Transaction.deserializeAndSortByDate validData
        transactions
    | None -> list.Empty

/// �������� ���������� ��� ����������� �����.
let writeTransaction transaction account =
    let path = buildPath account
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (transaction.Date.ToFileTimeUtc())
    let operation = Transaction.getPrintableOperation transaction
    let line = sprintf "%O***%s***%M" transaction.Date operation transaction.Amount
    File.WriteAllText(filePath, line)