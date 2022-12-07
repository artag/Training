namespace HelloMicroservices

open System
open Microsoft.AspNetCore.Mvc

type CurrentDateTimeController() =
    inherit ControllerBase()

    [<HttpGet("/")>]
    member _.Get() =
        DateTime.UtcNow
