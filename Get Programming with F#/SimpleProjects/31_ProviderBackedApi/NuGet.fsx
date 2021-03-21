#I @"C:\Users\temy4\.nuget\packages\"
#r @"fsharp.data\4.0.1\lib\netstandard2.0\FSharp.Data.dll"

open System
open FSharp.Data

type Package = FSharp.Data.HtmlProvider< @"../data/nuget-sample.html">
//type Package = FSharp.Data.HtmlProvider< @"../data/sample-package.html">

(*
// string -> HtmlProvider<...>
let packageLoad packageName =
    packageName |> sprintf "https://www.nuget.org/packages/%s" |> Package.Load

// string -> decimal
let getDownloadsForPackage packageName =
    let package = packageLoad packageName
    package.Tables.``Version History``.Rows |> Seq.sumBy(fun p -> p.Downloads)

// string -> string -> HtmlProvider<...>.VersionHistory.Row option
let getDetailsForVersion versionText packageName  =
    let package = packageLoad packageName
    package.Tables.``Version History``.Rows |> Seq.tryFind(fun p -> p.Version.Contains versionText)
*)


(*
    Refactoring
*)

(*
// string -> HtmlProvider<...>
let getPackage =
    sprintf "https://www.nuget.org/packages/%s" >> Package.Load

// HtmlProvider<...> -> HtmlProvider<...>.VersionHistory.Row[]
let getVersionsForPackage (package:Package) =
    package.Tables.``Version History``.Rows

// string -> HtmlProvider<...>.VersionHistory.Row[]
let loadPackageVersions = getPackage >> getVersionsForPackage

// string -> decimal
let getDownloadsForPackage =
    loadPackageVersions >> Seq.sumBy(fun p -> p.Downloads)

// string -> string -> HtmlProvider<...>.VersionHistory.Row option
let getDetailsForVersion versionText =
    loadPackageVersions >> Seq.tryFind(fun p -> p.Version.Contains versionText)

// Не работает
// string -> HtmlProvider<...>.VersionHistory.Row option
let getDetailsForCurrentVersion = getDetailsForVersion "(this version)"
*)

(*
    Refactoring #2. F# Domain
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

(* Вспомогательные функции parse *)

let parseVersion (version:string) =
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

let parseDateTime (date:string) =
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

let tryParseRow (row:Package.VersionHistory.Row option) =
    match row with
    | None -> None
    | Some row -> parseRow row |> Some

// string -> HtmlProvider<...>
let getPackage =
    sprintf "https://www.nuget.org/packages/%s" >> Package.Load

// HtmlProvider<...> -> HtmlProvider<...>.VersionHistory.Row[]
let getVersionsForPackage (package:Package) =
    package.Tables.``Version History``.Rows

// HtmlProvider<...>.VersionHistory.Row[] -> seq<VersionDetails>
let getVersions (rows:Package.VersionHistory.Row[]) =
    rows |> Seq.map parseRow

// string -> HtmlProvider<...>.VersionHistory.Row[] -> seq<VersionDetails>
let loadPackageVersions = getPackage >> getVersionsForPackage >> getVersions

// string -> string -> VersionDetails option
let getDetailsForVersion version =
    loadPackageVersions >> Seq.tryFind(fun p -> p.Version.ToString().Contains version)

// string -> NugGetPackage
let getNuGetPackage name =
    let versionDetails = name |> loadPackageVersions |> Seq.toList
    {
        PackageName = name
        Versions = versionDetails
    }

// string -> VersionDetail option
let tryGetNuGetPackageVersion name version =
    name |> getDetailsForVersion version

let printNuGetPackage package =
    printfn "NuGet package: %s" package.PackageName
    printfn "Versions:"
    package.Versions
    |> Seq.iter(fun p -> printfn "\t%s: %A" (p.Version.ToString(3)) p.PackageVersion)

let printVersionDetails (versionDetails:VersionDetails) =
    printfn "Version: %s" (versionDetails.Version.ToString(3))
    printfn "Downloads: %M" versionDetails.Downloads
    printfn "LastUpdated: %s" (versionDetails.LastUpdated.ToString("g"))
    printfn "PackageVersion: %A" versionDetails.PackageVersion

let tryPrintVersionDetails (versionDetails:VersionDetails option) =
    match versionDetails with
    | Some details -> printVersionDetails details
    | None -> printfn "Version: not found"

(*
    Testing
*)
getNuGetPackage "Newtonsoft.Json" |> printNuGetPackage
tryGetNuGetPackageVersion "Newtonsoft.Json" "9.0.1" |> tryPrintVersionDetails

let row1 = "EntityFramework" |> getDetailsForVersion "6.4.4"
let row2 = "EntityFramework" |> getDetailsForVersion "6.4.0-preview1-19506-01"
let row3 = "EntityFramework" |> getDetailsForVersion "6.3.0-rc1-19458-04"
let row4 = "EntityFramework" |> getDetailsForVersion "6.0.0-rc1"
let row5 = "EntityFramework" |> getDetailsForVersion "6.1.2-beta1"
let row6 = "EntityFramework" |> getDetailsForVersion "6.1.2-beta2"
let row7 = "EntityFramework" |> getDetailsForVersion "6.1.0-alpha1"
let row8 = "EntityFramework" |> getDetailsForVersion "6.0.0-alpha3"

let version1 = "6.4.4"
let version2 = "6.4.0-preview1-19506-01"
let version3 = "6.3.0-rc1-19458-04"
let version4 = "6.0.0-rc1"
let version5 = "6.1.2-beta1"
let version6 = "6.1.2-beta2"
let version7 = "6.1.0-alpha1"
let version8 = "6.0.0-alpha3"

parseDateTime row2.Value.``Last updated``

parseRow row1
parseRow row2
parseRow row3
parseRow row4
parseRow row5
parseRow row6
parseRow row7
parseRow row8

//let tryParseRow (row:Package.VersionHistory.Row option) =
//    match row with
//    | None -> None
//    | Some row -> Some (parseRow row)

//// HtmlProvider<...>.VersionHistory.Row[] -> seq<VersionDetails>
//let parseRows (rows:Package.VersionHistory.Row[]) =
//    rows |> Seq.map parseRow

//// seq<VersionDetails> -> seq<Version>
//let getVersion (versionsDetails:seq<VersionDetails>) =
//    versionsDetails |> Seq.map(fun v -> v.Version)

//let loadPackageVersions = getPackage >> getVersionsForPackage //>> parseRows >> getVersion

//loadPackageVersions "EntityFramework"

//["EntityFramework"; "Newtonsoft.Json"]
//|> List.map getDownloadsForPackage

//getDetailsForVersion "6.4.0" "EntityFramework"

//// Не работает
//getDetailsForCurrentVersion "Newtonsoft.Json"