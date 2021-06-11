# Chapter 2

## Why function purity matters

## 2.1 What is function purity?

### 2.1.1 Purity and side effects

|Pure functions                                     | Impure functions                                        |
|---------------------------------------------------|---------------------------------------------------------|
|The output depends entirely on the input arguments.|Factors other than input arguments may affect the output.|
|Cause no side effects.                             |May cause side effects.                                  |

Function has **side effects** if:

* *Mutates global state* - "Global" here means any state that’s visible outside of the
function's scope. Example: private instance field of the class (visible from all methods).

* *Mutates its input arguments*

* *Throws exceptions*

* *Performs any I/O operation* 
  * any interaction between the program and the external world
  * reading from or writing to the console, the filesystem, or a database
  * interacting with any process outside the application's boundary.

**Pure** function:

* Easy to test 
  * They always return the same output for the same input.
  * Output is solely (исключительно) determined by their inputs.

* The order of evaluation isn't important:
  * *Parallelization* - Different threads carry out tasks in parallel.
  * *Lazy evaluation* - Only evaluate values as needed.
  * *Memoization* - Cache the result of a function so it’s only computed once.

**Recommended**: pure functions should be preferred whenever possible.

### 2.1.2 Strategies for managing side effects

### Isolate I/O effects

Functions that perform I/O can never be pure. What you can do is *isolate* the pure,
computational parts of your programs from the I/O.

Example:

```csharp
WriteLine("Enter your name:");
var name = ReadLine();
WriteLine($"Hello {name}");
```

Logic that could be extracted in a pure function:

```csharp
static string GreetingFor(string name) =>
    $"Hello {name}";
```

### Avoid mutating arguments

```csharp
decimal RecomputeTotal(Order order, List<OrderLine> linesToDelete)
{
    var result = 0m;
    foreach (var line in order.OrderLines)
        if (line.Quantity == 0) linesToDelete.Add(line);    // Mutating argument (BAD!)
        else result += line.Product.Price * line.Quantity;
    return result;
}
```

You can *always* structure your code in such a way that functions never mutate their input
arguments.

Решение: returning all the computed information to the caller instead.

Refactored version:

```csharp
(decimal, IEnumerable<OrderLine>) RecomputeTotal(Order order) =>
    (order.OrderLines.Sum(l => l.Product.Price * l.Quantity),
        order.OrderLines.Where(l => l.Quantity == 0));
```

### Mutating non-local state. Throwing exceptions.

* It's *always* possible to handle errors without relying on exceptions.

* It's *often* possible to avoid state mutation. 

## 2.2 Purity and concurrency

Example:

```csharp
// Usage of the list formatter
var shoppingList = new List<string> { "coffee beans", "BANANAS", "Dates" };
new ListFormatter()             // prints: 1. Coffee beans
    .Format(shoppingList)       //         2. Bananas
    .ForEach(WriteLine);        //         3. Dates

// A list formatter combining pure and impure functions
static class StringExt
{
    public static string ToSentenceCase(this string s) =>   // A pure function
        s.ToUpper()[0] + s.ToLower().Substring(1);
}

class ListFormatter
{
    int counter;

    string PrependCounter(string s) =>    // An impure function 
        $"{++counter}. {s}";              //(it mutates global state - counter)

    public List<string> Format(List<string> list) =>
        list
        .Select(StringExt.ToSentenceCase)    // Pure and impure functions
        .Select(PrependCounter)              // can be applied similarly.
        .ToList();
}
```

### 2.2.1 Pure functions parallelize well

Pure функция отлично параллелится.

Следующие два вызова показывают одинаковые результаты:

```csharp
list.Select(ToSentenceCase).ToList()
list.AsParallel().Select(ToSentenceCase).ToList()    // Using Parallel LINQ (PLINQ)
```

### Concurrency (о параллелелизме)

**Concurrency** is when a program initiates a task before another
one has completed, so that different tasks are executed in overlapping time windows.

* *Asynchrony* - This means that your program performs non-blocking operations.

  Example: it can initiate a request for a remote resource via HTTPand then go on to do
  some other task while it waits for the response to be received.

  It's a bit like when you send an email and then go on with your life without waiting
  for a response.

* *Parallelism* - This means that your program leverages the hardware of multicore
machines to execute tasks at the same time by breaking up work into tasks,
each of which is executed on a separate core.

  It's a bit like singing in the shower: you're actually doing two things at exactly
  the same time.

* *Multithreading* - This is a software implementation allowing different threads to be
  executed concurrently. A multithreaded program appears to be doing several things at the
  same time even when it's running on a single-core machine.

  This is a bit like chatting with different people through various IM windows;
  although you're actually switching back and forth, the net result is that you're having
  multiple conversations at the same time.

Побочные и не очень приятные эффекты от concurrency:

* The order of execution isn't guaranteed.

* Concurrency can be the source of difficult problems,
most notably when multiple tasks concurrently try to update some shared mutable state.

### 2.2.2 Parallelizing impure functions

Impure функция параллелится плохо:

```csharp
list.Select(PrependCounter).ToList()
list.AsParallel().Select(PrependCounter).ToList()    // Будут ошибки
```

Impure функции не параллелятся просто так - их надо дорабатывать
(использовать Interlocked и т.п.).

### 2.2.3 Avoiding state mutation

Для переработки ListFormatter будет использоваться операции `Range` и `Zip`.

`Range`:

```csharp
Enumerable.Range(1, 3)      // => [1, 2, 3]
```

`Zip` - the operation of pairing two parallel lists:

```csharp
Enumerable.Zip(
    new[] {1, 2, 3},
    new[] {"ichi", "ni", "san"},
    (number, name) => $"In Japanese, {number} is: {name}")

// => ["In Japanese, 1 is: ichi",
//     "In Japanese, 2 is: ni",
//     "In Japanese, 3 is: san"
```

Refactored List formatter:

```csharp
using static System.Linq.Enumerable;
static class ListFormatter
{
    public static List<string> Format(List<string> list)
    {
        var left = list.Select(StringExt.ToSentenceCase);
        var right = Range(1, list.Count);
        var zipped = Zip(left, right, (s, i) => $"{i}. {s}");
        return zipped.ToList();
    }
}
```

`Zip` can be used as an extension method. Refactored (second version):

```csharp
using static System.Linq.ParallelEnumerable;  // Uses Range (from Parallel-Enumerable)
static class ListFormatter
{
    public static List<string> Format(List<string> list) =>
        list.AsParallel()    // Turns the original data source into a parallel query
        .Select(StringExt.ToSentenceCase)
        .Zip(Range(1, list.Count), (s, i) => $"{i}. {s}")
        .ToList();
}
```

### Когда создавать статические методы

Когда все переменные, требуемые для работы метода, provided as input (or are statically
available), метод может быть сделан статичным.

Статичные методы могут вызвать **проблемы** если:

* *Act on mutable static fields* (воздействуют на изменямые статичкие поля)

  These are effectively the most global variables, and it's well known
  that maintainability suffers from the presence of global mutable variables.

* *Perform I/O*

  In this case, it's testability that's jeopardized (страдает тестируемость).
  If method A depends on the I/O behavior of static method B,
  it's not possible to unit test A.

**General guideline** (общая рекомендация):

* Make pure functions static.

* Avoid mutable static fields.

* Avoid direct calls to static methods that perform I/O.