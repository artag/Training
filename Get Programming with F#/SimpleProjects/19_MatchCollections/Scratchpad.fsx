type Customer = {
    Balance : int
    Name : string
}

// Matching against lists
let handleCustomers (customers : list<'a>) =
    if customers.Length < 1 then failwith "No customers supplied!"
    elif customers.Length = 1 then printfn "Customer name: %s" customers.[0].Name
    elif customers.Length = 2 then printfn "Sum of balance: %i" (customers.[0].Balance + customers.[1].Balance)
    else printfn "Customers supplied: %d" customers.Length

let joe = { Balance = 10; Name = "Joe" }
let sam = { Balance = 6; Name = "Sam" }
let roy = { Balance = 12; Name = "Roy" }
let isaak = { Balance = 50; Name = "Isaak" }
let ivan = { Balance = 50; Name = "Ivan" }
let peter = { Balance = 0; Name = "Peter" }

handleCustomers [] // throws exception
handleCustomers [ joe ] 
handleCustomers [ joe; sam ] 
handleCustomers [ joe; sam; roy ]

let handleCustomers2 customers =
    match customers with
    | [] -> failwith "No customers supplied!"
    | [ customer ] -> printfn "Single customer, name is %s" customer.Name
    | [ first; second ] ->
        printfn "Two customers, balance = %d" (first.Balance + second.Balance)
    | customers -> printfn "Customers supplied: %d" customers.Length

handleCustomers2 [] // throws exception
handleCustomers2 [ joe ] 
handleCustomers2 [ joe; sam ] 
handleCustomers2 [ joe; sam; roy ]

// Pattern matching with records
let getStatus customer =
    match customer with
    | { Balance = 0 } -> "Customer has empty balance!"
    | { Name = "Isaak" } -> "This is a great customer!"
    | { Name = name; Balance = 50 } -> sprintf "%s has a large balance!" name
    | { Name = name } -> sprintf "%s is a normal customer" name

getStatus peter
getStatus isaak
getStatus ivan
getStatus joe


let tanya = { Balance = 5; Name = "Tanya" }
let pit = { Balance = 25; Name = "Pit" }
let unknown = { Balance = 125; Name = "_" }

// 1 The list of customers has three elements.
// 2 The first customer is called “Tanya.”
// 3 The second customer has a balance of 25.
let getStatus2 customers =
    match customers with
    | [ { Name = "Tanya" }; { Balance = 25 }; _ ] -> "It's a match"
    | _ -> "Not match"

getStatus2 [ tanya; pit; unknown ]      // "It's a match"
getStatus2 [ tanya; pit; ]              // "Not match"
