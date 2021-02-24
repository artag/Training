namespace BankFS

open System

/// �������.
type CommandType =
    | AccountOperation of AccountOperationType
    | Exit

/// �������.
module Command =
    /// ��������� ������� ������.
    let private executeExitCommand account =
        printf "Exit. "
        account

    /// ��������� �������.
    let execute auditFunc account (command : CommandType, amount) =
        match command with
        | AccountOperation operation ->
            //let transaction = Transaction.createDateTimeNow operation amount
            let transaction = Transaction.create DateTime.Now operation amount
            let updatedAccount = Transaction.parse account transaction
            auditFunc transaction updatedAccount
            updatedAccount
        | Exit -> executeExitCommand account

    /// ��������� ������� � �������.
    let executeWithAudit = execute Auditing.logComposite

    /// ������������� ������ � ��������� �������.
    let tryParseChar command =
        match command with
        | 'd' -> Some(AccountOperation Deposit)
        | 'w' -> Some(AccountOperation Withdraw)
        | 'x' -> Some Exit
        | _ -> None
