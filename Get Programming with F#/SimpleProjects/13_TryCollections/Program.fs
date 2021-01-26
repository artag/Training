open System
open System.IO

let enterFolder() =
    Console.Write("Enter existing folder: ")
    Console.ReadLine()

let checkFolderExists folder =
    let exists = Directory.Exists folder
    exists

[<EntryPoint>]
let main argv =
    let folder = enterFolder()
    let exists = checkFolderExists folder
    if exists = false then
        Console.WriteLine($"{folder} does't exist. Exit.")
        Environment.Exit(1)

    Console.WriteLine($"Simple information about {folder}")
    folder
    |> SimpleFolderInfo.get
    |> SimpleFolderInfo.print
    Console.WriteLine();

    Console.WriteLine($"More complex information about {folder}")
    folder
    |> ComplexFolderInfo.get
    |> ComplexFolderInfo.print
    Console.WriteLine();
    0
