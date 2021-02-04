#load "Domain.fs"
#load "Operations.fs"
#load "Transactions.fs"

open Capstone3
open System
open System.IO

let isValidCommand command =
    if command = 'd' || command = 'w' || command = 'x' then true
    else false

let isStopCommand command =
    if command = 'x' then true
    else false

let getAmount command =
    if command = 'd' then ('d', 50M)
    elif command = 'w' then ('w', 25M)
    else ('x', 0M)

let processCommand account (command, amount) =
    if command = 'd' then
        { account with Balance = account.Balance + amount }
    elif command = 'w' then
        let canWithdraw = account.Balance >= amount
        if canWithdraw then
            { account with Balance = account.Balance - amount }
        else account
    else
        account

let openingAccount = {
    Owner = { Name = "Isaac"};
    Balance = 0M;
    Id = Guid.Empty }

let account =
    let commands = [ 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' ]

    commands
    |> Seq.filter isValidCommand
    |> Seq.takeWhile (not << isStopCommand)
    |> Seq.map getAmount
    |> Seq.fold processCommand openingAccount

let t1 = { Operation = "deposit"; Amount = 25M; Accepted = true; Timestamp = DateTime(2021, 01, 05) }
let t2 = { Operation = "deposit"; Amount = 4M; Accepted = true; Timestamp = DateTime(2021, 01, 03) }
let t3 = { Operation = "withdraw"; Amount = 33M; Accepted = false; Timestamp = DateTime(2021, 01, 06) }

let loadAccount id owner transactions =
    let sortedTransactions =
        transactions
        |> Seq.sortBy(fun t -> t.Timestamp)

    let updateAccount transaction account =
        if transaction.Operation = "deposit" then
            Operations.deposit transaction.Amount account
        elif transaction.Operation = "withdraw" then
            Operations.withdraw transaction.Amount account
        else
            account

    let account = { Id = id; Owner = owner; Balance = 0M }
    (account, sortedTransactions)||> Seq.fold(fun acc trans -> updateAccount trans acc)

loadAccount Guid.Empty { Name = "Sam" } [ t1; t2; t3]

let deserialize (record : string) =
    let splitted = record.Split([|"***"|], StringSplitOptions.None)
    {
        Timestamp = DateTime.Parse splitted.[0]
        Operation = splitted.[1]
        Amount = Decimal.Parse splitted.[2]
        Accepted = Boolean.Parse splitted.[3]
    }

let record = "04.02.2021 21:19:54***withdraw***4***true"

deserialize record

let accountsPath =
    let path = @"accounts"
    Directory.CreateDirectory path |> ignore
    path

let findAccountFolder owner =
    let folders = Directory.EnumerateDirectories(@"D:\Git\Training\Get Programming with F#\SimpleProjects\18_Capstone3\bin\Debug\netcoreapp3.1\accounts", sprintf "%s_*" owner)
    if Seq.isEmpty folders then ""
    else
        let folder = Seq.head folders
        DirectoryInfo(folder).Name

findAccountFolder "Sam"

let readFiles dir =
    let files = Directory.GetFiles(@"D:\Git\Training\Get Programming with F#\SimpleProjects\18_Capstone3\bin\Debug\netcoreapp3.1\accounts\Sam_00000000-0000-0000-0000-000000000000\")
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

findTransactionsOnDisk "Sam"





