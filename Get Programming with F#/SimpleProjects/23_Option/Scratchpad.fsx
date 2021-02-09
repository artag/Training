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

// int -> string
let describe score =
    match score with
    | 0 -> "Standard Risk"
    | score when score < 0 -> "Safe"
    | score when score > 0 -> "Hard Risk"

// Customer -> string option
let descriptionOne customer =
    match customer.Score with
    | Some score -> Some(describe score)
    | None -> None

// Customer -> string option
let descriptionTwo customer =
    customer.Score
    |> Option.map(fun score -> describe score)

// Customer -> string option
let descriptionThree customer =
    customer.Score |> Option.map describe

// int option -> string option
let optionalDescribe = Option.map describe

// Score 0
descriptionOne bob
descriptionTwo bob
descriptionThree bob
optionalDescribe bob.Score

// Score -100
descriptionOne joe
descriptionTwo joe
descriptionThree joe
optionalDescribe joe.Score

// Score 100
descriptionOne roy
descriptionTwo roy
descriptionThree roy
optionalDescribe roy.Score

// Score None
descriptionOne sam
descriptionTwo sam
descriptionThree sam
optionalDescribe sam.Score

let someValue = Some 99
let noneValue = None

let result value =
    match value with 
    | Some i -> Some(i * 2)
    | None -> None

result someValue
result noneValue

Some 99 |> Option.map (fun v -> v * 2)
None |> Option.map (fun v -> v * 2)

// Binding
let tryFindCustomer cId (customers : Customer list) =
    if cId = 1 then Some customers.[1]
    else None

let getScore customer = customer.Score

tryFindCustomer 2 customers |> Option.bind getScore
getScore joe

// Filtering
let test1 = Some 5 |> Option.filter(fun x -> x > 5)     // None
let test2 = None |> Option.filter(fun x -> x = 5)       // None
let test3 = Some 5 |> Option.filter(fun x -> x = 5)     // Some 5

// Iter
None |> Option.iter(fun n -> printfn "Num = %i" n)      // Нет печати
Some 0 |> Option.iter(fun n -> printfn "Num = %i" n)    // Num = 0
Some 1 |> Option.iter(fun n -> printfn "Num = %i" n)    // Num = 1
