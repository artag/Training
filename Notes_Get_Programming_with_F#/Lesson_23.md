# Lesson 23. Business rules as code

## Specific types in F#

A sample F# record:

```fsharp
type Customer =
    { CustomerId : string
      Email : string
      Telephone : string
      Address : string }
```

Проблема: при создании customer можно легко перепутать данные (например, поставить вместо email
параметр для telephone). Решение этой и подобных проблем - использование single-case DUs.

### Single-case discriminated unions (DU)

```fsharp
// Creating a single-case DU to store a string Address
type Address = Address of string

// Creating an instance of a wrapped Address
let myAddress = Address "1 The Street"

// Comparing a wrapped Address and a raw string won’t compile
let isTheSameAddress = (myAddress = "1 The Street")

// Unwrapping an Address into its raw string as addressData
let (Address addressData) = myAddress
```

Тип Customer можно записать так:

```fsharp
// Creating a number of single-case DUs
type CustomerId = CustomerId of string
type Email = Email of string
type Telephone = Telephone of string
type Address = Address of string

// Using single-case DUs in the Customer type
type Customer =
    { CustomerId : CustomerId
      Email : Email
      Telephone : Telephone
      Address : Address }
```

Рекомендуется сразу "оборачивать" в single-case DU данные, которые приходят извне. И сразу
проверять их на валидность. А внутри доменной модели просто их "распаковывать" при выполнении
операций над ними.

### Combining single-case discriminated unions

```fsharp
// Only one of the contact details should be allowed at any point in time.
type ContactDetails =
| Address of string
| Telephone of string
| Email of string

let customer = createCustomer (CustomerId "Nicki") (Email "nicki@myemail.com")
```

### Using optional values within a domain

Adding an option field for optional secondary contact details:

```fsharp
type Customer =
    { CustomerId : CustomerId
      PrimaryContactDetails : ContactDetails
      SecondaryContactDetails : ContactDetails option }
```

## Encoding business rules with marker types

### Creating custom types to represent business states

Create a single-case DU that acts as a marker type: it wraps around a standard `Customer`, and
allows you to treat it differently.

```fsharp
// Single-case DU to wrap around Customer
type GenuineCustomer = GenuineCustomer of Customer

// Custom logic to validate a customer
let validateCustomer customer =
    match customer.PrimaryContactDetails with
    | Email e when e.EndsWith "SuperCorp.com" -> Some(GenuineCustomer customer)
    | Address _ | Telephone _ -> Some(GenuineCustomer customer)
    | Email _ -> None

// The sendWelcomeEmail accepts only a GenuineCustomer as input
let sendWelcomeEmail (GenuineCustomer customer) =
    printfn "Hello, %A, and welcome to our site!" customer.CustomerId

// Usage
unknownCustomer                             // Customer
|> validateCustomer                         // Customer -> GenuineCustomer option
|> Option.map(fun c -> sendWelcomeEmail c)  // unit option
```

### When and when not to use marker types

You can use them for all sorts of things (examples):

* email (verified, unverified)
* order (unpaid, paid, dispatched, or fulfilled)
* data (checked, unchecked)

But be careful not to take it too far, as it can become difficult to wade through a sea of
types if overdone.

## Results vs. exceptions

В F#, можно использовать исключения как в C#, используя `try .. with` синтакс.
Но в F# обычно вместо кидания исключений используется `result`.

### Using `Result` (instead of using exceptions)

F# 4.1 contains a `Result` type built into the standard library.

Creating a result type to encode success or failure:

```fsharp
// Defining a simple Result discriminated union
type Result<'a> =
| Success of 'a
| Failure of string
```

```fsharp
// Type signature of a function that might fail
insertCustomer : contactDetails:ContactDetails -> Result<CustomerId>

// Handling both success and failure cases up front
match insertContact (Email "nicki@myemail.com") with
| Success customerId -> printfn "Saved with %A" customerId
| Failure error -> printfn "Unable to save: %s" error
```

Internally in `insertCustomer`, you'd execute the code in a `try/catch` statement;
any caught errors would be returned as a failure.

### When to use `Result` and exceptions

* If an error occurs and is something that you *don't* want to reason
about (не хотите/можете обработать) (for example, a catastrophic error that leads (ведет) to an
end of the application), stick to **exceptions**.

* If it's something that you *do* want to reason about (for example, depending
on success or failure, you want to do some custom logic and then resume processing
in the application), a `Result` type is a useful tool to have.
