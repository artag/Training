module SimpleFolderInfo

open System.IO

let get folder =
    Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
    |> Array.map(fun file -> FileInfo(file))
    |> Array.groupBy(fun fileInfo -> fileInfo.DirectoryName)
    |> Array.map (fun (dir, filesInfo) ->
        dir, filesInfo
        |> Array.map(fun fileInfo -> fileInfo.Length)
        |> Array.sum)

let print (result : (string * int64) array) =
    result
    |> Array.iter(fun (dir, size) -> printfn "Folder: %s; Size: %i" dir size)