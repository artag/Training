namespace Capstone2

type Customer = { Name : string }

type Account =
    { Id : System.Guid
      Owner : Customer
      Balance : decimal }
