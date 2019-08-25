<Query Kind="FSharpProgram" />

open System

let loop_stuff() =
    let magic_num = "7"
    let mutable guess = ""
    
    while not (magic_num.Equals(guess)) do
        printf "Guess the Number : "
        guess <- Console.ReadLine()
        
    printfn "You Guessed the Number"
    
loop_stuff()