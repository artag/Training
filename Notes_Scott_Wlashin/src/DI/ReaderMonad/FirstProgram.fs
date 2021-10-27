module ReaderMonad.FirstProgram
open Reader
open DefaultServices

type IServices =
    inherit ILogger
    inherit IConsole
let services = {
    new IServices
    interface IConsole with
        member _.ReadLn() = defaultConsole.ReadLn()
        member _.WriteLn str = defaultConsole.WriteLn str
    interface ILogger with
        member _.Debug str = defaultLogger.Debug str
        member _.Info str = defaultLogger.Info str
        member _.Error str = defaultLogger.Error str
}

let run :Reader<IServices,_> = reader {
    let! str1, str2 = FirstImpl.readFromConsole()
    let! result = FirstImpl.compareTwoStrings str1 str2
    do! FirstImpl.writeToConsole result
}
