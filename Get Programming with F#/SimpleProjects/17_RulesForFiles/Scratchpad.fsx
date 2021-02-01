#load "Checks.fs"

open Checks
open System
open System.IO

let getFiles folder =
    Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly)

let getAndFilterFiles filter folder =
    getFiles folder
    |> Seq.map(fun file -> FileInfo file)
    |> Seq.filter(fun fi -> fi |> filter)

getFiles "E:\Temp"

let filterMp3 = buildFilter[ (checkExtension ".mp3") ]

// Use mp3 filter
"E:\Temp" |> getAndFilterFiles filterMp3

let filterBigMp3 = buildFilter [
    checkExtension ".mp3"
    checkFileIsLarger 200000000L ]

// Use mp3 filter with size
"E:\Music\Mp3\Set's" |> getAndFilterFiles filterBigMp3

let filterNewBigMp3 = buildFilter [
    checkExtension ".mp3"
    checkFileIsLarger 200000000L
    checkFileCreatedAfter (System.DateTime(2019, 01, 01)) ]

// Use mp3 filter with size and date
"E:\Music\Mp3\Set's" |> getAndFilterFiles filterNewBigMp3
