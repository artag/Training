open System
open System.IO

Directory.EnumerateDirectories(@"E:\")
|> Seq.map (fun dir -> DirectoryInfo dir)
|> Seq.map (fun dirInfo -> (dirInfo.Name, dirInfo.CreationTimeUtc))
|> Map.ofSeq
|> Map.map (fun dir date -> (DateTime.Now - date).Days)

let getExtensions folder =
    Directory.GetFiles(folder)
    |> Seq.map (fun file -> (FileInfo file).Extension)
    |> Set.ofSeq

let ex1 = getExtensions "E:\Downloads"
let ex2 = getExtensions "E:\Materials"

let sharedEx = ex1 |> Set.intersect ex2
