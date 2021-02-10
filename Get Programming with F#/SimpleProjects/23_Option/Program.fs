module Program

open System
open System.IO

let fileExists filename =
    if (File.Exists filename) then
        Some filename
    else
        None

let printResult result =
    match result with
    | Some file -> printfn "File %s is exists" file
    | None -> printfn "File is not found"

[<EntryPoint>]
let main argv =
    if argv.Length < 1 then
        printfn "First argument must be filename"
        Environment.Exit(1)

    printResult (fileExists argv.[0])
    0