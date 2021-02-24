module BankFS.Terminal

open System

/// Пользовательский ввод.
module UserInput =
    /// Ввод имени владелеца счета. Создание владельца банковского счета.
    let enterNameAndCreateOwner() =
        Console.Write "Please enter your name: "
        let name = Console.ReadLine()
        Owner.create name

    /// Ввод комманд в виде символов.
    let commandChars =
        Seq.initInfinite(fun _ ->
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            let output = Console.ReadKey().KeyChar
            Console.WriteLine()
            output)

    /// Комманды на выполнение.
    let commands =
        commandChars
        |> Seq.choose Command.tryParseChar
        |> Seq.takeWhile ((<>) Exit)

    /// Ввод количества денежных средств для пополнения или снятия с банковского счета.
    let getAmount command =
        let captureAmount _ =
            Console.Write "Enter Amount: "
            Console.ReadLine() |> Decimal.TryParse
        Seq.initInfinite captureAmount
        |> Seq.choose(fun amount ->
            match amount with
            | true, amount when amount <= 0M -> None
            | false, _ -> None
            | true, amount -> Some(command, amount))
        |> Seq.head

/// Открытие аккаунта.
/// Попытка загрузить историю операций по банковскому счету.
/// Если загрузка не удалась, создается новый банковский счет.
let openAccount owner =
    let transactions = FileRepository.tryLoadTransactions owner
    let accountId = FileRepository.tryGetAccountId owner
    let id = accountId |> Option.defaultValue (Guid.NewGuid())
    let openedAccount = Account.create id owner
    (openedAccount, transactions) ||> List.fold(fun a t-> Transaction.parse a t)

/// Запуск работы с определенным банковским счетом.
let run account =
    UserInput.commands
    |> Seq.map UserInput.getAmount
    |> Seq.fold Command.executeWithAudit account

// Печать информации об открытом банковском счете.
let printOpeningAccountInfo account =
    let balance = Account.getBalance account
    Console.WriteLine $"Opening balance is ${balance}"

// Печать информации о банковском счете после выполнения всех операций.
let printResultAccountInfo account =
    let balance = Account.getBalance account
    Console.WriteLine $"Result balance is ${balance}"
