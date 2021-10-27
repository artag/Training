namespace ReaderMonad

type ILogger =
    abstract Debug : string -> unit
    abstract Info : string -> unit
    abstract Error : string -> unit

type IConsole =
    abstract ReadLn : unit -> string
    abstract WriteLn : string -> unit

type ComparisonResult =
    | Bigger
    | Smaller
    | Equal
