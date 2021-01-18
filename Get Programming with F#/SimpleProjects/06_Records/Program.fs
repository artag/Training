open System

type Address =
    { Street : string
      Town : string
      City : string }

let print message result =
    printfn "%s Result: %b" message result

[<EntryPoint>]
let main argv =
    let address1 =
        { Street = "The Street"
          Town = "The Town"
          City = "The City" }

    let address2 =
        { Street = "The Street"
          Town = "The Town"
          City = "The City" }

    let compare1 = (address1 = address2)
    print " =                           " compare1             // true

    let compare2 = address1.Equals address2
    print ".Equals                      " compare2             // true

    let compare3 = Object.ReferenceEquals(address1, address2)
    print "System.Object.ReferenceEquals" compare3             // false
    0
