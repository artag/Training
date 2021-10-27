module ReaderMonad.SecondImpl
open Reader

let compareTwoStrings str1 str2 =
    reader {
        // ask for an ILogger
        let! (logger:#ILogger) = Reader.ask
        logger.Debug "compareTwoStrings: Starting"

        let result =
            if str1 > str2 then
                Bigger
            else if str1 < str2 then
                Smaller
            else
                Equal

        logger.Info $"compareTwoStrings: result=%A{result}"
        logger.Debug "compareTwoStrings: Finished"
        return result
    }

let readFromConsole() =
    reader {
        // ask for an IConsole, ILogger pair
        let! (console:IConsole),(logger:ILogger) = Reader.ask     // a tuple

        logger.Debug "readFromConsole: Starting"

        console.WriteLn "Enter the first value"
        let str1 = console.ReadLn()
        console.WriteLn "Enter the second value"
        let str2 = console.ReadLn()

        logger.Debug "readFromConsole: Finished"

        return str1, str2
    }

let writeToConsole (result:ComparisonResult) =
    reader {
        // ask for an IConsole
        let! (console:#IConsole) = Reader.ask

        match result with
        | Bigger -> console.WriteLn "The first value is bigger"
        | Smaller -> console.WriteLn "The second value is smaller"
        | Equal -> console.WriteLn "The values are equal"
    }
