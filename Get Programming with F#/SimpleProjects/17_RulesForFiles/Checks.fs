module Checks

open System
open System.IO

type Check = FileInfo -> bool

let checkExtension extension : Check =
    fun fileInfo ->
        fileInfo.Extension = extension

let checkFileIsLarger size : Check =
    fun fileInfo ->
        fileInfo.Length > size

let checkFileCreatedAfter datetime : Check =
    fun fileInfo ->
        fileInfo.LastWriteTime > datetime

let buildFilter (checks : Check seq) =
    checks
    |> Seq.reduce(fun firstRule secondRule ->
        fun fileInfo ->
            let passed = firstRule fileInfo
            if passed then secondRule fileInfo
            else false)
