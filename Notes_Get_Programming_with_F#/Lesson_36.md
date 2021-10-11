# Lesson 36. Asynchronous workflows

### Several ways of performing background work in .NET

* *Thread* - lowest primitive for allocating background work.

* *Task* - higher-level abstraction over thread.

* *I/O-bound workloads* - background tasks that you want to execute that don’t need a thread
to run on. These are typically used when communicating and waiting for work from an external
system to complete

Examples of I/O- and CPU-bound workloads:

| Type | Example                                           |
|------|---------------------------------------------------|
| CPU  | Calculating the average of a large set of numbers |
| CPU  | Running a set of rules over a loan application    |
| I/O  | Downloading data from a remote web server         |
| I/O  | Loading a file from disk                          |
| I/O  | Executing a long-running query on SQL             |

### async block in F#

Example:

```fsharp
// A conventional, synchronous sequential set of instructions
printfn "Sync. Loading data!"
System.Threading.Thread.Sleep(5000)
printfn "Sync. Loaded Data!"
printfn "Sync. My name is Simon 1."

// Wrapping a portion of code in an async block
async {
    printfn "Async. Loading data!"
    System.Threading.Thread.Sleep(5000)
    printfn "Async. Loaded Data!" }
|> Async.Start      // Starting the async block in the background

printfn "Sync. My name is Simon 2."
```

Output:

```text
Sync. Loading data!
Sync. Loaded Data!
Sync. My name is Simon 1.
Sync. My name is Simon 2.
Async. Loading data!
Async. Loaded Data!
```

### Returning the result from an async block

* Result of an async expression must be prefixed with the `return` keyword.

* Unwrap an `Async<_>` value by calling `Async.RunSynchronously` (грубое подобие `Task.Result`).

* `async` block doesn’t automatically start the work. To start it call
`RunSynchronously` or `Start`.

* Every call `RunSynchronously` on an async block, it will re-execute the code every time.

```fsharp
// Returning a value from an async block
let asyncHello : Async<string> = async { return "Hello" }       // val asyncHello : Async<string>

// Compiler error when trying to access a property of an async value
let length = asyncHello.Length      // <-- Error

// Executing and unwrapping an asynchronous block on the current thread
let text = asyncHello |> Async.RunSynchronously                 // val text : string = "Hello"
let lengthTwo = text.Length                                     // val lengthTwo : int = 5
```

More complex example

```fsharp
open System.Threading

let printThread text = printfn "THREAD %d: %s" Thread.CurrentThread.ManagedThreadId text

let doWork() =
    printThread "Starting long running work!"           // 5
    Thread.Sleep 5000
    "HELLO"

let asyncLength : Async<int> =
    printThread "Creating async block"                  // 1
    let asyncBlock =
        async {
            printThread "In block!"                     // 4
            let text = doWork()                         // 5
            return (text + " WORLD").Length }
    printThread "Created async block"                   // 2
    asyncBlock

printThread "Run async block"                           // 3
let length = asyncLength |> Async.RunSynchronously
printThread "Done!"                                     // 6
printfn "Length: %i" length                             // 7
```

Output:

```text
THREAD 1: Creating async block
THREAD 1: Created async block
THREAD 1: Run async block
THREAD 7: In block!
THREAD 7: Starting long running work!
THREAD 1: Done!
Length: 11
```

### Creating a continuation by using `let!`

