# Lesson 29. Capstone 5

### Shown XML comments (from F# triple-slash declarations) outside the F# library

Ensure that you have the *XML Documentation File* selected in the *Build* tab of the *Properties*
pane of the F# project.

### Creating a member field on a discriminated union (DU)

Полезно при использовании в C#. Иногда полезно (но не обязательно) в F#.

```fsharp
type RatedAccount =
    | InCredit of CreditAccount
    | Overdrawn of Account
    member this.Balance =       // Member declaration
        match this with         // Self-matching to access nested fields
        | InCredit (CreditAccount account) -> account.Balance
        | Overdrawn account -> account.Balance

    // Или. Более универсальный способ получения значения какого-либо поля Account
    // (Account -> 'a)
    member this.GetField getter =
    match this with
    | InCredit (CreditAccount account) -> getter account
    | Overdrawn account -> getter account

// Пример. Использование универсального поля
let accountId = loadedAccount.GetField(fun a -> a.AccountId)
```

### Encapsulation

`internal` or `private` на module ограничивают область его видимости
(полезно для пользователей библиотеки на F#).

Пример:

```fsharp
module internal Capstone5.Operations
```

### Naming conventions. Attribute `[<CompiledName>]`

Для API модулей F#, которые видны и вызываются из C#, принято использовать наименование
с *большой* буквы (Pascal).

Поэтому:

* Либо функции в API на F# писать с большой буквы.

* Либо использовать на функциях атрибут `[<CompiledName>]`, to change the
function name post-compile.

Пример:

```fsharp
[<CompiledName "ClassifyAccount">]
```
