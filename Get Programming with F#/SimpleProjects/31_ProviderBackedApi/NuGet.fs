module NuGet
open System

(*
    Private types.
*)
type private Package = FSharp.Data.HtmlProvider< @"../data/nuget-sample.html">

(*
    Public types.
*)
type PackageVersion =
    | Release
    | Preview
    | RC
    | Beta
    | Alpha

type VersionDetails = {
    Version : Version
    Downloads : decimal
    LastUpdated : DateTime
    PackageVersion : PackageVersion
}

type NuGetPackage = {
    PackageName : string
    Versions : VersionDetails list
}

(*
    Private. Parser'ы для строк.
*)
let private parseVersion (version:string) =
    let parts = version.Split '-'
    match parts with
    | [| number |] -> Version.Parse number, Release
    | [| number; symbols; _; _ |] when symbols.Contains("preview") -> Version.Parse number, Preview
    | [| number; symbols; _; _ |] when symbols.Contains("rc") -> Version.Parse number, RC
    | [| number; symbols; _; _ |] when symbols.Contains("beta") -> Version.Parse number, Beta
    | [| number; symbols; _; _ |] when symbols.Contains("alpha") -> Version.Parse number, Alpha
    | [| number; symbols |] when symbols.Contains("preview") -> Version.Parse number, Preview
    | [| number; symbols |] when symbols.Contains("rc") -> Version.Parse number, RC
    | [| number; symbols |] when symbols.Contains("beta") -> Version.Parse number, Beta
    | [| number; symbols |] when symbols.Contains("alpha") -> Version.Parse number, Alpha
    | _ -> failwith "Unknown version format"

let private parseDateTime (date:string) =
    let parse = Int32.Parse
    let parts = date.Split '/'
    let month = parts.[0] |> parse
    let day = parts.[1] |> parse
    let year = parts.[2] |> parse
    DateTime(year, month, day)

let parseRow (row:Package.VersionHistory.Row) =
    let (version, packageVersion) = parseVersion row.Version
    {
        Version = version
        Downloads = row.Downloads
        LastUpdated = parseDateTime row.``Last updated``
        PackageVersion = packageVersion
    }


(*
    Private. Получение и работа с данными.
*)
let private getPackage =
    sprintf "https://www.nuget.org/packages/%s" >> Package.Load

// HtmlProvider<...> -> HtmlProvider<...>.VersionHistory.Row[]
let private getVersionsForPackage (package:Package) =
    package.Tables.``Version History``.Rows

// HtmlProvider<...>.VersionHistory.Row[] -> seq<VersionDetails>
let private getVersions (rows:Package.VersionHistory.Row[]) =
    rows |> Seq.map parseRow

// string -> HtmlProvider<...>.VersionHistory.Row[] -> seq<VersionDetails>
let private loadPackageVersions = getPackage >> getVersionsForPackage >> getVersions

// string -> string -> VersionDetails option
let private getDetailsForVersion (version:string) =
    loadPackageVersions >> Seq.tryFind(fun p -> p.Version.ToString().Contains version)

(*
    Public.
*)
/// Получает список всех версий, доступных для nuget-пакета.
let getPackageVersions name =
    let versionDetails = name |> loadPackageVersions |> Seq.toList
    {
        PackageName = name
        Versions = versionDetails
    }

/// Пытается получить информацию о версии для nuget-пакета.
let tryGetPackageVersion name version =
    name |> getDetailsForVersion version