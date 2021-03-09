open FSharp.Data
open XPlot.GoogleCharts

type Package = HtmlProvider< @"..\data\sample-package.html">

[<EntryPoint>]
let main argv =
    let nunit = Package.Load "https://www.nuget.org/packages/nunit"
    let entityFramework = Package.Load "https://www.nuget.org/packages/entityframework"
    let newtonsoftJson = Package.Load "https://www.nuget.org/packages/newtonsoft.json"

    [entityFramework; nunit; newtonsoftJson]
    |> Seq.collect(fun package -> package.Tables.``Version History``.Rows)
    |> Seq.sortByDescending(fun package -> package.Downloads)
    |> Seq.take 10
    |> Seq.map(fun package -> package.Version, package.Downloads)
    |> Chart.Column
    |> Chart.Show
    0
