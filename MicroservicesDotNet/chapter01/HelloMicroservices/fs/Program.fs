open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    // Adds MVC controller and helper services to the service collection
    builder.Services.AddControllers() |> ignore
    let app = builder.Build()

    //app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore

    // Redirects all HTTP requests to HTTPS
    app.UseHttpsRedirection() |> ignore
    app.UseRouting() |> ignore
    // Adds all endpoints in all controllers to MVC’s route table
    app.UseEndpoints(fun (endpoints) -> endpoints.MapControllers() |> ignore) |> ignore 

    app.Run()

    0 // Exit code

