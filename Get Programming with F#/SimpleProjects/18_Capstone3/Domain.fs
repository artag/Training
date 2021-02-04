namespace Capstone3

open System

type Customer = {
    Name : string
}

type Account = {
    Id : Guid
    Owner : Customer
    Balance : decimal
}

type Transaction = {
    Timestamp : DateTime
    Operation : string
    Amount : decimal
    Accepted : bool
}
