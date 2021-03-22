open System.Configuration

[<EntryPoint>]
let main argv =
    let runtimeConnectionString =
        ConfigurationManager
            .ConnectionStrings
            .["AdventureWorks"]
            .ConnectionString
    CustomerRepository.printCustomers(runtimeConnectionString)
    0
