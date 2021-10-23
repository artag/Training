module Logger

type ILogger =
    abstract Debug : string -> unit
    abstract Info : string -> unit
    abstract Error : string -> unit
