type Customer = {
    Id : int
    Name : string
    Score : int option
}

let sam = { Id = 1; Name = "Sam"; Score = None }
let joe = { Id = 2; Name = "Joe"; Score = Some -100 }
let roy = { Id = 3; Name = "Roy"; Score = Some 100 }
let bob = { Id = 4; Name = "Bob"; Score = Some 0 }

let customers = [ sam; joe; roy; bob ]

let calculatePremiumUsd customer =
    match customer.Score with
    | Some score when score = 0 -> 250
    | Some score when score < 0 -> 400
    | Some score when score > 0 -> 140
    | None ->
        printfn "No score supplied! Using temporary premium."
        300

let calculatePremiumsUsd customers =
    customers |> Seq.map(fun customer -> calculatePremiumUsd customer)

calculatePremiumsUsd customers
calculatePremiumUsd sam

None = Some 10      // false
None > Some 10      // false
None < Some 10      // true
None <> Some 10     // true
