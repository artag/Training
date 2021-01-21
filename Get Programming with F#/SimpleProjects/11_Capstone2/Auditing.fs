module Capstone2.Auditing
open System
open System.IO

let console account message =
    Console.WriteLine($"Account {account.Id}: {message}")

let filesystem account message =
    Directory.CreateDirectory(account.Owner.Name) |> ignore
    let path = sprintf "%s\\%O.txt" account.Owner.Name account.Id
    File.AppendAllLines(path, [ $"Account {account.Id}: {message}" ])
