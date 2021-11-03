module Logic
open System

let sumNumbers numbers =
    numbers |> List.fold (+) 0

let failedSumNumbers numbers =
    if numbers |> List.contains 5 then -1
    else numbers |> List.fold (+) 0

let flipCase (text:string) =
    text.ToCharArray()
    |> Array.map(fun c ->
        if Char.IsUpper c then Char.ToLower c
        else Char.ToUpper c)
    |> String
