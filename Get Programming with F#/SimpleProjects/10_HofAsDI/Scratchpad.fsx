open System
open System.IO

type Customer = { Age : int }

let printCustomerAge writer customer =
    if customer.Age < 13 then writer "Child!"
    elif customer.Age < 20 then writer "Teenager!"
    else writer "Adult!"

let writeToFile text = File.WriteAllText("E:\\test.txt", text);

let printToConsole = printCustomerAge Console.WriteLine
let printToFile = printCustomerAge writeToFile

printToConsole { Age = 10 }
printToConsole { Age = 19 }
printToConsole { Age = 33 }

printToFile { Age = 33 }