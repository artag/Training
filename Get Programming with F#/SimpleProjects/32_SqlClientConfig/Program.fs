//open System.Configuration
open FSharp.Data

type GetCustomers = SqlCommandProvider<"SELECT TOP 50 * FROM SalesLT.Customer", "name=AdventureWorks">

[<EntryPoint>]
let main _ = 
    let customers = GetCustomers.Create();
    customers.Execute()
    |> Seq.iter (fun c -> printfn "%A: %s %s" c.CompanyName c.FirstName c.LastName)
    0
