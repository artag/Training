#load "Common.fs"
#load "Owner.fs"
#load "Account.fs"
#load "AccountOperation.fs"
#load "Transaction.fs"

open BankFS
open System
open System.IO

let sam = Owner.create "Sam"
let joe = Owner.create "Joe"

let str = "11.02.2021 19:47:51***deposit***10***true"

//let account =
//    let commands = seq { 'd'; 'w'; 'z'; 'f'; 'd'; 'x'; 'w' }
//    commands
//    |> Seq.choose Command.tryParseChar
//    |> Seq.takeWhile (not << Command.isExit)

let s = "D:\Wow\Some_dir\Sam_3ef6455b-35f2-41bd-b915-07c66703d8e7"

let tryGetAccountId (directoryName : string) =
    let splitted = directoryName.Split([|"_"|], StringSplitOptions.RemoveEmptyEntries)
    let s = splitted |> Array.last
    let id = s |> Common.Parser.tryParseGuid
    id

tryGetAccountId s
