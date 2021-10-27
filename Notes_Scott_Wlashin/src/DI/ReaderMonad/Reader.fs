module ReaderMonad.Reader

type Reader<'env, 'a> = Reader of action:('env -> 'a)

/// Run a Reader with a given environment
let run env (Reader action) =
    action env

/// Create a Reader which returns the environment itself
let ask = Reader id

/// Map a function over a Reader
let map f reader =
    Reader (fun env -> f (run env reader))

/// flatMap a function over a Reader
let bind f reader =
    let newAction env =
        let x = run env reader
        run env (f x)
    Reader newAction

type ReaderBuilder() =
    member _.Return(x) = Reader (fun _ -> x)
    member _.Bind(x,f) = bind f x
    member _.Zero() = Reader (fun _ -> ())

let reader = ReaderBuilder()
