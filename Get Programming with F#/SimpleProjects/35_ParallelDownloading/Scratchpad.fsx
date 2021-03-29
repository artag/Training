open System
open System.Diagnostics;
open System.Net
open System.Threading.Tasks

let downloadDataSync (url : string) =
    let wc = new WebClient()
    let dataLength =
        try
            let data = wc.DownloadData(url)
            data.Length
        with error -> 0
    printfn "\tDownloaded (sync) data from site %s" url
    dataLength

let downloadDataAsync url = async {
    let wc = new WebClient()
    let! data = Async.Catch <| wc.AsyncDownloadData(Uri url)
    let dataLength =
        match data with
        | Choice1Of2 data -> data.Length
        | Choice2Of2 error -> 0
    printfn "\tDownloaded (async) data from site %s" url
    return dataLength }

let downloadDataTask url = async {
    let wc = new WebClient()
    let! data = wc.DownloadDataTaskAsync(Uri url) |> Async.AwaitTask |> Async.Catch
    let dataLength =
        match data with
        | Choice1Of2 data -> data.Length
        | Choice2Of2 error -> 0
    printfn "\tDownloaded (with task) data from site %s" url
    return dataLength }

let printResult result =
    printfn "You downloaded %d characters" (Array.sum result)

let printResultTask (result : Task<int[]>) =
    printfn "You downloaded %d characters" (Array.sum result.Result)

let duration f urls = 
    let timer = new Stopwatch()
    timer.Start()
    let returnValue = f urls
    printfn "Elapsed Time: %sms" <| timer.Elapsed.TotalMilliseconds.ToString()
    returnValue

let downloadBytesSync urls =
    printfn "Synchronous download"
    urls
    |> Array.map downloadDataSync
    |> printResult

downloadBytesSync [|"unknown_host"|]


let downloadBytesAsync urls =
    printfn "Asynchronous download"
    urls
    |> Array.map downloadDataAsync
    |> Async.Parallel
    |> Async.RunSynchronously
    |> printResult

let downloadBytesTask urls =
    printfn "Download with task"
    urls
    |> Array.map downloadDataTask
    |> Async.Parallel
    |> Async.StartAsTask
    |> printResultTask

let testUrls = [| 
    "http://www.fsharp.org";
    //"http://unknownhost.com"
    "http://microsoft.com";
    "http://fsharpforfunandprofit.com";
    "https://www.manning.com/"
    "https://www.packtpub.com/"
    "https://habr.com/ru/top/"
    "https://www.ixbt.com/"
    "https://www.discogs.com/"
    "https://fsharp.org/"
|]

duration downloadBytesSync testUrls

duration downloadBytesAsync testUrls

duration downloadBytesTask testUrls
