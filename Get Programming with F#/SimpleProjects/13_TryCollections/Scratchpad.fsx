open System
open System.IO

Directory.GetFiles("E:\Temp", "*.*", SearchOption.AllDirectories)
|> Array.map(fun file -> FileInfo(file))
|> Array.groupBy(fun file -> file.DirectoryName)
|> Array.map (fun (dir, files) ->
    dir, files
    |> Array.map(fun file -> file.Length) |> Array.sum)

type FolderInfo = 
    { Name : string
      Size : int64
      NumberOfFiles : int
      AverageFileSize : int64
      Extensions : string array }

Directory.GetFiles("E:\Temp", "*.*", SearchOption.AllDirectories)
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
