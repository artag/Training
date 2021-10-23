open System
open Domain
open StringComparer

let program (logger:ILogger) (console:IConsole) =
    let input() =
        console.WriteLn "Enter the first value"
        let str1 = console.ReadLn()
        console.WriteLn "Enter the second value"
        let str2 = console.ReadLn()
        str1, str2

    let output = function
        | Bigger -> printfn "The first value is bigger"
        | Smaller -> printfn "The first value is smaller"
        | Equal -> printfn "The values are equal"

    let str1, str2 = input()
    let stringComparisons = StringComparisons(logger, StringComparison.CurrentCulture)
    stringComparisons.CompareTwoStrings str1 str2 |> output

[<EntryPoint>]
let main argv =
    let logger = {
        new ILogger with
            member this.Debug s = Console.WriteLine $"DEBUG: %s{s}"
            member this.Info s = Console.WriteLine $"INFO: %s{s}"
            member this.Error s = Console.WriteLine $"ERROR: %s{s}"
        }

    let console = {
        new IConsole with
            member this.ReadLn() = Console.ReadLine()
            member this.WriteLn str = printfn $"%s{str}"
        }

    program logger console
    0