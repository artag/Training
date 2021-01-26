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


// groupBy
let firstLetter (str:string) = str.[0]
["apple"; "alice"; "bob"; "carrot"] |> List.groupBy firstLetter
// [('a', ["apple"; "alice"]); ('b', ["bob"]); ('c', ["carrot"])]

type Cust = { Name : string; Town : string }
let custs = [
    { Name = "Isaac"; Town = "London" }
    { Name = "Sara"; Town = "Birmingham" }
    { Name = "Tim"; Town = "London" }
    { Name = "Michelle"; Town = "Manchester" } ]
custs |> List.groupBy (fun person -> person.Town)
// [("London", [{ Name = "Isaac"; Town = "London" }
//              { Name = "Tim"; Town = "London" }])
//  ("Birmingham", [{ Name = "Sara"; Town = "Birmingham" }]);
//  ("Manchester", [{ Name = "Michelle"; Town = "Manchester" }])]


// countBy
custs |> List.countBy (fun person -> person.Town)
// [("London", 2); ("Birmingham", 1); ("Manchester", 1)]

[ ("a","A"); ("b","B"); ("a","C") ] |> List.countBy fst
// [("a", 2); ("b", 1)]

[ ("a","A"); ("b","B"); ("a","C") ] |> List.countBy snd
// [("A", 1); ("B", 1); ("C", 1)]


// partition
let londonCustomers, otherCustomers =
    custs |> List.partition(fun c -> c.Town = "London")

let isEven i = (i % 2 = 0)
[1..10] |> List.partition isEven
// ([2; 4; 6; 8; 10], [1; 3; 5; 7; 9])


// chunkBySize, splitInto, splitAt
[1..10] |> List.chunkBySize 3
// [[1; 2; 3]; [4; 5; 6]; [7; 8; 9]; [10]]
[1] |> List.chunkBySize 3
// [[1]]

[1..10] |> List.splitInto 3
// [[1; 2; 3; 4]; [5; 6; 7]; [8; 9; 10]]
[1] |> List.splitInto 3
// [[1]]

['a'..'i'] |> List.splitAt 3
// (['a'; 'b'; 'c'], ['d'; 'e'; 'f'; 'g'; 'h'; 'i'])

['a'; 'b'] |> List.splitAt 3
// InvalidOperationException: The input sequence has an insufficient number of elements.


// Aggregation functions
let numbers = [1.0..10.0]
let total = numbers |> List.sum         // 55.0
let average = numbers |> List.average   // 5.5
let max = numbers |> List.max           // 10.0
let min = numbers |> List.min           // 1.0


// find, findIndex, findback, findIndexBack
let listOfTuples = [ (1,"a"); (2,"b"); (3,"b"); (4,"a"); ]
listOfTuples |> List.find(fun (x,y) -> y = "b")
// (2, "b")

listOfTuples |> List.findIndex(fun (x,y) -> y = "b")
// 1

listOfTuples |> List.findBack(fun (x,y) -> y = "b")
// (3, "b")

listOfTuples |> List.findIndexBack(fun (x,y) -> y = "b")
// 2


// head, last
[2..10] |> List.head    // 2
[2..10] |> List.last    // 10


// item
[3..12] |> List.item(3)      // 6
[3..12] |> List.item(10)     // System.ArgumentException


// take, truncate, takeWhile, skip, skipWhile
[1..10] |> List.take 3          // [1; 2; 3]
[1..10] |> List.take 11         // InvalidOperationException

[1..5] |> List.truncate 4       // [1; 2; 3; 4]
[1..5] |> List.truncate 11      // [1; 2; 3; 4; 5]

[1..10] |> List.takeWhile(fun i -> i < 3)    // [1; 2]
[1..10] |> List.takeWhile(fun i -> i < 1)    // []

[1..10] |> List.skip 3      // [4; 5; 6; 7; 8; 9; 10]
[1..10] |> List.skip 11     // ArgumentException

[1..5] |> List.skipWhile(fun i -> i < 2)    // [2; 3; 4; 5]
[1..5] |> List.skipWhile(fun i -> i < 6)    // []


// exists, exists2
[1..10] |> List.exists(fun i -> i > 3 && i < 5)    // true
[1..10] |> List.exists(fun i -> i > 5 && i < 3)    // false

let ints1 = [2; 3; 4]
let ints2 = [5; 6; 7]
(ints1, ints2) ||> List.exists2(fun i1 i2 -> i1 + 10 > i2)     // true


// forall, forall2
ints1 |> List.forall(fun i -> i > 5)        // false
(ints1, ints2) ||> List.forall2(fun i1 i2 -> i1 < i2)       // true


// contains
[1..10] |> List.contains 5      // true
[1..10] |> List.contains 42     // false


// filter, where
[1..10] |> List.filter(fun i -> i % 2 = 0)      // [2; 4; 6; 8; 10]
[1..10] |> List.where(fun i -> i % 2 = 0)       // [2; 4; 6; 8; 10]


// length, distinctBy
[1..11] |> List.length      // 11

[(1,"a"); (1,"b"); (1,"c"); (2,"d")] |> List.distinctBy fst    // [(1, "a"); (2, "d")]


// distinct
[1; 1; 1; 2; 3; 3; 1] |> List.distinct      // [1; 2; 3]


// rev, sort, sortBy, sortDescending, sortByDescending, sortWith

[1..5] |> List.rev
// [5; 4; 3; 2; 1]

[2; 4; 1; 3; 5] |> List.sort
// [1; 2; 3; 4; 5]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortBy fst
// [("a", "3"); ("b", "2"); ("c", "1")]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortBy snd
// [("c", "1"); ("b", "2"); ("a", "3")]

[2; 4; 1; 3; 5] |> List.sortDescending
// [5; 4; 3; 2; 1]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortByDescending fst
// [("c", "1"); ("b", "2"); ("a", "3")]

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortByDescending snd
// [("a", "3"); ("b", "2"); ("c", "1")]


// sortWith
// example of a comparer
let tupleComparer tuple1 tuple2  =
    if tuple1 < tuple2 then 
        -1 
    elif tuple1 > tuple2 then 
        1 
    else
        0

[ ("b","2"); ("a","3"); ("c","1") ] |> List.sortWith tupleComparer
// [("a", "3"); ("b", "2"); ("c", "1")]
