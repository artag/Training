open System

type Address =
    { Street : string
      Town : string
      City: string }

type Customer =
    { Forename : string
      Surname : string
      Age : int
      Address : Address
      EmailAddress : string }

let getRandomAge min max =
    let rnd = Random()
    rnd.Next(min, max)

let customer = 
    { Forename = "Joe"
      Surname = "Bloggs"
      Age = 30
      Address =
        { Street = "The Street"
          Town = "The Town"
          City = "The City" }
      EmailAddress = "joe@bloggs.com" }

let getNewCustomer oldCustomer =
    let newCustomer =
        { oldCustomer with 
            Age = getRandomAge 18 45 }
    printfn "Old age: %d" oldCustomer.Age
    printfn "New age: %d" newCustomer.Age
    newCustomer

getNewCustomer customer
