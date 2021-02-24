namespace BankFS

/// Владелец счета.
type OwnerType = {
    Name : string
}

/// Владелец счета.
module Owner =
    /// Создать владельца счета.
    let create name = { Name = name }