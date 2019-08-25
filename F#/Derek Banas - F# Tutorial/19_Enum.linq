<Query Kind="FSharpProgram" />

open System

// Enum
type emotion =
| joy = 0
| fear = 1
| anger = 2

// Проверка на enum
let check_feeling feeling =
    match feeling with
    | emotion.joy -> printfn "I'm joyful"
    | emotion.fear -> printfn "I'm fearful"
    | emotion.anger -> printfn "I'm angry"

check_feeling emotion.joy       // I'm joyful
check_feeling emotion.fear      // I'm fearful
check_feeling emotion.anger     // I'm angry