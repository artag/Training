module BankFS.Terminal

open System

/// ���������������� ����.
module UserInput =
    /// ���� ����� ��������� �����. �������� ��������� ����������� �����.
    let enterNameAndCreateOwner() =
        Console.Write "Please enter your name: "
        let name = Console.ReadLine()
        Owner.create name

    /// ���� ������� � ���� ��������.
    let commandChars =
        Seq.initInfinite(fun _ ->
            Console.Write "(d)eposit, (w)ithdraw or e(x)it: "
            let output = Console.ReadKey().KeyChar
            Console.WriteLine()
            output)

    /// �������� �� ����������.
    let commands =
        commandChars
        |> Seq.choose Command.tryParseChar
        |> Seq.takeWhile ((<>) Exit)

    /// ���� ���������� �������� ������� ��� ���������� ��� ������ � ����������� �����.
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

/// �������� ��������.
/// ������� ��������� ������� �������� �� ����������� �����.
/// ���� �������� �� �������, ��������� ����� ���������� ����.
let openAccount owner =
    let transactions = FileRepository.tryLoadTransactions owner
    let accountId = FileRepository.tryGetAccountId owner
    let id = accountId |> Option.defaultValue (Guid.NewGuid())
    let openedAccount = Account.create id owner
    (openedAccount, transactions) ||> List.fold(fun a t-> Transaction.parse a t)

/// ������ ������ � ������������ ���������� ������.
let run account =
    UserInput.commands
    |> Seq.map UserInput.getAmount
    |> Seq.fold Command.executeWithAudit account

// ������ ���������� �� �������� ���������� �����.
let printOpeningAccountInfo account =
    let balance = Account.getBalance account
    Console.WriteLine $"Opening balance is ${balance}"

// ������ ���������� � ���������� ����� ����� ���������� ���� ��������.
let printResultAccountInfo account =
    let balance = Account.getBalance account
    Console.WriteLine $"Result balance is ${balance}"
