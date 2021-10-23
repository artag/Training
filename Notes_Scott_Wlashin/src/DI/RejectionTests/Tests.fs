module Tests

open Xunit

[<Fact>]
let ``First string is smaller than second one`` () =
    let expected = PureCode.Smaller
    let actual = PureCode.compareTwoStrings "a" "b"
    Assert.Equal(expected, actual)

[<Fact>]
let ``First string is equal to second one`` () =
    let expected = PureCode.Equal
    let actual = PureCode.compareTwoStrings "a" "a"
    Assert.Equal(expected, actual)

[<Fact>]
let ``First string is bigger than second one`` () =
    let expected = PureCode.Bigger
    let actual = PureCode.compareTwoStrings "b" "a"
    Assert.Equal(expected, actual)
