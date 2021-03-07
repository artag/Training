#I @"..\.paket\load\netstandard2.0"
#load "FSharp.Data.fsx"
#load "XPlot.GoogleCharts.fsx"

open FSharp.Data
open XPlot.GoogleCharts

type Films = FSharp.Data.HtmlProvider<"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">
let deNiro = Films.GetSample()
deNiro.Tables.FilmsEdit.Rows
|> Array.countBy(fun row -> string row.Year)
|> Chart.SteppedArea
|> Chart.Show
