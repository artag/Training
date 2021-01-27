open System

// Imperative implementation of length
let length inputs =
    let mutable accumulator = 0
    for input in inputs do
        accumulator <- accumulator + 1
    accumulator

let fruits = [ "Apples"; "Apples"; "Apples"; "Bananas"; "Pineapples" ]
fruits |> length

// Imperative implementation of max
let max inputs =
    let mutable accumulator = 0
    for input in inputs do
        if input > accumulator then accumulator <- input
    accumulator

let numbers = [ 3; 4; 10; 3; 2; 0; -1; 42; 2; 7; 8; 1]
numbers |> max
