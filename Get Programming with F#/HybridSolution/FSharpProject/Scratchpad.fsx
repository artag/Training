#r @"bin\Debug\netcoreapp3.1\CSharpProject.dll"

open System
open System.Collections.Generic

open CSharpProject

let simon = Person "Simon"
simon.PrintName()

let fred = Person "Fred"
fred.PrintName()

let printPerson (person : Person) =
    person.PrintName()

let longhand =
    [ "Tony1"; "Fred1"; "Samantha1"; "Brad1"; "Sophie1"]
    |> List.map(fun name -> Person(name))

let shorthand =
    [ "Tony2"; "Fred2"; "Samantha2"; "Brad2"; "Sophie2"]
    |> List.map Person

longhand |> List.iter printPerson
shorthand |> List.iter printPerson

type PersonComparer() =
    interface IComparer<Person> with
        member this.Compare(x, y) = x.Name.CompareTo(y.Name)

let pComparer = PersonComparer() :> IComparer<Person>
pComparer.Compare(simon, fred)              // 1
pComparer.Compare(simon, Person "Simon")    // 0

let pComparer2 =
    { new IComparer<Person> with
        member this.Compare(x, y) = x.Name.CompareTo(y.Name) } 

pComparer2.Compare(simon, fred)              // 1
pComparer2.Compare(simon, Person "Simon")    // 0

let blank:string = null
let name = "Vera"
let number = Nullable 10

let blankAsOption = blank |> Option.ofObj           // None
let nameAsOption = name |> Option.ofObj             // Some "Vera"
let numberAsOption = number |> Option.ofNullable    // Some 10
let unsafeName = Some "Fred" |> Option.toObj        // "Fred"

blankAsOption
nameAsOption
numberAsOption
unsafeName
