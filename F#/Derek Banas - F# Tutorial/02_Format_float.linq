<Query Kind="FSharpProgram" />

open System

let pi = 3.141592653589793238462643383
let big_pi = 3.141592653589793238462643383M

let hello() =
    // По умолчанию выводит 6 знаков после запятой
    printf "PI : %f" pi
    printf "\n"
    
    // Вывод 4 знаков
    printf "PI : %.4f" pi
    printf "\n"
    
    // Вывод всех знаков после запятой
    printf "Big PI : %M" big_pi
    printf "\n"
    
hello()
