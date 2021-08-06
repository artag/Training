# Chapter 10

## Event sourcing: a functional approach to persistence (постоянству)

Two approaches of append-only data storage:
* *Assertion-based* - Treats the DB as an ever-growing (постоянно-растущая) collection of facts
that are true at a given point in time.
* *Event-based* - Treats the DB as an ever-growing collection of events that occur at
given points in time.

In both cases, data is never updated or deleted, only appended.

The traditional functions of a relational DB are the *CRUD* operations:
create, read, update, and delete.

The functional approach to data storage is *CRA*: create, read, append.

## 10.1 Thinking functionally about data storage

Many server applications today are inherently (по существу) *stateless*: when they receive a
request, they retrieve the required data from the database, do some processing, and persist the
relevant changes.

This also means that it’s relatively easy to avoid state mutation in a stateless server:
just create new, updated versions of the data, and persist those to the database.

### 10.1.1 Why data storage should be append-only

Причины:

1. Storage is cheap, and data is valuable.
  * Анализ данных (Big data, маркетинговые исследования и т.д.)
  * История действий (Системы контроля кода, банковские счета)
2. Append-only storage имеет еще одно преимущество: устраняет проблему конфликтов при
одновременном доступе.

### 10.1.2 Relax, and forget about storing state

*States* are snapshots (моментальные снимки) of an entity at a given time.

*Entity* is a sequence of logically related states.

*State transitions* cause (порождает) a new state to be associated with the entity.
Иначе: cause the entity to go from one state to the next.

Пример: банковский счет находится в определенном state, а завтра он будет в другом state
из-за произошедших events: таких как deposits, withdrawals, or interest charges (процентные
платежи). Events приводят к transition (переходу) счета из одного состояния в другое.

Обычный подход к хранению данных в БД:

In relational databases, we tend to only store the latest state of an entity, overwriting
previous states. When we really need to know about the past, we often use history
tables, in which we store *all* snapshots.

Event sourcing (ES):

Stores data about the events. It's always possible to reconstruct the current state of an
entity by "replaying" all the events that affected the entity. *State is secondary*.

## 10.2 Event sourcing basics

* *Events* can be represented as simple, immutable data objects capturing details of
what happened.
* *States* can also be represented as immutable data objects, although they may
have a more complex structure than events.
* *State transitions* can be represented as functions that take a state and an event,
and produce a new state.

### 10.2.1 Representing events

Events *should* (должны) be really simple. They're just plain data objects that capture the
*minimum* amount of information required to faithfully (точно) represent what happened.

Пример. Some events affecting a bank account:

```csharp
public abstract class Event
{
    // Identifies the affected entity (in this case, an account)
    public Guid EntityId { get; }
    public DateTime Timestamp { get; }
}

public sealed class CreatedAccount : Event
{
    public CurrencyCode Currency { get; }
}

public sealed class DepositedCash : Event
{
    public decimal Amount { get; }
    public Guid BranchId { get; }
}

public sealed class DebitedTransfer : Event
{
    public string Beneficiary { get; }
    public string Iban { get; }
    public string Bic { get; }
    public decimal DebitedAmount { get; }
    public string Reference { get; }
}
```

Events should be treated as being immutable: they represent things that happened in the past,
and there's no changing the past. They are persisted to storage, so they must also be
serializable.

### 10.2.2 Persisting events

Events have a different structure - different fields. You can't store them in a fixed-format
structure like a relational table.

Various options for storing events (in order of preference):

* A specialized event DB such as Event Store (https://geteventstore.com).
* A document database such as Redis, Mongo DB , and others. These storage systems make no
assumptions about the structure of the data they store.
* A traditional relational DB such as SQL Server.

Если требуется хранить events в relational DB, что для этого требуется:

* Header columns such as `EntityId` and `Timestamp`.
* Данные event'а  is serialized into a `JSON` string and stored into a wide column.

Пример:

| EntityId | Timestamp        | EventType        | Data                                           |
|----------|------------------|------------------|------------------------------------------------|
| abcd     | 2016-07-22 12:40 | CreatedAccount   | `{ “Currency”: “EUR” }`                        |
| abcd     | 2016-07-30 13:25 | DepositedCash    | `{ “Amount”: 500, “BranchId”: “BOCLHAYMCKT” }` |
| abcd     | 2016-08-03 10:33  | DebitedTransfer | `{ “DebitedAmount”: 300, “Beneficiary”: “Rose Stephens”, ...}` |

### 10.2.3 Representing state

Какое назначение для states, если у нас есть events:

* (Основная причина). We need snapshots to make decisions on how to process commands. For example,
if we receive a command that we should make a transfer, and the account is frozen or has an
insufficient balance, then we must reject the command.
* We also need snapshots to display to users. I'll refer to these as *view models*.

A simplified model of the entity state:

```csharp
public sealed class AccountState
{
    public AccountStatus Status { get; }
    public CurrencyCode Currency { get; }
    public decimal Balance { get; }
    public decimal AllowedOverdraft { get; }

    public AccountState WithStatus(AccountStatus newStatus)    // ...

    public AccountState Debit(decimal amount) =>
        Credit(-amount);

    public AccountState Credit(decimal amount) =>
        new AccountState(
            Balance: this.Balance + amount,
            Currency: this.Currency,
            Status: this.Status,
            AllowedOverdraft: this.AllowedOverdraft
        );
}
```

This is dumb, immutable data object with read-only properties, some copy methods such as
`WithStatus` , and with methods named `Debit` and `Credit`, which are also just copy methods
and contain no business logic.
