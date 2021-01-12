open System
open System.IO

let countWords (inputWord : string) =
    let splitted = inputWord.Split()
    splitted.Length

let checkArgs (argv : string[]) =
    if argv.Length < 2 then
        printfn "Недостаточно входных аргументов"
        exit 1
    else
        0


let saveToFile (filename : string) (inputWord : string) =
    let length = countWords inputWord
    use fs = new FileStream(filename, FileMode.OpenOrCreate)
    use writer = new StreamWriter(fs)
    writer.WriteLine(inputWord)
    writer.WriteLine(length)
    0

[<EntryPoint>]
let main argv =
    let r = checkArgs argv
    let r = saveToFile argv.[0] argv.[1]
    0
