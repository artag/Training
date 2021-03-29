printfn "Sync. Loading data!"
System.Threading.Thread.Sleep(5000)
printfn "Sync. Loaded Data!"
printfn "Sync. My name is Simon 1."

async {
    printfn "Async. Loading data!"
    System.Threading.Thread.Sleep(5000)
    printfn "Async. Loaded Data!" }
|> Async.Start

printfn "Sync. My name is Simon 2."

// Sync. Loading data!
// Sync. Loaded Data!
// Sync. My name is Simon 1.
// Sync. My name is Simon 2.
// Async. Loading data!
// Async. Loaded Data!

let asyncHello : Async<string> = async { return "Hello" }
//let length = asyncHello.Length
let text = asyncHello |> Async.RunSynchronously
let lengthTwo = text.Length

//val asyncHello : Async<string>
//val text : string = "Hello"
//val lengthTwo : int = 5

open System.Threading

let printThread text = printfn "THREAD %d: %s" Thread.CurrentThread.ManagedThreadId text

let doWork() =
    printThread "Starting long running work!"
    Thread.Sleep 5000
    "HELLO"

let asyncLength : Async<int> =
    printThread "Creating async block"
    let asyncBlock =
        async {
            printThread "In block!"
            let text = doWork()
            return (text + " WORLD").Length }
    printThread "Created async block"
    asyncBlock

printThread "Run async block"
let length = asyncLength |> Async.RunSynchronously
printThread "Done!"
printfn "Length: %i" length

(*
    Creating a continuation by using let!
*)
let getTextAsync =
    printfn "1.getTextAsync. Before async"
    async {
        printfn "6.getTextAsync. async"
        return "HELLO" }
let printHelloWorld =
    printfn "2.printHelloWorld. Before async."
    async {
        printfn "5.printHelloWorld. async. Before getTextAsync"
        let! text = getTextAsync
        printfn "7.printHelloWorld. async. After getTextAsync"
        return printf "%s WORLD" text }

printfn "3.Before Async.Start"
printHelloWorld |> Async.Start
printfn "4.After Async.Start"

//1.getTextAsync. Before async
//2.printHelloWorld. Before async.
//3.Before Async.Start
//4.After Async.Start
//5.printHelloWorld. async. Before getTextAsync
//6.getTextAsync. async
//7.printHelloWorld. async. After getTextAsync
//HELLO WORLD

let random = System.Random()
let pickANumberAsync = async { return random.Next(10) }
let createFiftyNumbers =
    let workflows = [ for i in 1 .. 50 -> pickANumberAsync ]
    async {
        let! numbers = workflows |> Async.Parallel
        printfn "Total is %d" (numbers |> Array.sum) }

createFiftyNumbers |> Async.Start

(*
    Downloading data from HTTP resources
*)
open System
open System.Net

let downloadData (url : string) = async {
        let wc = new WebClient()
        printfn "Downloading data on thread %d" System.Threading.Thread.CurrentThread.ManagedThreadId
        let! data = wc.AsyncDownloadData(Uri url)
        return data.Length }

let downloadBytes urls =
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.RunSynchronously

let printResult bytesCount =
    printfn "You downloaded %d characters" (Array.sum bytesCount)

[|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]
|> downloadBytes
|> printResult

(*
Downloading data from HTTP resources. Using Task
*)
//open System
//open System.Net
//open System.Threading.Tasks

//let downloadData (url : string) = async {
//    let wc = new WebClient()
//    printfn "Downloading data on thread %d" System.Threading.Thread.CurrentThread.ManagedThreadId
//    let! data = wc.DownloadDataTaskAsync(Uri url) |> Async.AwaitTask
//    return data.Length }

//let downloadBytes urls =
//    urls
//    |> Array.map downloadData
//    |> Async.Parallel
//    |> Async.StartAsTask

//let printResult (downloadedBytes : Task<int[]>)=
//    printfn "You downloaded %d characters" (Array.sum downloadedBytes.Result)

//[|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]
//|> downloadBytes
//|> printResult
