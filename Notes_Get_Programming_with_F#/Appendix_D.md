# Appendix D. Must-have F# libraries

## Build and DevOps

* http://fsharp.github.io/FAKE
**FAKE** (F# Make) is a build automation system with capabilities that are similar
to make and rake.

* http://fsprojects.github.io/ProjectScaffold/
**ProjectScaffold** helps you get started with a new .NET/Mono project solution with everything
needed for successful organizing of code, tools, and publishing.

## Data

* http://fsprojects.github.io/ExcelProvider/
**ExcelProvider**. Type provider to work seamlessly with CSV, JSON, or XML files.

* http://bluemountaincapital.github.io/Deedle/
**Deedle** is an easy-to-use library for data and time-series manipulation and for scientific
programming. (Equivalent of R's DataFrames, or Python's Pandas).

* https://fslab.org/
**FsLab** is a collection of libraries for data science. It provides a rapid development
environment that lets you write advanced analysis with a few lines of production-quality code.

* https://fslab.org/FSharp.Charting/
**FSharp.Charting** uses the charting components built into .NET to create charts.

## Web

* https://fable.io/
**Fable** is a compiler F# into JavaScript.

* https://websharper.com/
**WebSharper**. Develop microservices, client-server web applications, reactive SPAs, and more
in C# or F#.

* https://freya.io/, https://github.com/xyncro/freya
**Freya** - F#-first web programming framework, just like Suave.

* http://fsprojects.github.io/FSharp.Formatting/
**F# Formatting** - generate HTML documentation based on F# scripts or a combination of Markdown
files with embedded F#.

## Cloud

* https://github.com/fsprojects/FSharp.Azure.Storage
**FSharp.Azure.Storage** provides a pleasant F# DSL on top of the table service, with support
for easy insertion, updates, and queries of data directly from F# records.

* http://fsprojects.github.io/AzureStorageTypeProvider/
The **Azure Storage Type Provider** gives you a full type provider over the three main
Azure Storage services: Blobs, Tables, and Queues.

* https://github.com/fsprojects/FSharp.AWS.DynamoDB
**FSharp.AWS.DynamoDB** an F# wrapper over the standard Amazon.DynamoDB library.

* http://mbrace.io/
**MBrace.Core** is a simple programming model for scalable cloud data scripting and programming with F# and C#. With MBrace.Azure, you can script Azure for large-scale compute and data processing, directly from your favourite editor.

## Desktop

* http://fsprojects.github.io/FsXaml/, https://github.com/fsprojects/FsXaml
**FsXaml** - F# Tools for working with XAML Projects. Library that removes the need for the
code-behind code generation through a type provider.

* https://github.com/fsprojects/FSharp.ViewModule
**FSharp.ViewModule**. Library providing MVVM and `INotifyPropertyChanged` support for
F# projects.

## Miscellaneous

* http://fsprojects.github.io/Argu/
**Argu** -  parse configuration arguments for a console application.

* http://fsprojects.github.io/FSharp.Management/
The **FSharp.Management** project contains various type providers for the management
of the machine: File System, Registry, Windows Management Instrumentation (WMI),
PowerShell, SystemTimeZonesProvider.

* http://fsprojects.github.io/FsReveal/
**FsReveal** allows you to write beautiful slides in Markdown and brings C# and F# to the
`reveal.js` web presentation framework.

* http://fsprojects.github.io/FSharp.Configuration/
**FSharp.Configuration** is a set of easy-to-use type providers that support the reading of
various configuration file formats: AppSettings, ResX, Yaml, INI.

* http://fsprojects.github.io/Chessie/
**Chessie**. Brings railway-oriented programming to .NET.

## The F# toolchain. Comparing alternative technology stacks on .NET and F#

| Function               | Microsoft stack              | Pure F# stack
|------------------------|------------------------------|-------------------------------------
| Complex build process  | MS Build custom tasks        | FAKE script with MSBuild
| Continuous integration | TeamCity, TFS pipeline, etc. | FAKE script on TeamCity, TFS, etc.
| Dependency management  | NuGet                        | Paket with NuGet + GitHub dependencies
| Project system         | Solution and projects        | Standalone scripts and/or project + solution
| Ad hoc processing      | Console applications         | Standalone scripts
| Test libraries         | xUnit, NUnit                 | Expecto, QuickCheck, Unquote, FsTest
| SQL ORM                | Entity Framework             | SQLProvider
| SQL micro-ORM          | Dapper                       | FSharp.Data SQLClient
| Server-side web        | Full-blown Web API project   | Bare-bones NET Web API OWIN, or Suave
| Front-end web          | ASP .NET MVC, TypeScript     | F# with Fable
| IDE                    | Visual Studio                | VSCode, Emacs, Visual Studio, and so on
