module Project

open FSharp.Data
open XPlot.GoogleCharts

type Football = CsvProvider< @"E:\Temp\FootballResults.csv" >

let showChart() =
    let data = Football.GetSample().Rows |> Seq.toArray
    data
    |> Seq.filter(fun row ->
        row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
    |> Seq.countBy(fun row -> row.``Home Team``)
    |> Seq.sortByDescending snd
    |> Seq.take 10
    |> Chart.Column
    |> Chart.Show
