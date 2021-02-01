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
