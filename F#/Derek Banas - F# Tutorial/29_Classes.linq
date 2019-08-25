<Query Kind="FSharpProgram" />

open System

// Определение класса
type Animal = class
    val Name: string
    val Height : float
    val Weight : float
    
    // Constructor
    new (name, height, weight) =
        { Name = name; Height = height; Weight = weight; }
    
    member x.Run =
        printfn "%s Runs" x.Name
end

// Наследование
type Dog(name, height, weight) = 
    inherit Animal(name, height, weight)
    
    member x.Bark =
        printfn "%s Barks" x.Name


let main() =
    let spot = new Animal("Spot", 20.5, 40.5)
    spot.Run                                    // Spot Runs
    
    let bowser = new Dog("Bowser", 20.5, 40.5)
    bowser.Run                                  // Bowser Runs
    bowser.Bark                                 // Bowser Barks

main()