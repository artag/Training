# Chapter 9

## Thinking about data functionally

## 9.1 The pitfalls of state mutation

Example. A `Product` class with mutable `Inventory` field:

```csharp
public class Product
{
    public int Inventory { get; private set; }
    public void ReplenishInventory(int units) => Inventory += units;
    public void ProcessSale(int units) => Inventory -= units;
}
```

Побочные явления и не только из-за наличия mutable `Inventory`:

1. When concurrent threads updating `Inventory` value, the results can be unpredictable. This is
known as a *race condition*.
2. The risk of introducing *coupling* - a high degree of interdependence between different parts
of your system. How many parts of the application can get the same instance of the `Product` and
rely on the value of `Inventory`, and how many will be affected if you introduce a new
component that causes `Inventory` to change?
3. Mutable state implies (подразумевает) *loss of purity* (потерю чистоты). Mutating global
state (state not local to a function) constitutes a side effect.

>### Local mutation is OK
>Mutating local state (only visible whithin the scope of a function) неэлегантно, но допустимо.
>
> Example:
>```csharp
>int Sum(int[] ints)
>{
>    var result = 0;
>    foreach (int i in ints) result += i;
>        return result;
>}
>```

## 9.2 Understanding state, identity, and change

### 9.2.1 Some things never change

>### Immutable types in the .NET framework
>A few reference types in the framework are immutable. These are the most commonly used:
>* `DateTime`, `TimeSpan`, `DateTimeOffset`
>* `Delegate`
>* `Guid`
>* `Nullable4444<T>`
>* `String`
>* `Tuple<T1>`, `Tuple<T1, T2>`, ...
>* `Uri`
>* `Version`
>Furthermore, all primitive types in the framework are immutable.

Example of immutable `DateTime`:

```csharp
var momsBirthday = new DateTime(1966, 12, 13);
var johnsBirthday = momsBirthday;       // John has the same birthday as Mom.

// You then realize that John’s birthday is actually one day later.
johnsBirthday = johnsBirthday.AddDays(1);

johnsBirthday       // => 14/12/1966
momsBirthday        // => 13/12/1966  - Mom’s birthday was not affected.
```

В данном примере двойная защита от изменений:

* Because `System.DateTime` is a struct, it's copied upon assignment, so `momsBirthday` and
`johnsBirthday` are different instances.
* Even if `DateTime` were a class, so that `momsBirthday` and `johnsBirthday`
pointed to the same instance, the behavior would still be the same, because
`AddDays` creates a new instance, leaving the underlying instance unaffected.

Example of custom immutable type `Cicle`:

```csharp
struct Circle
{
    public Point Center { get; }
    public double Radius { get; }

    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }
}
```

Define functions to create a new circle based on an existing one:

```csharp
static Circle Scale(this Circle circle, double factor) =>
    new Circle(circle.Center, circle.Radius * factor);
```

### 9.2.2 Representing change without mutation

Приводится пример класса `AccountState`, у которого все свойства имеют `get` и `set`.
Лучше сохранить здесь более грамотную реализацию этого класса в следующем разделе 9.3.

*Совет*: несмотря на то, что копирование объектов довольно быстрое, может оказаться, что
для требуемой скорости работы приложения потребуется использовать изменяемые объекты.
Но во многих случаях такие потери в скорости несущественны, поэтому рекомендуется в начале
разработки делать упор на неизменяемость и только потом, по мере надобности, оптимизировать.

## 9.3 Enforcing immutability

New instances must then be populated by passing all values as arguments to the constructor:

```csharp
public enum AccountStatus { Requested, Active, Frozen, Dormant, Closed }

public class AccountState
{
    public AccountStatus Status { get; }
    public CurrencyCode Currency { get; }
    public decimal AllowedOverdraft { get; }
    public List<Transaction> Transactions { get; }

    public AccountState(
        CurrencyCode Currency,
        AccountStatus Status = AccountStatus.Requested,
        decimal AllowedOverdraft = 0,
        List<Transaction> Transactions = null)
    {
        this.Status = Status;
        this.Currency = Currency;
        this.AllowedOverdraft = AllowedOverdraft;
        this.Transactions = Transactions ?? new List<Transaction>();
    }

    // Copy method. All other fields are copied from the current state.
    public AccountState WithStatus(AccountStatus newStatus) =>
        new AccountState(
            Status: newStatus,                      // The updated field
            Currency: this.Currency,
            AllowedOverdraft: this.AllowedOverdraft,
            Transactions: this.TransactionHistory);
}
```

Obtaining a modified version of the object:

```csharp
var newState = account.WithStatus(AccountStatus.Frozen);
var account = new AccountState(Currency: "EUR", Status: AccountStatus.Active);
```

### 9.3.1 Immutable all the way down

Для неизменяемости коллекций можно воспользоваться библиотекой `System.Collections.Immutable`:

