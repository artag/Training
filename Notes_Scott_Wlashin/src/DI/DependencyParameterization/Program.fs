module Program
open System
open Logger
open PureCode

let program() =
    let console = {
        new IConsole with
            member this.ReadLn() = Console.ReadLine()
            member this.WriteLn str = printfn $"%s{str}"
    }

    let logger = {
        new ILogger with
            member this.Debug s = Console.WriteLine $"DEBUG: %s{s}"
            member this.Info s = Console.WriteLine $"INFO: %s{s}"
            member this.Error s = Console.WriteLine $"ERROR: %s{s}"
    }

    compareCaseSensitive logger console

[<EntryPoint>]
let main argv =
    program()
    0
