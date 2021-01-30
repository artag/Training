module Rules

open System

type Rule = string -> bool * string

let mustBe3Words enableLog : Rule =
    fun text ->
        if enableLog then printfn "Running 3-word rule..."
        (text.Split ' ').Length = 3, "Must be three words"

let maxLengthIs30Characters enableLog : Rule =
    fun text ->
        if enableLog then printfn "Running max length (30 chars) rule..."
        text.Length <= 30, "Max length is 30 characters"

let allLettersMustBeCaps enableLog : Rule =
    fun text ->
      if enableLog then printfn "Running all letters must be caps rule..."
      text
      |> Seq.filter Char.IsLetter
      |> Seq.forall Char.IsUpper, "All letters must be caps"

let textWithoutNumbers enableLog : Rule =
    fun text ->
       if enableLog then printfn "Running text must be without any numbers..."
       not (text |> Seq.exists Char.IsDigit), "Text contains number"