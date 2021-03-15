open FSharp.Data.Sql

let [<Literal>] Conn =
    "Server=(localdb)\MSSQLLocalDB;Database=AdventureWorksLT;Integrated Security=SSPI"

type AdventureWorks = SqlDataProvider<ConnectionString = Conn, UseOptionTypes = true>

let printCustomer (firstName, lastName) =
    printfn " %s %s" firstName lastName

[<EntryPoint>]
let main argv =
    // Set db-context
    let context = AdventureWorks.GetDataContext()

    (*
        Querying data by using the SQLProvider library
    *)
    let customers =
        query {
            for customer in context.SalesLt.Customer do
            take 10
        } |> Seq.toArray

    // Print first customer
    let customer = customers.[0]
    printfn "First customer:"
    printCustomer (customer.FirstName, customer.LastName)
    printfn ""

    (*
        More complex query
    *)
    let customersFromFriendlyBikeShop =
        query {
            for customer in context.SalesLt.Customer do
            where (customer.CompanyName = Some "Friendly Bike Shop")
            select (customer.FirstName, customer.LastName)
            distinct
        }

    // Print selected customers
    printfn "Customers from \"Friendly Bike Shop\":"
    customersFromFriendlyBikeShop
    |> Seq.toArray
    |> Array.iter(fun c -> printCustomer c)
    printfn ""

    (*
         Inserting data
    *)
    // Check if record already exists in the table "Product Category"
    let finded =
        query {
            for pc in context.SalesLt.ProductCategory do
            where (pc.Name = "Scarf")
        } |> Seq.toArray

    if finded.Length <= 0 then
        printfn "Inserting data into the table \"Product Category\"..."
        let category = context.SalesLt.ProductCategory.Create()
        category.ParentProductCategoryId <- Some 3
        category.Name <- "Scarf"
        context.SubmitUpdates()

    (*
        Working with reference data
    *)
    let mittens =
        context.SalesLt.ProductCategory
            .Individuals.``As Name``.``42, Mittens``

    // Print selected row
    printfn "%s %O" mittens.Name mittens.ModifiedDate

    0
