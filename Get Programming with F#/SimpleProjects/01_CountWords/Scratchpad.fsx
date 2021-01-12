open System.IO

let countWords (inputWord : string) =
    let splitted = inputWord.Split()
    splitted.Length

countWords "Test simple word"

let saveToFile (filename : string) (inputWord : string) =
    let length = countWords inputWord
    use fs = new FileStream(filename, FileMode.OpenOrCreate)
    use writer = new StreamWriter(fs)
    writer.WriteLine(inputWord)
    writer.WriteLine(length)
    0

saveToFile "test.txt" "Test simple word"
