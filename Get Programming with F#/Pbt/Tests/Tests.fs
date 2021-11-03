module Tests

open System
open FsCheck
open FsCheck.Xunit

// Verbose = true - показ генерируемых входных параметров при запуске теста
// Для .NET Core, чтобы увидеть эти генерируемые параметры, тесты надо запустить из консоли так:
// dotnet test --logger:"console;verbosity=detailed"
[<Property(Verbose = true)>]
let ``Correctly add numbers`` numbers =
    let actual = Logic.sumNumbers numbers
    actual = List.sum numbers

// Этот тест всегда завершается с ошибкой
// [<Property(Verbose = true)>]
// let ``Incorrectly add numbers`` numbers =
//     let actual = Logic.failedSumNumbers numbers
//     actual = List.sum numbers

[<Property>]
let ``Always has same number of letters 1`` (input:string) =
    input <> null ==> lazy      // Adding a guard clause to an FsCheck property
        let output = input |> Logic.flipCase
        input.Length = output.Length

// Creating a class that contains arbitrary generators
// (1) - Creating a generator that creates a stream of letters
type LettersOnlyGen() =
    static member Letters() =
        Arb.Default.Char() |> Arb.filter Char.IsLetter      // (1)

// (2) - Attaching the generator to the property test
[<Property(Arbitrary = [| typeof<LettersOnlyGen> |], Verbose = true)>]
let ``Always has same number of letters 2`` (NonEmptyString input) =
    let output = input |> Logic.flipCase
    input.Length = output.Length
