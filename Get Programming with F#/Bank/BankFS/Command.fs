namespace BankFS

open System

/// Команды.
type CommandType =
    | AccountOperation of AccountOperationType
    | Exit

/// Команда.
module Command =
    /// Выполнить команду выхода.
    let private executeExitCommand account =
        printf "Exit. "
        account

    /// Выполнить команду.
    let execute auditFunc account (command : CommandType, amount) =
        match command with
        | AccountOperation operation ->
            //let transaction = Transaction.createDateTimeNow operation amount
            let transaction = Transaction.create DateTime.Now operation amount
            let updatedAccount = Transaction.parse account transaction
            auditFunc transaction updatedAccount
            updatedAccount
        | Exit -> executeExitCommand account

    /// Выполнить команду с аудитом.
    let executeWithAudit = execute Auditing.logComposite

    /// Преобразовать символ в доступную команду.
    let tryParseChar command =
        match command with
        | 'd' -> Some(AccountOperation Deposit)
        | 'w' -> Some(AccountOperation Withdraw)
        | 'x' -> Some Exit
        | _ -> None
