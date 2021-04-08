(*
    Exception Handling
*)
open System
let riskyCode() =
    raise(ApplicationException("From risky code!"))
    ()

let runSafely() =
    try
    riskyCode()
    with
    | :? ApplicationException as ex -> printfn "App exception! %O" ex
    | :? MissingFieldException as ex -> printfn "Missing field! %O" ex
    | ex -> printfn "Got some other type of exception! %O" ex

runSafely()

(*
    Resource management
*)
let createDisposable() =
    printfn "Created!"
    { new IDisposable with member __.Dispose() = printfn "Disposed!"}

let foo() =
    use x = createDisposable()
    printfn "inside!"

let bar() =
    using (createDisposable()) (fun disposableObject -> printfn "inside!")

foo()
bar()

(*
    Casting
*)
let anException = Exception()

// Safely upcasting to Object
let upcastToObject = anException :> obj                             // Success

// Trying to safely upcast to an incompatible type (error)
let upcastToAppException = anException :> ApplicationException      // Error
// Display:
// Type constraint mismatch. The type 'Exception' is not compatible with type 'ApplicationException'

// Unsafely downcasting to an ApplicationException
let downcastToAppException = anException :?> ApplicationException   // InvalidCastException
// Display:
// System.InvalidCastException: Unable to cast object of type 'System.Exception' to type 'System.ApplicationException'.

let downcastToString = anException :?> string                       // Error
// Display:
// Type constraint mismatch. The type 'string' is not compatible with type 'Exception'    
