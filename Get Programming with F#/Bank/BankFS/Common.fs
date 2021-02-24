/// Общий код.
module BankFS.Common

open System

/// Парсер.
module Parser =
    let private tryParse parser (value : string) =
        let parsed, result = parser value
        match parsed with
        | true -> Some result
        | false -> None

    let private tryParseOption parser (value : string option) =
        match value with
        | None -> None
        | Some v -> tryParse parser v

    let private boolParser (value : string) = Boolean.TryParse(value)
    let private dateTimeParser (value : string) = DateTime.TryParse(value)
    let private decimalParser (value : string) = Decimal.TryParse(value)
    let private guidParser (value : string) = Guid.TryParse(value)

    /// Преобразует string в bool
    let tryParseBool = tryParse boolParser

    /// Преобразует string option в bool option
    let tryParseBoolOption = tryParseOption boolParser

    /// Преобразует string в DateTime option
    let tryParseDateTime = tryParse dateTimeParser

    /// Преобразует string option в DateTime option
    let tryParseDateTimeOption = tryParseOption dateTimeParser

    /// Преобразует string в decimal option
    let tryParseDecimal = tryParse decimalParser

    ///Преобразует string option в decimal option
    let tryParseDecimalOption = tryParseOption decimalParser

    /// Преобразует string в Guid option
    let tryParseGuid = tryParse guidParser

    ///Преобразует string option в Guid option
    let tryParseGuidOption = tryParseOption guidParser

/// Массив.
module Array =
    /// Получает элемент из массива по его индексу.
    let private tryGetItem idx (array : 'a []) =
        try
            Some(array.[idx])
        with
        | :? System.IndexOutOfRangeException -> None

    /// Получает строку из массива по его индексу.
    let tryGetString idx (array : string []) = tryGetItem idx array