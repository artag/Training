namespace BankFS

open System

/// Банковский счет.
type AccountType = {
    Id : Guid
    Owner : OwnerType
    Balance : decimal
}

/// Кредитный банковский счет.
type CreditAccountType = CreditAccountType of AccountType

/// Банковский счет, который может быть либо кредитным, либо овердрафтным.
type RatedAccountType =
    | InCredit of CreditAccountType
    | Overdrawn of AccountType

module Account =
    /// Определить к какому типу принадлежит счет.
    let classify (account : AccountType) =
        match account.Balance with
        | balance when balance <= 0M -> Overdrawn account
        | balance when balance > 0M  -> InCredit(CreditAccountType account)
        | _ -> failwith "Unsupported account type"

    /// Создать новый счет с нулевым балансом.
    let create id owner =
        let newAccount = {
            Id = id
            Owner = owner
            Balance = 0M }
        newAccount |> classify

    /// Преобразовать (развернуть) RatedAccountType -> AccountType.
    let unwrap ratedAccount =
        match ratedAccount with
        | InCredit (CreditAccountType acc) -> acc
        | Overdrawn acc -> acc

    /// Получить баланс банковского счета.
    let getBalance ratedAccount =
        let account = unwrap ratedAccount
        account.Balance

    /// Пополнить баланс счета определенной суммой.
    let deposit amount ratedAccount =
        let account = unwrap ratedAccount
        { account with Balance = account.Balance + amount } |> classify

    /// Снять с баланса счета определенную сумму.
    let withdraw amount (creditAccount : CreditAccountType)=
        let (CreditAccountType account) = creditAccount
        { account with Balance = account.Balance - amount } |> classify

