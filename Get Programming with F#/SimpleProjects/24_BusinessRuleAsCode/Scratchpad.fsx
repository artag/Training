//type CustomerId = CustomerId of string
//type Email = Email of string
//type Telephone = Telephone of string
//type Address = Address of string

//type Customer = {
//    CustomerId : CustomerId
//    Email : Email
//    Telephone : Telephone
//    Address : Address
//}

//let createCustomer customerId email telephone address =
//    { CustomerId = customerId
//      Email = email
//      Telephone = telephone
//      Address = address }

//createCustomer
//    (CustomerId "C-123")
//    (Email "nicki@myemail.com")
//    (Telephone "029-293-23")
//    (Address "1 The Street")

type CustomerId = CustomerId of string

type ContactDetails =
| Address of string
| Telephone of string
| Email of string

type Customer = {
    CustomerId : CustomerId
    PrimaryContactDetails : ContactDetails
    SecondaryContactDetails : ContactDetails option }

type GenuineCustomer = GenuineCustomer of Customer

let createCustomer customerId contactDetails optionalContactDetails =
    { CustomerId = customerId
      PrimaryContactDetails = contactDetails
      SecondaryContactDetails = optionalContactDetails }

let unknownCustomer = createCustomer (CustomerId "Nicki") (Email "nicki@unknown.com") (Some (Telephone "12345"))
let knownCustomer = createCustomer (CustomerId "Bill") (Email "bill@SuperCorp.com") None

let validate customer =
    match customer.PrimaryContactDetails with
    | Email e when e.EndsWith "SuperCorp.com" -> Some(GenuineCustomer customer)
    | Address _ | Telephone _ -> Some(GenuineCustomer customer)
    | Email _ -> None

let sendWelcomeEmail (GenuineCustomer customer) =
    printfn "Hello, %A, and welcome to our site!" customer.CustomerId 

unknownCustomer
|> validate
|> Option.map(fun c -> sendWelcomeEmail c)

knownCustomer
|> validate
|> Option.map(fun c -> sendWelcomeEmail c)
