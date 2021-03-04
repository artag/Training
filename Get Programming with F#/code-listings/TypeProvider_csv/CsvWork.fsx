#I @"..\.paket\load\netstandard2.0"
#load "FSharp.Data.fsx"
#load "XPlot.GoogleCharts.fsx"

open FSharp.Data
open XPlot.GoogleCharts

type Football = FSharp.Data.CsvProvider< @"E:\Temp\FootballResults.csv">
let data = Football.GetSample().Rows |> Seq.toArray
data.[0].``Full Time Home Goals``.ToString()
data
|> Seq.filter(fun row ->
    row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
|> Seq.countBy(fun row -> row.``Home Team``)
|> Seq.sortByDescending snd
|> Seq.take 10
|> Chart.Column
|> Chart.Show
