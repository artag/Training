open FSharp.Data
open XPlot.GoogleCharts

type Films = HtmlProvider<"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">

[<EntryPoint>]
let main argv =
    let deNiro = Films.GetSample()
    deNiro.Tables.FilmsEdit.Rows
    |> Array.countBy(fun row -> string row.Year)
    |> Chart.SteppedArea
    |> Chart.Show
    0
