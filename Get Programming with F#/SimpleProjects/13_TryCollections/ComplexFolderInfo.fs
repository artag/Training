module ComplexFolderInfo
open System.IO

type FolderInfo = 
    { Name : string
      Size : int64
      NumberOfFiles : int
      AverageFileSize : int64
      Extensions : string array }

let get folder =
    Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
    |> Array.map(fun file -> FileInfo(file))
    |> Array.groupBy(fun file -> file.DirectoryName)
    |> Array.map (fun (dir, files) ->
        let filesSizes = files |> Array.map(fun file -> file.Length)
        let numberOfFiles = files.Length
        let summaryFilesSize = filesSizes |> Array.sum
        let averageFilesSizes = summaryFilesSize / ((int64)numberOfFiles)
        let filesExtensions =
            files
            |> Array.map(fun file -> file.Extension)
            |> Array.distinct
        { Name = dir
          Size = summaryFilesSize
          NumberOfFiles = numberOfFiles
          AverageFileSize = averageFilesSizes
          Extensions = filesExtensions })

let print (foldersInfo : FolderInfo array) =
    foldersInfo
    |> Array.iter(fun fi ->
        printfn "Folder: %s" fi.Name
        printfn "Size: %d" fi.Size
        printfn "Number of files: %d" fi.NumberOfFiles
        printfn "Average file size: %d" fi.AverageFileSize
        printf "Files extensions: "
        fi.Extensions |> Array.iter(fun ext -> printf "%s; " ext)
        printfn "\n")
