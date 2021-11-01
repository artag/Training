module ``Business Logic Tests``
open BusinessLogic
open Xunit

let department = {
        Name = "Super Team"
        Team = [ for i in 1..15 -> { Name = $"Person %d{i}"; Age = 19 } ]
    }

// A simple wrapper around Assert.True
let isTrue (b:bool) = Assert.True b

[<Fact>]
let ``Large, young teams are correctly identified``() =
    // department |> isLargeAndYoungTeam |> Assert.True
    department |> isLargeAndYoungTeam |> isTrue

open FsUnit.Xunit

// (1) - FsUnit's custom language functions for equality checking
// (2) - Custom checks for "greater than"
[<Fact>]
let ``FSUnit makes nice DSLs!``() =
    department
    |> isLargeAndYoungTeam
    |> should equal true            // (1)

    department.Team.Length
    |> should be (greaterThan 10)   // (2)

open Swensen.Unquote

// The custom =! operator fails if the values on both sides aren't equal.
[<Fact>]
let ``Unquote has a simple custom operator for equality``() =
    department |> isLargeAndYoungTeam =! true

// (1) - Wrapping a condition within a quitation block
[<Fact>]
let ``Unquote can parse quotations for excellent diagnostics``() =
    let emptyTeam = { Name = "Super Team"; Team = [] }
    test <@ emptyTeam.Name.StartsWith "S" @>            // (1)