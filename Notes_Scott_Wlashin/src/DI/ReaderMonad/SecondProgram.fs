module ReaderMonad.SecondProgram
open Reader

type Services = {
    Logger : ILogger
    Console : IConsole
}

let services : Services = {
        Console = DefaultServices.defaultConsole
        Logger = DefaultServices.defaultLogger
    }

/// Transform a Reader's environment from subtype to supertype
let withEnv (f:'superEnv->'subEnv) reader =
    Reader (fun superEnv -> (run(f superEnv) reader))

let run = reader {
    // helper functions to transform the environment
    let getConsole services = services.Console
    let getLogger services = services.Logger
    let getConsoleAndLogger services = services.Console,services.Logger     // a tuple

    let! str1, str2 =
        SecondImpl.readFromConsole()
        |> withEnv getConsoleAndLogger

    let! result =
        SecondImpl.compareTwoStrings str1 str2
        |> withEnv getLogger

    do! SecondImpl.writeToConsole result
        |> withEnv getConsole
}
