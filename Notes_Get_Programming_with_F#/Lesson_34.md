# Lesson 34. Using type providers in the real world

### Supplying connection details to a type provider via config

`app.config`:

```text
<configuration>
    <connectionStrings>
        <add name="AdventureWorks"
             connectionString="Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"/>
    </connectionStrings>
</configuration>
```

Using with SqlClient:

```fsharp
// Supplying the connection string name (AdventureWorks) to the SQL Client type provide
type GetCustomers = SqlCommandProvider<"SELECT TOP 50 * FROM SalesLT.Customer", "name=AdventureWorks">

[<EntryPoint>]
let main _ = 
    // Removed Conn value frome Create() call.
    let customers = GetCustomers.Create();
    customers.Execute()
    |> Seq.iter (fun c -> printfn "%A: %s %s" c.CompanyName c.FirstName c.LastName)
    0
```

### Separating retrieval of live connection string from application code.

*Переопределение connection string в runtime.*

Единственный способ использовать connection string в скриптах
(у меня в скрипте не заработало).

1. Создание type provider - с использованием hardcoded строки соединения `[<Literal>]`.
Файл `CustomerRepository.fs`:

```fsharp
let [<Literal>] private CompileTimeConnection =
    "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
type private GetCustomers =
    SqlCommandProvider<"SELECT TOP 50 * FROM SalesLT.Customer", CompileTimeConnection>

let printCustomers(runtimeConnection:string) =
  let customers = GetCustomers.Create(runtimeConnection)
  customers.Execute()
  |> Seq.iter (fun c -> printfn "%A: %s %s" c.CompanyName c.FirstName c.LastName)
```

2. Использование type provider с переопределением строки соединения из файла `app.config`.
Файл `Program.fs`:

```fsharp
open System.Configuration

[<EntryPoint>]
let main argv =
    let runtimeConnectionString =
        ConfigurationManager
            .ConnectionStrings
            .["AdventureWorks"]     // Connection string name in the app.config file
            .ConnectionString
    CustomerRepository.printCustomers(runtimeConnectionString)  // Usage
    0
```

3. Скрипт `DataAccessThroughScript.fsx` (у меня не заработал):

```fsharp
#I "C:/Users/USER_NAME/.nuget/packages"
#r "fsharp.data.sqlclient/2.0.7/lib/netstandard2.0/FSharp.Data.SqlClient.dll"
#load "CustomerRepository.fs"

// Нужная нам строка соединения с БД
let scriptConnectionString =
    "Server=(localdb)\MSSQLLocalDb;Database=AdventureWorksLT;Integrated Security=SSPI"
// Запрос к БД
CustomerRepository.printCustomers(scriptConnectionString)
```

### Configuring type providers

|Compile time   | Runtime           | Effort    | Best for                                    |
|---------------|-------------------|-----------|---------------------------------------------|
|Literal values | Literal values    | Very easy | Simple systems, scripts, fixed data sources |
|app.config     | app.config        | Easy      | Simple redirection, improved security       |
|Literal values | Function argument | Medium    | Script drivers, large teams, full control   |
