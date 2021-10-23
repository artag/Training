module PureCode
open System
open Logger

type IConsole =
    abstract ReadLn: unit -> string
    abstract WriteLn: string -> unit

let compareTwoStrings (comparison:StringComparison) (logger:ILogger) (console:IConsole) =
    logger.Debug "compareTwoStrings: Starting"

    console.WriteLn "Enter the first value"
    let str1 = console.ReadLn()
    console.WriteLn "Enter the second value"
    let str2 = console.ReadLn()

    let result = String.Compare(str1, str2, comparison)
    logger.Info $"compareTwoStrings: result=%A{result}"

    if result > 0 then
        console.WriteLn "The first value is bigger"
    else if result < 0 then
        console.WriteLn "The first value is smaller"
    else
        console.WriteLn "The values are equal"

    logger.Debug $"compareTwoStrings: Finished"

let compareCaseSensitive: ILogger -> IConsole -> unit =
    compareTwoStrings StringComparison.CurrentCulture

let compareCaseInsensitive: ILogger -> IConsole -> unit =
    compareTwoStrings StringComparison.CurrentCultureIgnoreCase