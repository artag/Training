<Query Kind="FSharpProgram" />

open System

// Проверка, является ли число простым
// rec - функция является рекурсивной
let is_prime n =
    let rec check i =
        i > n/2 || (n % i <> 0 && check (i + 1))
    check 2

// Создание последовательности из простых чисел
let prime_seq = seq { for n in 1..500 do if is_prime n then yield n }

// Печать только 4-х первых значений
printfn "%A" prime_seq      // seq [1; 2; 3; 5; ...]

// Напечатает все простые числа в последовательности
Seq.toList prime_seq |> List.iter (printfn "Prime: %i")