module Capstone3.FileRepository

open Transactions
open System.IO
open System

let private accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path

let private findAccountFolder owner =
    let folders = Directory.EnumerateDirectories(accountsPath, sprintf "%s_*" owner)
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name

let private buildPath(owner, accountId:Guid) = sprintf @"%s\%s_%O" accountsPath owner accountId

/// Logs to the file system
let writeTransaction accountId owner transaction =
    let path = buildPath(owner, accountId)
    path |> Directory.CreateDirectory |> ignore
    let filePath = sprintf "%s/%d.txt" path (DateTime.UtcNow.ToFileTimeUtc())
    File.WriteAllText(filePath, serialize transaction)

let private readFiles dir =
    let path = accountsPath + "/" + dir
    let files = Directory.GetFiles path
    files
    |> Seq.map(fun file ->
        let data = File.ReadAllText file
        let transaction = data |> Transactions.deserialize
        transaction)

let findTransactionsOnDisk owner =
    let dir = findAccountFolder owner
    if (String.IsNullOrEmpty dir) then
        Guid.NewGuid(), Seq.empty
    else
        let splitted = dir.Split('_')
        let guid = Guid.Parse(splitted.[1])
        let transactions = readFiles dir
        guid, transactions