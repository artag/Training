module Capstone3.Transactions

open System

let serialize transaction =
    sprintf "%O***%s***%M***%b"
        transaction.Timestamp
        transaction.Operation
        transaction.Amount
        transaction.Accepted

let deserialize (record : string) =
    let splitted = record.Split([|"***"|], StringSplitOptions.None)
    { Timestamp = DateTime.Parse splitted.[0]
      Operation = splitted.[1]
      Amount = Decimal.Parse splitted.[2]
      Accepted = Boolean.Parse splitted.[3] }