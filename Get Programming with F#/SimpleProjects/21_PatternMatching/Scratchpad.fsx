// Pattern matching on list
open System

let randomList() =
    let rnd = Random()
    let length = rnd.Next(0, 10)
    let list = List.init length (fun item -> rnd.Next(0, 10))
    list

let checkList (numbers : int list) =
    printfn "%A" numbers
    match numbers with
    | numbers when numbers.Length = 7 -> printfn "List has seven numbers"
    | [] -> printfn "List is empty"
    | head::tail when head = 5 -> printfn "The first number is %i" head
    | _ -> printfn "Not match"

checkList [1; 2; 3; 4; 5; 6; 7]
checkList []
checkList [5; 2; 6; 9; 3; 4]
checkList [10; 5; 2; 6; 9; 3; 4; 5]

checkList (randomList())

// Pattern matching on record
type Person = { Name : string; Age: int; Balance: int }

let getRandomAge() =
    let rnd = Random()
    let age = rnd.Next(1, 120)
    age

let getRandomBalance() =
    let rnd = Random()
    rnd.Next(0, 120)

let getRandomSam() =
    { Name = "Sam"; Age = getRandomAge(); Balance = getRandomBalance() }

let checkAge person =
    match person with
    | p when p.Age < 10 -> printfn "Child"
    | p when p.Age < 18 -> printfn "Teen"
    | _ -> printfn "Adult"

let checkBalance person =
    match person with
    | p when p.Age < 10 -> printfn "Child. No money"
    | p when p.Balance < 25 -> printfn "Poor guy"
    | p when p.Balance < 75 -> printfn "Normal"
    | _ -> printfn "Rich man!"

let checkPerson person =
    printfn "%A" person
    checkAge person
    checkBalance person

checkPerson (getRandomSam())
