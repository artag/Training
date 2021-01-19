open System

let drive distance petrol =
    if distance = "far" then petrol / 2.0
    elif distance = "medium" then petrol - 10.0
    else petrol - 1.0

[<EntryPoint>]
let main argv =
    let startPetrol = 100.0
    printfn "Start petrol: %f" startPetrol

    let finalPetrol =
        startPetrol
        |> drive "far"
        |> drive "medium"
        |> drive "short"

    printfn "Final petrol: %f" finalPetrol
    0 // return an integer exit code
