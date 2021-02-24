module BankFS.Program

[<EntryPoint>]
let main argv =
    let openedAccount =
        Terminal.UserInput.enterNameAndCreateOwner()
        |> Terminal.openAccount
    Terminal.printOpeningAccountInfo openedAccount

    let resultAccount = Terminal.run openedAccount
    Terminal.printResultAccountInfo resultAccount

    0
