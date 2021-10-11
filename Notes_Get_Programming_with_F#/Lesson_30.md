# Lesson 30. Introducing type providers

### Working with CSV files using FSharp.Data

#### Working with CSV files from scripts using Paket and `CsvProvider`

1. Добавить Paket в переменную окружения PATH (требуется единожды)

2. Запустить build.cmd (требуется единожды, при создании новой директории со скриптом)

3. Можно работать со скриптом. Для загрузки `*.csv` используется `CsvProvider`,
из `FSharp.Data` namespace.

```text
paket.bootstrapper                      // Скачивает (если требуется) новую версию paket
paket init                              // Инициализация (если требуется) paket
paket add FSharp.Data                   // Добавление nuget-пакетов в paket
paket add XPlot.GoogleCharts
paket add Google.DataTable.Net.Wrapper  // (?) Возможно, добавится при добавлении XPlot.GoogleCharts
paket generate-load-scripts             // Формирует файлы *.fsx для загрузки nuget-пакетов.
```

```fsharp
#I @"..\.paket\load\netstandard2.0"
// Referencing the FSharp.Data assembly
#load "FSharp.Data.fsx"
#load "XPlot.GoogleCharts.fsx"

open FSharp.Data
open XPlot.GoogleCharts

// Для CsvProvider в скриптах надо указывать префикс FSharp.Data,
// иначе пишет "CsvProvider not found" (? - глюк Intellisence)
// Connecting to the CSV file to provide types based on the supplied file
type Football = FSharp.Data.CsvProvider< @"E:\Temp\FootballResults.csv">
// Loading in all data from the supplied CSV file
let data = Football.GetSample().Rows |> Seq.toArray

// Select row
data.[0].``Full Time Home Goals``.ToString()

// Select data with visualizing data
data
|> Seq.filter(fun row ->
    row.``Full Time Home Goals`` > row.``Full Time Away Goals``)
// countBy generates a sequence of tuples (team vs. number of wins).
|> Seq.countBy(fun row -> row.``Home Team``)
|> Seq.sortByDescending snd
|> Seq.take 10
// Converting the sequence of tuples into an XPlot Column Chart
|> Chart.Column
// Showing the chart in a browser window
|> Chart.Show
```

### Backtick members ``

Установка Double backtick (знак `` ) в начале и конце member definition позволяет указывать
в имени member буквы, пробелы, цифры и прочие символы (см. предыдущий пример).