```csharp
using System.Collections.Immutable;

// Mark the class as sealed to prevent mutable subclasses.
public sealed class AccountState
{
    public IEnumerable<Transaction> TransactionHistory { get; }

    public AccountState(
        CurrencyCode Currency,
        AccountStatus Status = AccountStatus.Requested,
        decimal AllowedOverdraft = 0,
        IEnumerable<Transaction> Transactions = null)
    {
        // ...
        // Create and store a defensive copy of the given list.
        TransactionHistory = ImmutableList.CreateRange
            (Transactions ?? Enumerable.Empty<Transaction>());
    }
}
```

Adding a child to an immutable object requires creation of a new parent object:

```csharp
using LaYumba.Functional;   // Prepend as an extension method on IEnumerable

// (1) A new IEnumerable, including existing values and the one being added
// (2) All other fields are copied as usual.
public sealed class AccountState
{
    public AccountState Add(Transaction t) =>
        new AccountState(
            Transactions: TransactionHistory.Prepend(t),        // (1)
            Currency: this.Currency,                            // (2)
            Status: this.Status,                                // (2)
            AllowedOverdraft: this.AllowedOverdraft);           // (2)
}
```

### 9.3.2 Copy methods without boilerplate?

Здесь приводятся 2 попытки количественно минимизировать написание методов копирования `With`.

Попытка 1. A single With method that can set any property:

```csharp
// (1) null indicates that the field wasn't specified.
// (2) If no value was specified, use the current instance's value.
// (3) You can prevent arbitrary changes.
public AccountState With(
    AccountStatus? Status = null, decimal? AllowedOverdraft = null) =>      // (1)
        new AccountState(
            Status: Status ?? this.Status,                                  // (2)
            AllowedOverdraft: AllowedOverdraft ?? this.AllowedOverdraft,    // (2)
            Currency: this.Currency,                                        // (3)
            Transactions: this.TransactionHistory);                         // (3)
```

PROS:

* Fine-grained control over what operations we want to allow to change.
* Better performance compared to using the classic `With[Property]` methods: if we need to
update multiple fields, a single new instance is created.

CONS:

* Using `null` probably is not good idea.

Пример использования:

```csharp
public static AccountState Freeze(this AccountState account) =>
    account.With(Status: AccountStatus.Frozen);
public static AccountState RedFlag(this AccountState account) =>
    account.With
    (
        Status: AccountStatus.Frozen,
        AllowedOverdraft: 0m
    );
```

Попытка 2. A general-purpose copy method (see library `LaYumba.Functional`, class `Immutable`):

```csharp
public static T With<T>(this T source, string propertyName, object newValue)
    where T : class
{
    T newObj = source.ShallowCopy();

    typeof(T).GetBackingField(propertyName).SetValue(newObj, newValue);

    return newObj;
}

public static T With<T, P>(this T source, Expression<Func<T, P>> exp, object newValue)
    where T : class =>
        source.With(exp.MemberName(), newValue);

static T ShallowCopy<T>(this T source) =>
    (T)source.GetType()
             .GetTypeInfo()
             .GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic)
             .Invoke(source, null);

static string MemberName<T, P>(this Expression<Func<T, P>> e) =>
    ((MemberExpression)e.Body).Member.Name;

static FieldInfo GetBackingField(this Type t, string propertyName) =>
    t.GetTypeInfo()
     .GetField(BackingFieldName(propertyName), BindingFlags.Instance | BindingFlags.NonPublic);

static string BackingFieldName(string propertyName) =>
    string.Format("<{0}>k__BackingField", propertyName);
```

Using a general-purpose copy method:

```csharp
var oldState = new AccountState("EUR", AccountStatus.Active);
var newState = oldState.With(a => a.Status, AccountStatus.Frozen);

oldState.Status         // => AccountStatus.Active
newState.Status         // => AccountStatus.Frozen
newState.Currency       // => "EUR"
```

PROS:

* Bitwise copy of the original object. For any field and any type.

CONS:

* Reflection is relatively slow.
* Lose the fine-grained control (can't choose what fields can be updated in `With`).

### 9.3.3 Leveraging (использование) F# for data types

Idea: write your program behavior in C# and define your data objects in F#.

Writing the domain model in F#:

```fsharp
namespace Boc.Domain
open System             // Equivalent to a using statement in C#

// Use a "discriminated union" instead of enum.
type AccountStatus =
    Requested | Active | Frozen | Dormant | Closed

// A "type alias"
type CurrencyCode = string

// Use "record types" instead of classes.
type Transaction = {
    Amount: decimal
    Description: string
    Date: DateTime
}

type AccountState = {
    Status: AccountStatus
    Currency: CurrencyCode
    AllowedOverdraft: decimal
    TransactionHistory: Transaction list
}
```

PROS:

* All types are immutable by default (for mutable we must using `CLIMutable` attribute).
* Copy methods built in.

CONS:

* F# objects need to go into their own assembly.

Using F# copy and update expressions:

```fsharp
// with introduces a list of members (you can think of these as extension methods).
type AccountState with

// Creates a shallow copy with the updated value for Status.
// Copy and update expressions (using the with keyword).
member this.WithStatus(status) =
    { this with Status = Active }

// Prepends the given transaction to the list
member this.Add(transaction) =
    { this with TransactionHistory =
        transaction :: this.TransactionHistory }
```

### 9.3.4 Comparing strategies for immutability: an ugly contest
