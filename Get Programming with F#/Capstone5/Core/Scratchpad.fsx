#r @"c:\Users\temy4\.nuget\packages\newtonsoft.json\12.0.3\lib\netstandard2.0\Newtonsoft.Json.dll"

#load "Domain.fs"
#load "Operations.fs"

open Capstone5.Operations
open Capstone5.Domain
open Newtonsoft.Json
open System

let txn = {
    Transaction.Amount = 100M
    Timestamp = DateTime.UtcNow
    Operation = "withdraw" }

let serialized = txn |> JsonConvert.SerializeObject
let deserialized = JsonConvert.DeserializeObject<Transaction>(serialized)
