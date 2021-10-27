module ReaderMonad.Program

[<EntryPoint>]
let main argv =
    // The first realization
    // Reader.run FirstProgram.services FirstProgram.run

    // The second realization
    Reader.run SecondProgram.services SecondProgram.run
    0
