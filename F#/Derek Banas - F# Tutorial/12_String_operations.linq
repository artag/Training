<Query Kind="FSharpProgram" />

open System

let string_stuff() =
    // \n - new line
    // etc.
    // %% - percent sign
    
    let str1 = "This is a random string"
    let str2 = @"I ignore backslashes \"
    let str3 = """ "I ignore double quotes and backslashes \" """
    
    printfn "str1 = %s" str1    // This is a random string
    printfn "str2 = %s" str2    // I ignore backslashes \
    printfn "str3 = %s" str3    // "I ignore double quotes and backslashes \"
    printfn ""
    
    let str1and2 = str1 + " " + str2
    
    printfn "str1and2 = %s" str1and2                // This is a random string I ignore backslashes \
    printfn "Length : %i" (String.length str1and2)  // Length : 46
    printfn "%c" str1.[1]                           // h
    printfn "1st word : %s" (str1.[0..3])           // 1st word : This
    printfn ""
    
    // Обработка каждого символа в строке
    let upper_str = String.collect (fun c -> sprintf "%c, " c) "commas"
    printfn "Commas : %s" upper_str                 // Commas : c, o, m, m, a, s, 
    
    // Есть ли в строке символы в верхнем регистре
    printfn "Any upper : %b" (String.exists (fun c -> Char.IsUpper(c)) str1)        // Any upper : true
    
    // Все ли символы в строке являются числами
    printfn "All numbers : %b" (String.forall (fun c -> Char.IsDigit(c)) "1234")    // All numbers : true
    
    // Создание чисел от 0 до 9 и их вывод
    let string1 = String.init 10 (fun i -> i.ToString())
    printfn "Numbers : %s" string1                  // Numbers : 0123456789
    
    // Печать каждого символа из строки на отдельной строке
    String.iter(fun c -> printfn "%c" c) "Print Me"
    
string_stuff()