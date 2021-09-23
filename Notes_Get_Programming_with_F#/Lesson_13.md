# Lesson 13. Achieving code reuse in F#

## Пример переиспользования кода в C#

Есть коллекция `Customers`, которую надо отфильтровать. Есть разные фильтры.
Одна из реализаций на C#:

```csharp
// === Using interfaces as a way of passing code
// Filter interface represents a contract used by Where()
interface IFilter
{
    bool IsValid(Customer customer);
}

// Where receives an instance of Filter to allow varying the algorithm
IEnumerable<Customer> Where(this IEnumerable<Customer> customers, IFilter filter)
{
    foreach (var customer in customers)
    {
        if (filter.IsValid(customer))
            yield return customer;
    }
}

// === Использование. Consuming an interface-based design
// An instance of an IFilter
public class IsOver35Filter : IFilter
{
    public Boolean IsValid(Customer customer)
    {
        return customer.Age > 35;
    }
}

public void FilterOlderCustomers()
{
    var customers = new Customer[0];
    // Creating an instance of the IsOver35Filter class
    var filter = new IsOver35Filter();
    // Supplying the filter to the Where method
    var olderCustomers = customers.Where(filter);
}
```

## Reuse in the world of LINQ

LINQ появился в C# начиная с версии 3. LINQ широко использует higher-order functions (HOFs).
*Higher-order function* is a function that takes in another function as one of its arguments.

C# 3 also introduced the concept of lambda expressions. *Lambda* is just a way of declaring a
function inline of a method.

Пример использования higher-order functions to reuse code:

```csharp
// (1) - Using Func<Customer, bool> as a means of a contract instead of an interface
// (2) - Calling the filter on the customer directly
Enumerable<Customer> Where(
    this IEnumerable<Customer> customers, Func<Customer, bool> filter)      // (1)
{
    foreach (var customer in Customers)
    {
        if (filter(customer))           // (2)
            yield return customer;
    }
}
```

Этот метод похож на предыдущий, но вместо интерфейса использует функцию.

Consuming a higher-order function:

```csharp
// Creating a function of signature Customer->bool to check a customer age
public Boolean IsOver35(Customer customer)
{
    return customer.Age > 35;
}
// ...код пропущен...

// Providing the IsOver35 function to the Where higher-order function
var olderCustomers = customers.Where(IsOver35);
// Reimplementing IsOver35 as an inline lambda expression
var olderCustomersLambda = customers.Where(customer => customer.Age > 35);
```

## Comparing OO and FP mechanisms for reuse

|-                             | Object-oriented                | Functional            |
|------------------------------|--------------------------------|-----------------------|
| Contract specification       | Interface (nominal)            | Function (structural) |
| Common patterns              | Strategy/command               | Higher-order function |
| Verbosity (многословность)   | Medium/heavy                   | Lightweight           |
| Composability and reuse      | Medium                         | High                  |
| Dimensionality (размерность) | Multiple methods per interface | Single functions      |

>### Delegates and anonymous methods
>C# has always supported the notion of typesafe function pointers:
>* C# 1 - delegates
>* C# 2 - anonymous methods
>
>Эти способы описания функций устарели после введения `Func<T>` and lambda expressions.

## Higher-order function (HOF) in F#. Dependency Injection (DI)

В C# и BCL существует мешанина из стратегий использования interface и high-order function.
В F# встроенные библиотеки почти все используют higher-order functions.

* Dependencies in F# tend to be functions; in C#, they're interfaces.

```fsharp
type Customer = { Age : int }

// val where: filter:('a -> bool) -> customers:seq<'a>
let where filter customers =          // filer acts like a dependency injection
    seq {
        for customer in customers do
            if filter customer then   // Calling the filter function with customer as an argument
                yield customer }

let customers = [ { Age = 21 }; { Age = 35 }; { Age = 36 } ]
let isOver35 customer = customer.Age > 35    // filter

// Supplying the isOver35 function into the where function
customers |> where isOver35
// Passing a function inline using lambda syntax
customers |> where (fun customer -> customer.Age > 35)
```

* `seq { }` - This is a type of *computation expression* (вычислительное выражение).
Generate a sequence of customers by using the `yield` keyword.

* `[ ; ; ; ] syntax` - F# list.

### When to pass functions as arguments

В F# передача функции в качестве аргумента является основным способом обеспечения
повторного переиспользования кода.

У F# есть поддержка interfaces, но они как правило не используются. Исключением являются
зависимости, логически сгруппированные по поведению (например, функции логирования или что-то
подобное).

A hardcoded function that can be converted into a HOF:

```fsharp
let whereCustomerAreOver35 customers =
    seq {
        for customer in customers do
            if customer.Age > 35 then    // customer.Age > 35 можно выделить в HOF
                yield customer }
```

## Dependencies as functions

Очень часто для передачи зависимостей используются интерфейсы. Часто предпочтительнее явно
передавать зависимости как функции, а не один более крупный интерфейс, содержащий десятки
методов, из которых вам понадобится только один или два. Становится намного легче понять
взаимосвязь между функцией и ее зависимостями.

```fsharp
// Specifying your dependency as the writer argument
let printCustomerAge writer customer =
    if customer.Age < 13 then writer "Child!"         // Calling writer 
    elif customer.Age < 20 then writer "Teenager!"    // with a string argument
    else writer "Adult!"

// === Partially applying a function with dependencies
// 1. Calling printCustomerAge with Console.WriteLine as a dependency
printCustomerAge Console.WriteLine { Age = 21 }

// 2. Partially applying printCustomerAge to create a constrained version of it
let printToConsole = printCustomerAge Console.WriteLine
printToConsole { Age = 21 }         // Calling

// === Creating a dependency to write to a file
// Creating a File System writer that's compatible with printCustomerAge
open System.IO
let writeToFile text = File.WriteAllText(@"C:\temp\output.txt", text)

let printToFile = printCustomerAge writeToFile
printToFile { Age = 21 }
```
