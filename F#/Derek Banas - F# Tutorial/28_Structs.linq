<Query Kind="FSharpProgram" />

open System

// Structs - позволяет создавать пользовательские типы данных

// Определение структуры
type Rectangle = struct
    val Length : float
    val Width : float
    
    new (length, width) =
       { Length = length; Width = width }
end

let struct_stuff() =
    // Функция вычисления площади прямоугольника
    let area(shape: Rectangle) =
        shape.Length * shape.Width
    
    // Создание прямоугольника
    let rect = new Rectangle(5.0, 6.0)
    
    // Вычисление площади прямоугольника
    let rect_area = area rect
    printfn "Area: %.2f" rect_area          // Area: 30.00

struct_stuff()