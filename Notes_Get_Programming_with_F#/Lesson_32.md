# Lesson 32. Working with SQL

### SqlClient

SqlClient - data access layer (Micro ORM) designed specifically for MSSQL.
NuGet package `FSharp.Data.Sql`.

#### Querying data with the `SqlCommandProvider`

* `[<Literal>]` attribute mark connection string value as a *compile-time constant*,
which is needed when passing values as arguments to type providers.

* Название `Conn` с большой буквы, т.к. это compile-time constant.

* В качестве SQL запросов можно использовать помимо SELECT запросов более сложные:
  * Joins
  * Common table expressions
  * Stored procedures—even table valued functions.

* Но SqlClient поддерживает не все команды SQL (подробности см. в официальной документации).

```fsharp
// A standard SQL connection string
let [<Literal>] Conn =
    "Server=(localdb)\MSSQLLocalDb;Database= AdventureWorksLT;Integrated Security=SSPI"
// Creating a strongly typed SQL command
type GetCustomers =
    SqlCommandProvider<"SELECT * FROM SalesLT.Customer", Conn>
// Executing the command to return a dataset
let customers =
    GetCustomers.Create(Conn).Execute() |> Seq.toArray
// Get record from dataset
let customer = customers.[0]
```

Запрос с параметром:

```fsharp
type GetProductCategory =
    SqlCommandProvider<"SELECT * FROM SalesLT.ProductCategory WHERE Name = @Name", Conn>
let findProductCatergory productName =
    GetProductCategory.Create(Conn).Execute(productName) |> Seq.toArray
```

#### Inserting data. `SqlProgrammabilityProvider`

* `Update()` - create a DataAdapter and the appropriate insert command.

* `BulkInsert()` - insert data by using SQL Bulk Copy functionality.
Extremely efficient and great for large one-off inserts of data.

* You can also use the data table for updates and deletes, or via T-SQL commands.

```fsharp
type AdventureWorks = SqlProgrammabilityProvider<Conn>
type ProductCategory = AdventureWorks.SalesLT.Tables
// Get table from db
let productCategory = new AdventureWorks.SalesLT.Tables.ProductCategory()
// Inserting data into the table
productCategory.AddRow("Mittens", Some 3, Some (Guid.NewGuid()), Some DateTime.Now)
productCategory.AddRow("Long Short", Some 3, Some (Guid.NewGuid()), Some DateTime.Now)
productCategory.AddRow("Wooly Hats", Some 4, Some (Guid.NewGuid()), Some DateTime.Now)
// Create a DataAdapter and insert data
productCategory.Update()
```

#### Working with reference data. `SqlEnumProvider`

Reference data - static (or relatively stable) sets of lookup data: categories, country lists,
regions that need to be referenced both in code and data.

You’ll normally have a C# enum and/or class with constant values
that matches a set of items scripted into a database.

```fsharp
// Generating a Categories type for all product categories
type Categories =
    SqlEnumProvider<"SELECT Name, ProductCategoryId FROM SalesLT.ProductCategory", Conn>
// Accessing the Wooly Hats integer ID
let woolyHats = Categories.``Wooly Hats``
printfn "Wooly Hats has ID %d" woolyHats
```

### SQLProvider

SQLProvider - ORM. Can work with many ODBC data sources (MSSQL, Oracle, SQLite, Postgres, MySQL, ...).
NuGet package `SQLProvider` (плюс, мне потребовалась ссылка на nuget `System.Data.SqlClient`).

#### Querying data

* Query expressions can be used in F# over any `IQueryable` data source,
so you can use them anywhere you'd write a LINQ query in C#.

* `query { }` expressions are another form of computation expression,
similar to `seq { }` and `async { }`

```fsharp
open FSharp.Data.Sql

let [<Literal>] Conn =
    "Server=(localdb)\MSSQLLocalDB;Database=AdventureWorksLT;Integrated Security=SSPI"

// Creating an AdventureWorks type by using the SqlDataProvider
// UseOptionTypes=true - generate option types for nullable columns
// UseOptionTypes=false - default value will be generated for nullable columns
type AdventureWorks = SqlDataProvider<ConnectionString = Conn, UseOptionTypes = true>
// Getting a handle to a sessionized data context
let context = AdventureWorks.GetDataContext()
// Writing a query against the Customer table (get the first 10 customers)
let customers =
    query {
        for customer in context.SalesLt.Customer do
        take 10
    } |> Seq.toArray

// Get customer
let customer = customers.[0]
```

More complex query:

```fsharp
// More complex query
let customersFromFriendlyBikeShop =
    query {
        for customer in context.SalesLt.Customer do
        where (customer.CompanyName = Some "Friendly Bike Shop")   // A filter condition
        select (customer.FirstName, customer.LastName)             // Map to tuples as result
        distinct    // Selecting a distinct list of results
    }
```

#### Inserting data

Adding data to the database is simple:

1. Create new entities through the data context (`Create()`)
2. Set properties (with `<-`)
3. Save changes with (`SubmitUpdates()`)

```fsharp
// Creating a new entity attached to the ProductCategory table
let category = context.SalesLt.ProductCategory.Create()
// Mutating properties on the entity
category.ParentProductCategoryId <- Some 3
category.Name <- "Scarf"
// Calling SubmitUpdates to save the new data
context.SubmitUpdates()
```

Примечания:

* All entities track their own states and have a `_State` property on them.
* On create a new entity, you'll see that its initial state is `Created`.
* After calling `SubmitUpdates()`, its state changes to `Unchanged`.
* Updates are performed by first loading the data from the database, mutating
the records, and then calling `SubmitChanges()`.

#### Working with reference data

* Every table on the context has a property `Individuals`, which
will generate a list of properties that match the rows in the database - essentially the same
as the Enum Provider.

* You also have subproperties underneath that allow you to choose
which column acts as the "text" property (for example, `As Name` or `As ModifiedDate`).

Example:

```fsharp
let mittens =
        context.SalesLt.ProductCategory     // Table
            .Individuals
            .``As Name``                    // Text
            .``42, Mittens``                // Selected row

// Using
printfn "%s %O" mittens.Name mittens.ModifiedDate   // Mittens 11.03.2021 23:14:36
```
