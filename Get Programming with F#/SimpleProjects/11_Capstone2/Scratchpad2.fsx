#load "Domain.fs"
#load "Operations.fs"
#load "Auditing.fs"

open Capstone2
open Capstone2.Operations
open Capstone2.Auditing
open System

let withdraw = withdraw |> auditAs "withdraw" console
let deposit = deposit |> auditAs "deposit" console

let customer = { Name = "Isaak" }
let account = { Id = Guid.NewGuid(); Owner = customer; Balance = 90M }

account
|> withdraw 50M
|> deposit 50M
|> deposit 100M
|> withdraw 50M
|> withdraw 350M
