seq { 1; 2; 3 }

// map
let helloGood = 
    let list = ["a";"b";"c"]
    list |> List.map (fun element -> "hello " + element)

let timesTwo n = n * 2          // Mapping function
[ 1 .. 10 ] |> List.map timesTwo

let intList1 = [ 2; 3; 4 ]
let intList2 = [ 5; 6; 7 ]
let intList3 = [ 8; 9; 10 ]

List.map2 (fun i1 i2 -> i1 + i2) intList1 intList2 
List.map3 (fun i1 i2 i3 -> i1 + i2 + i3 ) intList1 intList2 intList3

// TIP use the ||> operator to pipe a tuple as two arguments
(intList1, intList2) ||> List.map2 (fun i1 i2 -> i1 + i2) 
// [7; 9; 11]
(intList1) |> List.mapi (fun index i1 -> index, i1)
// [(0, 2); (1, 3); (2, 4)]
(intList1, intList2) ||> List.mapi2 (fun index i1 i2 -> index, i1 + i2) 
// [(0, 7); (1, 9); (2, 11)]

['a' .. 'c' ] |> List.indexed
// [(0, 'a'); (1, 'b'); (2, 'c')]

intList1 |> List.indexed
// [(0, 2); (1, 3); (2, 4)]

type Person = { Name : string; Age : int }
[ "Isaac", 30; "John", 25; "Sarah", 18; "Faye", 27 ]
|> List.map(fun (name, age) -> { Name = name; Age = age})

// iter, iter2, iter3, iteri, iteri2
intList1 |> List.iter (printf "num = %i; ")
// num = 2; num = 3; num = 4; 

let sirs = [ { Name = "Isaac"; Age = 30 }; { Name = "John"; Age = 25 }; { Name = "Peter"; Age = 18 } ]
let ladies = [ { Name = "Sarah"; Age = 28 }; { Name = "Amy"; Age = 21 }; { Name = "Mary"; Age = 20 }]
(sirs, ladies) ||> List.iter2 (fun sir lady -> printfn "Pair: %s and %s" sir.Name lady.Name)

intList1 |> List.iteri(printf "(idx = %i num = %i); ")
// (idx = 0 num = 2); (idx = 1 num = 3); (idx = 2 num = 4);

(intList1,intList2) ||> List.iteri2 (fun idx n1 n2 -> printf "(index = %i sum = %i) " idx (n1 + n2))
// (index = 0 sum = 7) (index = 1 sum = 9) (index = 2 sum = 11)

// collect
type Order = { Id : int }
type Customer = { Id : int; Orders : Order list; Town : string }
let customers = [
    { Id = 1; Orders = [{ Id = 1 }; { Id = 2 }]; Town = "Moscow" }
    { Id = 2; Orders = [{ Id = 39 }]; Town = "Paris" }
    { Id = 4; Orders = [{ Id = 43 }; { Id = 56 }]; Town = "Rome" }
]
let orders = customers |> List.collect(fun c -> c.Orders)
// [{ Id = 1 }; { Id = 2 }; { Id = 39 }; { Id = 43 }; { Id = 56 }]

// pairwise
[ System.DateTime(2010,5,1)
  System.DateTime(2010,6,1)
  System.DateTime(2010,6,12)
  System.DateTime(2010,7,3) ]
|> List.pairwise
|> List.map(fun (a, b) -> b - a)
|> List.map(fun time -> time.TotalDays)
// [31.0; 11.0; 21.0]

['a'..'e'] |> List.pairwise
// [('a', 'b'); ('b', 'c'); ('c', 'd'); ('d', 'e')]
