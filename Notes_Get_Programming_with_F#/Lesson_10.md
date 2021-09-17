# Lesson 10. Shaping data with records

## DTO in C#

Plain Old C# Object (*POCO*) or data transfer object (*DTO*) - a class that's used for the
purposes of storing and transferring data, but not necessarily any behavior.

A basic DTO in C#:

```csharp
public class Customer {                     // Type definition
    public string Forename { get; set; }    // Public, mutable properties
    public string Surname { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }        // Address - another DTO
    public string EmailAddress { get; set; }
}
```

* *-* There is no way to guarantee that you’ll always create a valid object.
  * You might forget to set some property.
* *-* You can also modify this after the object is created.

Улучшение предыдущего DTO. Near-immutable DTOs in C#:

```csharp

public class Customer {
    // Public read-only, private mutable properties
    public string Forename { get; private set; }
    public string Surname { get; private set; }
    public int Age { get; private set; }
    public Address Address { get; private set; }
    public string EmailAddress { get; private set; }

    // Nondefault constructor guarantees safe initialization of object
    public Customer(
        string forename, string surname, int age, Address address, string mailAddress)
    {
        Forename = forename;
        Surname = surname;
        Age = age;
        Address = address;
        EmailAddress = emailAddress;
    }
}
```

* *+* Более надежная реализация по сравнению с предыдущей версией DTO.
* *-* There's a lot of boilerplate here.
* *-* The class itself could change its own state later in, for example, a method.

Плюс, основной недостаток - reference equality checks:

### Reference equality in C#

```csharp
// Example Address type
public class Address {
    public string Street { get; set; }
    public string Town { get; set; }
    public string City { get; set; }
}

// Comparing two address objects - почти всегда будет false
var sameAddress = (customerA.Address == customer.Address);
```

.NET классы perform reference equality checks by default. Only if both addresses are
*the same object*, existing in *the same space in memory*, will this check return `true`.

You're looking for is a form of *structural equality* checking. Чтобы это сделать надо:

* Override `GetHashCode()`.
* Override `Equals()`.
* Write a custom `==` operator (otherwise, `Equals` and `==` will give different behavior!).
* Ideally, implement `System.IEquatable`.
* Ideally, implement `System.Collections.Generic.IEqualityComparer`.

## Records in F#

F# *records* are best described as simple-to-use objects designed to store, transfer, and
access immutable data that have named fields - essentially (по существу) the same thing
you just tried to achieve with a C# POCO.

### Record basics

```fsharp
type Address = {
    Street : string
    Town : string
    City : string }
```

Здесь есть все что надо:

* F# records are immutable by default.
* A constructor that requires all fields to be provided.
* Public access for all fields (which are themselves read-only).
* Full structural equality, throughout the *entire object graph*.

Можно declaring records on a single line:

```fsharp
 type Address = { Line1 : string; Line2 : string }
```

### Creating records

Constructing a nested record in F#:

```fsharp
// Declaring the Customer record type
type Customer =
    { Forename : string
      Surname : string
      Age : int
      Address : Address
      EmailAddress : string }

// Creating a Customer with Address inline (встроенный)
let customer =
    { Forename = "Joe"
      Surname = "Bloggs"
      Age = 30
      Address =
        { Street = "The Street"
          Town = "The Town"
          City = "The City" }
          EmailAddress = "joe@bloggs.com" }
```

* You could've defined the address separately if you wanted to, as a separate `let` binding.
* You can access fields on the record just like "normal" C# objects.
* Constructor of record requires all properties. You can't miss any of the fields when declaring an instance of a record.

### Providing explicit types for constructing records

```fsharp
// Explicitly declaring the type of the address value
let address : Address =
    { Street = "The Street"
      Town = "The Town"
      City = "The City" }
// Explicitly declaring the type that the Street field belongs to
let addressExplicit =
    { Address.Street = "The Street"
      Town = "The Town"
      City = "The City" }
```

Автор советует избегать явного задания типа, за исключением когда это действительно необходимо.
Но есть одно преимущество от явного задания типа - компилятор немедленно включит IntelliSense.

Records have the standard members (for example, `ToString()` and `GetHashCode()`).
That's because records compile down to classes. And, as with classes, you
can create member methods on them (но это не особо должно использоваться в ФП).

### Copy-and-update record syntax

```fsharp
// Creating a new version of a record by using the 'with' keyword
let updatedCustomer =
    { customer with
        Age = 31
        EmailAddress = "joe@bloggs.co.uk" }
```

*+* You can provide records to other sections of code without having to worry about
their values being implicitly modified without your knowledge.
*+* You can still easily simulate mutation through copy-and-update.

1. If you want to write a function that does modify a record, you have it take in the
original version as an argument and return the new version as the output of the
function.

2. You can override immutability behavior on a field-by-field basis by adding the `mutable`
modifier.

### Record performance

Если record изменяется в цикле, сотни раз в секунду это может вызвать проблемы с быстродействием.
Because records are reference types every copy-and-update causes a new object to be allocated on
the heap, so garbage collector (GC) pressure could cause performance issues in this situation.

*Recommendation* - use immutable data structures initially, test performance, and only if you
see an issue, reconfigure the definition of the record.

### Equality checking

You can safely compare two F# records of the same type with a single `=` for full, deep
**structural** equality checking.

```fsharp
// Все поля address1 структурно равны полям address2
// Structure comparing two records by using the = operator
let isSameAddress = (address1 = address2)                               // true
// Structure comparing
let isSameAddress = address1.Equals address2                            // true
// Comparing by reference(!)
let isSameAddress = System.Object.ReferenceEquals(address1, address2)   // false
```

### Comparing classes and records

| -                          | .NET classes       | F# records            |
|----------------------------|--------------------|-----------------------|
| Default mutability of data | Mutable            | Immutable             |
| Default equality behavior  | Reference equality | Structural equality   |
| Copy-and-update syntax?    | No                 | Rich language support |
| F# type-inference support? | Limited            | Full                  |
| Guaranteed initialization  | No                 | Yes                   |

## When to use records

* Records - the most common type of data structure in F#.
* More powerful than tuples.
  * Explicitly name fields.
  * Using a neat copy-and-update syntax.
* Records compile down into classes - they can be used in .NET languages and systems expecting
classes.
