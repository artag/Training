open System

let drive2 petrol distance =
    if distance > 50 then petrol / 2.0
    elif distance > 25 then petrol - 10.0
    elif distance > 0 then petrol - 1.0
    else petrol

[<EntryPoint>]
let main argv =
    let petrol = 100.0
    let firstState = drive2 petrol 51
    let secondState = drive2 firstState 26
    let thirdState = drive2 secondState 1
    let finalState = drive2 thirdState 0

    printfn "Initial petrol %f" petrol
    printfn "After distance 51: %f" firstState
    printfn "After distance 26: %f" secondState
    printfn "After distance 1: %f" thirdState
    printfn "After distance 0: %f" finalState

    0 // return an integer exit code
