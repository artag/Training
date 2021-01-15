open System
open System.IO

let parse (person:string) =
    let parts = person.Split(' ')
    let playername = parts.[0]
    let game = parts.[1]
    let score = Int32.Parse(parts.[2])
    playername, game, score

let playername, game, score = parse "Mary Asteroids 2500"

let loadFile filename =
    let fileInfo = FileInfo filename
    let lastWriteTime = fileInfo.LastWriteTime
    filename, lastWriteTime

let file, date = loadFile "E:\file.txt"
