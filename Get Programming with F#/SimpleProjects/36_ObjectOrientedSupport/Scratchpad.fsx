(*
    Basic classes
*)

type Person(age, firstname, surname) =
    let fullName = sprintf "%s %s" firstname surname

    member __.PrintFullName() =
        printfn "%s is %d years old" fullName age

    member this.Age = age
    member that.Name = fullName
    member val FavouriteColour = "Green" with get, set

let person = Person(12, "Ivan", "Ivanov")
let name = person.Name                  // "Ivan Ivanov"
let age = person.Age                    // 12
let colour = person.FavouriteColour     // "Green"
person.FavouriteColour <- "Blue"        // Set FavouriteColour to "Blue"
person.PrintFullName()                  // Ivan Ivanov is 12 years old

(*
    Interfaces in F#
*)
type IQuack =
    abstract member Quack : unit -> unit

type Duck (name:string) =
    interface IQuack with
        member this.Quack() = printfn "QUACK!"

let duck = Duck "Donald"
let quackableDuck = duck :> IQuack
quackableDuck.Quack()

let quacker =
    { new IQuack with
        member this.Quack() = printfn "What type of animal am I?" }

quacker.Quack()

(*
    Inheritance in F#
*)
[<AbstractClass>]
type Employee(name:string) =
    member __.Name = name
    abstract member Work : unit -> string
    member this.DoWork() =
        printfn "%s is working hard: %s!" name (this.Work())

type ProjectManager(name:string) =
    inherit Employee(name)
    override this.Work() = "Creating a project plan"

let manager = ProjectManager "Peter"
manager.Work()
manager.DoWork()
