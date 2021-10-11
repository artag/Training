# Lesson 31. Building schemas from live data

### Data Providers. Opening a remote JSON data source with `JsonProvider`

```fsharp
// Referencing FSharp.Data
#r @"..\..\packages\FSharp.Data\lib\net40\FSharp.Data.dll"
open FSharp.Data
// Creating the TVListing type based on a URL
type TvListing =
  JsonProvider<"http://www.bbc.co.uk/programmes/genres/comedy/schedules/upcoming.json">
// Creating an instance of the type provider
let tvListing = TvListing.GetSample()
let title = tvListing.Broadcasts.[0].Programme.DisplayTitles.Title
```

### Data Providers. Opening HTML data source with `HtmlProvider`

Show the number of films acted in over time by Robert DeNiro from Wikipedia.

```fsharp
open FSharp.Data
open XPlot.GoogleCharts

type Films = FSharp.Data.HtmlProvider<"https://en.wikipedia.org/wiki/Robert_De_Niro_filmography">
let deNiro = Films.GetSample()
deNiro.Tables.FilmsEdit.Rows
|> Array.countBy(fun row -> string row.Year)
|> Chart.SteppedArea
|> Chart.Show
```

### Examples of live schema type providers

* *JSON type provider* - Provides a typed schema from JSON data sources
* *HTML type provider* - Provides a typed schema from HTML documents
* *Swagger type provider* - Provides a generated client for a Swagger-enabled HTTP
endpoint, using Swagger metadata to create a strongly typed model
* *Azure Storage type provider* - Provides a client for blob/queue/table storage assets
* *WSDL type provider* - Provides a client for SOAP-based web services

###  Avoiding problems with live schemas

* Large data sources

  Объем данных может быть слишком большим (500 MB и более).

* Inferred schemas

  (CSV файл с нужным полем, которое заполнено только в 9 999 строке).

* Priced schemas

  Некоторые ресурсы взымают деньги за доступ к данным.

* Connectivity

  Требуется постоянное соединение с ресурсом для генерации типов.

### Redirecting type providers to new data. Использование `Load` в type provider'ах.

Идея: используется локальный файл данных (обычно хранится в системе управления версиями)
как часть исходного кода во время компиляции. Этот файл данных представляет схему
и используется type provider'ом для генерации типов.

Во время runtime можно переключиться на реальный источник данных.

Пример. Подсчет количества скачиваний для трех nuget-пакетов:

```fsharp
// Using local file to create scheme for type provider
type Package = HtmlProvider< @"..\data\sample-package.html">

// Load in data from a live URI
let nunit = Package.Load "https://www.nuget.org/packages/nunit"
let entityFramework = Package.Load "https://www.nuget.org/packages/entityframework"
let newtonsoftJson = Package.Load "https://www.nuget.org/packages/newtonsoft.json"

// Creating a list of package statistics values
[entityFramework; nunit; newtonsoftJson]
// Merging all rows from each package into a single sequence
|> Seq.collect(fun package -> package.Tables.``Version History``.Rows)
|> Seq.sortByDescending(fun package -> package.Downloads)
//...
```
