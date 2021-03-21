module private Print =
    open NuGet

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

[<EntryPoint>]
let main argv =
    NuGet.getPackageVersions "Newtonsoft.Json" |> Print.printNuGetPackage
    NuGet.tryGetPackageVersion "Newtonsoft.Json" "9.0.1" |> Print.tryPrintVersionDetails
    NuGet.tryGetPackageVersion "Newtonsoft.Json" "Unknown" |> Print.tryPrintVersionDetails

    0
