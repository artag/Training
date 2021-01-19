open System

type Petrol = { Value : float }

let drive distance petrol  =
     if distance = "far" then { petrol with Value = petrol.Value / 2.0 }
     elif distance = "medium" then { petrol with Value = petrol.Value - 10.0 }
     else { petrol with Value = petrol.Value - 1.0 }

let petrol = { Value = 100.0 }

[<EntryPoint>]
let main argv =
    printfn "Petrol. Initial value %f" petrol.Value

    let finalPetrol =
        petrol
        |> drive "far"
        |> drive "medium"
        |> drive "short"

    printfn "Petrol. Final value %f" finalPetrol.Value
    0
