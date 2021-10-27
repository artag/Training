module ReaderMonad.DefaultServices
open System
let defaultLogger = {
    new ILogger with
    member _.Debug str = printfn $"DEBUG %s{str}"
    member _.Info str = printfn $"INFO %s{str}"
    member _.Error str = printfn $"ERROR %s{str}"
}

let defaultConsole = {
    new IConsole with
    member _.ReadLn() = Console.ReadLine()
    member _.WriteLn str = Console.WriteLine $"%s{str}"
}
