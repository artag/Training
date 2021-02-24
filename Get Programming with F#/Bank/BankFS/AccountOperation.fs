namespace BankFS

/// Доступные операции с банковским счетом.
type AccountOperationType =
    | Deposit
    | Withdraw

/// Операция с банковским счетом.
module AccountOperation =
    /// Преобразовать строку в операцию с банковским счетом.
    let tryParseString (command : string) =
        let processedString = command.Trim().ToLower()
        match processedString with
        | "deposit" -> Some Deposit
        | "withdraw" -> Some Withdraw
        | _ -> None

    /// Преобразовать строку optional в операцию с банковским счетом.
    let tryParseStringOption (command : string option) =
        match command with
        | None -> None
        | Some v -> tryParseString v