* `let!` - is valid only when inside the `async` block (you can't use it outside)

* `let!` waits for `asyncWork` to complete in the background
(it doesn't block a thread), unwraps the value and then continues.

* `Async.Start` - perfect if you want to start the task that has no specific end result.

* Value `text` is a type string.

* `async` blocks also allow you to perform `try/with` blocks around a `let!` computation;
you can nest multiple computations together and use .NET IDisposables without a problem.

```fsharp
let getTextAsync = async { return "HELLO" }
let printHelloWorld =
    async {
        // Using the let! keyword to asynchronously unwrap the result
        let! text = getTextAsync
        // Continuing work with the unwrapped string
        return printf "%s WORLD" text }

// Starting the entire workflow in the background
printHelloWorld |> Async.Start
```

### Using fork/join with `Async.Parallel`

* `Async.Parallel` объединяет коллекцию асинхронных рабочих процессов в один
комбинированный процесс.

* `Async.Parallel` similar to `Task.WhenAll`

Example 1:

```fsharp
let random = System.Random()
let pickANumberAsync =                                          // Async<int>
    async { return random.Next(10) }
let createFiftyNumbers =                                        // Async<unit>
    // Creating 50 asynchronous computations
    let workflows = [ for i in 1 .. 50 -> pickANumberAsync ]    // Async<int> list
    async {
        // Executing all computations in parallel and unwrapping the collated results
        let! numbers = workflows |> Async.Parallel    // Async<int> list |> Async<int[]> -> int[]
        printfn "Total is %d" (numbers |> Array.sum) }

createFiftyNumbers |> Async.Start
```

Example 2. Asynchronously downloading data over HTTP in parallel.

```fsharp
let downloadData (url : string) = async {   // string -> Async<int>
        let wc = new WebClient()
        printfn "Downloading data on thread %d" Thread.CurrentThread.ManagedThreadId
        let! data = wc.AsyncDownloadData(Uri url)
        return data.Length }

let downloadBytes urls =                    // string[] -> int[]
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.RunSynchronously

let printResult downloadedBytes =           // int[] -> unit
    printfn "You downloaded %d characters" (Array.sum downloadedBytes)

[|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]
|> downloadBytes
|> printResult
```

###  Interoperating with `Task`

* `Async.AwaitTask` converts a task into an async workflow.
* `Async.StartAsTask` converts an async workflow into a task.

Example:

```fsharp
let downloadData (url : string) = async {   // string -> Async<int>
    let wc = new WebClient()
    // Using the AwaitTask combinator to convert from Tasks to Async
    let! data = wc.DownloadDataTaskAsync(Uri url) |> Async.AwaitTask
    return data.Length }

let downloadBytes urls =                    // string[] -> Task<int[]>
    urls
    |> Array.map downloadData
    |> Async.Parallel
    |> Async.StartAsTask    // Using the StartAsTask combinator to convert from Async to Task

let printResult (downloadedBytes : Task<int[]>)=
    printfn "You downloaded %d characters" (Array.sum downloadedBytes.Result)

[|"http://www.fsharp.org"; "http://microsoft.com"; "http://fsharpforfunandprofit.com"|]
|> downloadBytes
|> printResult
```

### Comparing tasks and async

In F# code recommended to use async workflows wherever possible.

| -                       | Task and async await                      | F# async workflows
|-------------------------|-------------------------------------------|--------------------
| Native support in F#    | Via async combinators                     | Yes
| Allows status reporting | Yes                                       | No
| Clarity                 | Hard to know where async starts and stops | Very clear
| Unification             | `Task` and `Task<T>` types                | Unified `Async<T>`
| Statefulness            | Task result evaluated only once           | Infinite

### Useful async keywords

| Command                | Usage
|------------------------|-------------------------------------------------------------------
| `let!`                 | Used within an `async` block to unwrap an `Async<T>` value to `T`
| `do!`                  | Used within an `async` block to wait for an `Async<unit>` to complete
| `return!`              | Used within an `async` block as a shorthand for `let!` and `return`
| `Async.AwaitTask`      | Converts `Task<T>` to `Async<T>`, or `Task` to `Async<unit>`
| `Async.StartAsTask`    | Converts `Async<T>` to `Task<T>`
| `Async.RunSychronously`| Synchronously unwraps `Async<T>` to `<T>`
| `Async.Start`          | Starts an `Async<unit>` computation in the background (fire-and-forget)
| `Async.Ignore`         | Converts `Async<T>` to `Async<unit>`
| `Async.Parallel`       | Converts `Async<T>` array to `Async<T array>`
| `Async.Catch`          | Converts `Async<T>` into a two-case DU of `T` or `Exception`

### Handle an exception raised in an async block by using the Async.Catch

*(External resource)*:

```fsharp
// exception handling in async using Async.Catch
let fetchAsync (name, url:string) =
    async {
        let uri = new System.Uri(url)
        let webClient = new WebClient()
        let! html = Async.Catch (webClient.AsyncDownloadString(uri))
        match html with
        | Choice1Of2 html -> printfn "Read %d characters for %s" html.Length name
        | Choice2Of2 error -> printfn "Error! %s" error.Message
    } |> Async.Start

// exception handling in async using regular try/with
let fetchAsync2 (name, url:string) =
    async {
        let uri = new System.Uri(url)
        let webClient = new WebClient()
        try
            let! html = webClient.AsyncDownloadString(uri)
            printfn "Read %d characters for %s" html.Length name
        with error -> printfn "Error! %s" error.Message
    } |> Async.Start

fetchAsync2 ("blah", "http://asdlkajsdlj.com")
```
