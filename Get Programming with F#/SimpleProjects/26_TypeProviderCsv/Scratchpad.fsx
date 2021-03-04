//#I @"C:\Users\USER_NAME\.nuget\packages"
//#r @"fsharp.data\4.0.1\lib\netstandard2.0\FSharp.Data.dll"
//#r @"xplot.googlecharts\3.0.1\lib\netstandard2.0\XPlot.GoogleCharts.dll"
//#r @"google.datatable.net.wrapper\4.0.0\lib\netstandard2.0\Google.DataTable.Net.Wrapper.dll"

//open FSharp.Data
//open XPlot.GoogleCharts

//type Football = FSharp.Data.CsvProvider< @"E:\Temp\FootballResults.csv" >
//let data = Football.GetSample().Rows |> Seq.toArray
//data
//|> Seq.filter(fun row ->
//    row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
//|> Seq.countBy(fun row -> row.``Home Team``)
//|> Seq.sortByDescending snd
//|> Seq.take 10
//|> Chart.Column
//|> Chart.Show
