open FSharp.Data
open System

let [<Literal>] Conn =
    "Server=(localdb)\MSSQLLocalDB;Database=AdventureWorksLT;Integrated Security=SSPI"

/// Query data
module QueryCustomers =
    type GetCustomers =
        SqlCommandProvider<"SELECT * FROM SalesLT.Customer", Conn>

    let execute() =
        let customers = GetCustomers.Create(Conn).Execute() |> Seq.toArray
        let customer = customers.[0]
        let firstName = customer.FirstName
        let lastName = customer.LastName
        let companyName = customer.CompanyName |> defaultArg <| "Unknown company"
        printfn "%s %s works for %s" firstName lastName companyName

module FindProductCategory =
    type GetProductCategory =
        SqlCommandProvider<"SELECT * FROM SalesLT.ProductCategory WHERE Name = @Name", Conn>

    let execute productName =
        let finded = GetProductCategory.Create(Conn).Execute(productName) |> Seq.toArray
        match finded.Length with
        | 0 -> None
        | _ -> Some (finded |> Array.head)

/// Inserting data
module InsertProductCategory =
    type AdventureWorks = SqlProgrammabilityProvider<Conn>
    type ProductCategory = AdventureWorks.SalesLT.Tables

    let execute name parentProductCategoryId =
        let productCategory = new AdventureWorks.SalesLT.Tables.ProductCategory()
        productCategory.AddRow(name, Some parentProductCategoryId, Some (Guid.NewGuid()), Some DateTime.Now)
        productCategory.Update()

/// Working with reference data
module ReferenceFromSqlEnum =
    type Categories = SqlEnumProvider<"SELECT Name, ProductCategoryId FROM SalesLT.ProductCategory", Conn>

    let execute() =
        let woolyHats = Categories.``Wooly Hats``
        printfn "Wooly Hats has ID %d" woolyHats

module Helper =
    let insertProductIfNone name parentProductCategoryId =
        let finded = FindProductCategory.execute name
        match finded with
        | Some _ -> 0
        | None -> InsertProductCategory.execute name parentProductCategoryId

[<EntryPoint>]
let main argv =
    // Querying data with the SqlCommandProvider
    QueryCustomers.execute()

    // Inserting data
    Helper.insertProductIfNone "Mittens" 3 |> ignore
    Helper.insertProductIfNone "Long Shorts" 3 |> ignore
    Helper.insertProductIfNone "Wooly Hats" 4 |> ignore

    // Working with reference data
    ReferenceFromSqlEnum.execute()

    0
