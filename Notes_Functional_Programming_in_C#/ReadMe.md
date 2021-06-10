# Functional Programming in C#

## Chapter 1

### Introducing functional programming

#### REPL

A `REPL` is a command-line interface allowing you to experiment with the language by typing in
statements and getting immediate feedback.

If you use Visual Studio, you can start the REPL by going to

```text
View > Other Windows > C# Interactive
```

On Mono, you can use the `csharp` command.

### 1.1 What is this thing called functional programming?

* Functions as first-class values

* Avoiding state mutation

#### 1.1.1 Functions as first-class values

In a language where functions are first-class values, you can:

* use them as inputs or outputs of other functions

* assign them to variables

* store them in collections.

In other words, you can do with functions all the operations that you can do with
values of any other type.

Example:

```csharp
Func<int, int> triple = x => x * 3;
var range = Enumerable.Range(1, 3);
var triples = range.Select(triple);

triples // => [3, 6, 9]
```

#### 1.1.2 Avoiding state mutation

Once created, an object never changes, and variables should never be reassigned.

The term **mutation** indicates that a value is changed in-place - updating a value
stored somewhere in memory.

Mutation are also called **destructive updates**, because the value stored prior
to the update is destroyed.

**Functional** approach: `Where` and `OrderBy` don't affect the original list:

```csharp
Func<int, bool> isOdd = x => x % 2 == 1;
int[] original = { 7, 6, 1 };
var sorted = original.OrderBy(x => x);
var filtered = original.Where(isOdd);

original    // => [7, 6, 1]
sorted      // => [1, 6, 7]
filtered    // => [7, 1]
```

**Nonfunctional** approach: `List<T>.Sort` sorts the list in place:

```csharp
var original = new List<int> { 5, 7, 1 };
original.Sort();

original    // => [1, 5, 7]
```

#### 1.1.3 Writing programs with strong guarantees

Mutating state from concurrent processes yields unpredictable results
(`task2` is reordering that very same list):

```csharp
// This allows you to call Range and WriteLine without full qualification.
using static System.Linq.Enumerable;
using static System.Console;

var nums = Range(-10000, 20001).Reverse().ToList();
// => [10000, 9999, ... , -9999, -10000]

Action task1 = () => WriteLine(nums.Sum());
Action task2 = () => { nums.Sort(); WriteLine(nums.Sum()); };

// Executes both tasks in parallel
Parallel.Invoke(task1, task2);
// prints: 92332970
// 0
```

LINQ's functional implementation gives you a predictable result,
even when you execute the tasks in parallel.

Use LINQ's `OrderBy` method, instead of sorting the list in place
(`task3` isn't modifying the original list but rather creating a completely new sorted data):

```csharp
Action task3 = () => WriteLine(nums.OrderBy(x => x).Sum());

Parallel.Invoke(task1, task3);
// prints: 0
// 0
```

### 1.2 How functional a language is C#?

* Functions as first-class values

* The language should also *discourage* in-place updates.

* Fields and variables must explicitly be marked `readonly` to prevent mutation.

* The language must have garbage collection.

* Language must have immutable types.

#### 1.2.1 The functional nature of LINQ

Example:

```csharp
Enumerable
    .Range(1, 100)
    .Where(i => i % 20 == 0)
    .OrderBy(i => -i)
    .Select(i => $"{i}%")
// => ["100%", "80%", "60%", "40%", "20%"]
```

**Common operations on sequences**

* `Mapping`

  Given a sequence and a function, mapping yields a new sequence
  with the elements obtained by applying the given function to each element in
  the given sequence (in LINQ, this is done with the `Select` method).

  ```csharp
  Enumerable.Range(1, 3).Select(i => i * 3)    // => [3, 6, 9]
  ```

* `Filtering`

  Given a sequence and a predicate, filtering yields a new sequence
  consisting of the elements from the given sequence that pass the predicate
  (in LINQ, `Where`).

  ```csharp
  Enumerable.Range(1, 10).Where(i => i % 3 == 0) // => [3, 6, 9]
  ```

* `Sorting`

  Given a sequence and a key-selector function, sorting yields a new
  sequence ordered according to the key (in LINQ, `OrderBy` and `OrderByDescending`).

  ```csharp
  Enumerable.Range(1, 5).OrderBy(i => -i) // => [5, 4, 3, 2, 1]
  ```

#### 1.2.2 Functional features in C# 6 and C# 7

* Importing static members with using static.

* Easier immutable types with getter-only auto-properties.

* More concise (краткий) functions with expression-bodied members.

* Local functions.

* Better syntax for tuples.

Example:

```csharp
// Using static enables unqualified access to the static members of
// System.Math , like PI and Pow.
using static System.Math;

public class Circle
{
    // Expression-bodied constructor.
    public Circle(double radius) =>
        Radius = radius

    // A getter-only auto-property can be set only in the constructor.
    public double Radius { get; 

    // An expression-bodied property.
    public double Circumference =>
        PI * 2 * Radius

    public double Area
    {
        get
        {
            // A local function is a method declared within another method.
            double Square(double d) => Pow(d, 2);
            return PI * Square(Radius);
        }
    
    // C# 7 tuple syntax with named elements.
    public (double Circumference, double Area) Stats =>
        (Circumference, Area);
}
```

### 1.3 Thinking in functions

In mathematics, a *function* is a map between two sets, respectively called the *domain*
and *codomain*.

Function yields (выход) is determined *exclusively* by its input.

#### 1.3.2 Representing functions in C#

* Methods

* Delegates

* Lambda expressions

* Dictionaries

**Methods**

Methods can represent functions, but they also fit into the
object-oriented paradigm -they can be used to implement interfaces, they can be
overloaded, and so on.

**Delegates**

*Delegates* are type-safe function pointers.

Creating a delegate is a two-step process (This is analogous to writing an interface and then
instantiating a class implementing that interface):

1. you first declare the delegate type.

2. then provide an implementation.

```csharp
// Declaring a delegate (определен в namespace System)
namespace System
{
    public delegate int Comparison<in T>(T x, T y);
}


// Instantiating and using a delegate
var list = Enumerable.Range(1, 10).Select(i => i * 3).ToList();
list    // [3, 6, 9, 12, 15, 18, 21, 24, 27, 30]

// Provides an implementation of Comparison
Comparison<int> alphabetically =
    (l, r) => l.ToString().CompareTo(r.ToString());

// Uses the Comparison delegate as an argument to Sort
list.Sort(alphabetically);
list    // [12, 15, 18, 21, 24, 27, 3, 30, 6, 9]
```

```csharp
// (Примечание: в REPL для делегата надо указывать пространство имен напрямую)
System.Comparison<int> alphabetically = (l, r) =>
    l.ToString().CompareTo(r.ToString());
```

**The Func and Action delegates**

Delegates that returns the value:

* `Func<R>` represents a function that takes no arguments and returns a result of type `R` .

* `Func<T1, R>` represents a function that takes an argument of type `T1` and
returns a result of type `R`.

* `Func<T1, T2, R>` represents a function that takes a `T1` and a `T2` and returns an `R`.

* And so on.

* `Predicate<T>` (эквивалент `Func<T, bool>`) - рекомендуется использовать `Func<T, bool>`.

Delegates that have no return value, such as `void` methods:

* `Action` represents an action with no input arguments.

* `Action<T1>` represents an action with an input argument of type `T1`.

* `Action<T1, T2>` and so on represent an action with several input arguments.

* And so on.

**Lambda Expressions** (lambdas)

Lambda expressions, called *lambdas* for short, are used to declare a function inline.

If your function is short and you don't need to reuse it elsewhere, lambdas offer the
most attractive notation.

```csharp
var list = Enumerable.Range(1, 10).Select(i => i * 3).ToList();
list    // [3, 6, 9, 12, 15, 18, 21, 24, 27, 30]

list.Sort((l, r) => l.ToString().CompareTo(r.ToString()));
list    // [12, 15, 18, 21, 24, 27, 3, 30, 6, 9]
```

A *closure* is the combination of the lambda expression itself along with the context
in which that lambda is declared (that is, all the variables available in the scope
where the lambda appears):

```csharp
var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>();
// => [Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday]

// The days variable is referenced from within the lambda
// and is therefore captured in a closure.
IEnumerable<DayOfWeek> daysStartingWith(string pattern) =>
    days.Where(d => d.ToString().StartsWith(pattern));

daysStartingWith("S") // => [Sunday, Saturday]
```

**Dictionaries**

*Dictionaries* are fittingly also called *maps* (or *hashtables*); they're data structures that
provide a very direct representation of a function.

They literally contain the association of *keys* (elements from the domain)
to *values* (the corresponding elements from the codomain).

A function can be exhaustively (полностью) represented with a dictionary:

```csharp
var frenchFor = new Dictionary<bool, string>
{
    [true] = "Vrai",
    [false] = "Faux",
};

frenchFor[true]
// "Vrai"
```

Использование словаря позволяет организовать кэширование значений каких-либо вычислений.

### 1.4 Higher-order functions (HOFs)

*HOF*s are functions that take other functions as inputs or return a function as output, or both.

#### 1.4.1 Functions that depend on other functions (функция на входе)

HOFs that take a function as input (often referred to as a *callback* or a *continuation*)
and use it to perform a task or to compute a value. И еще, такой прием иногда
называют *inversion of control*.

Example. `Where` - a typical HOF that iteratively applies the given predicate
(пример функционально правильный, но не содержит оптимизаций, проверок и пр. из LINQ):

```csharp
public static IEnumerable<T> Where<T>(this IEnumerable<T> ts, Func<T, bool> predicate)
{
    // The task of iterating over the list is animplementation detail of Where
    foreach (T t in ts)
        // The criterion determining which items are included is decided by the caller.
        if (predicate(t))
            yield return t;
}
```

`Where` and `Sort` are examples of *iterated* applications - HOF will apply the given function
repeatedly for every element in the collection.

A HOF that *iteratively* applies the function given as an argument:

```csharp
IterativelyApply(f, ...)
{
    for (...)
        f(...)      // <= f
}
```

A HOF that *conditionally* applies the function given as an argument:

```csharp
ConditionallyApply(f, ...)
{
    if (...)
        f(...)      // <= f
}
```

A HOF that *optionally* invokes the given function:

```csharp
class Cache<T> where T : class
{
    public T Get(Guid id) => // ...

    public T Get(Guid id, Func<T> onMiss) =>
        Get(id) ?? onMiss();
}
```

#### 1.4.2 Adapter functions (функция на выходе)

Some HOFs return a new function.

С помощью подобных функций можно переделать interface другой функции - автор называет такие
функции - *adapter functions*.

Пример. Есть такая функция:

```csharp
Func<int, int, int> divide = (x, y) => x / y;
divide(10, 2)       // 5
```

HOF that modifies any binary function by swapping the order of its arguments:

```csharp
static Func<T2, T1, R> SwapArgs<T1, T2, R>(this Func<T1, T2, R> f) =>
    (t2, t1) => f(t1, t2);
```

Меняем порядок аргументов:

```csharp
var divideBy = divide.SwapArgs();
divideBy(2, 10)     // 5
```

#### 1.4.3 Functions that create other functions

*Function factories* - функции, чья первоочередная задача создавать другие функции.

Пример:

```csharp
Func<int, bool> isMod(int n) => i % n == 0;
```

Использование:

```csharp
using static System.Linq.Enumerable;

Range(1, 20).Where(isMod(2))    // [2, 4, 6, 8, 10, 12, 14, 16, 18, 20]
Range(1, 20).Where(isMod(3))    // [3, 6, 9, 12, 15, 18]
```

### 1.5 Using HOFs to avoid duplication

Another common use case for HOF s is to encapsulate setup and teardown operations.

Example. Duplication of setup/teardown logic:

```csharp
// Exposes Execute and Query as extension methods on the connection
// - Query queries the database and returns the deserialized LogMessages.
// - Execute runs the stored procedure and returns the number of affected rows
using Dapper;

public class DbLogger
{
    string connString;      // Assume this is set in the constructor.

    /// Inserts a given log message into the database.
    public void Log(LogMessage msg)
    {
        using (var conn = new SqlConnection(connString))        // Setup
        {
            int affectRows = conn.Execute(
                "sp_create_log", msg, commandType: CommandType.StoredProcedure);
        }   // Teardown is performed as part of Dispose.
    }

    /// Retrieves all logs since a given date from the database.
    public IEnumerable<LogMessage> GetLogs(DateTime since)      // Setup
    {
        var sqlGetLogs = "SELECT * FROM [Logs] WHERE [Timestamp] > @since";
        using (var conn = new SqlConnection(connString))
        {
            return conn.Query<LogMessage>(sqlGetLogs, new {since = since});
        }   // Teardown
    }
}
```

**Recommended** always perform I/O operations asynchronously:
`GetLogs` should really call `QueryAsync` and return a `Task<IEnumerable<LogMessage>>`.

#### 1.5.1 Encapsulating setup and teardown into a HOF

Example of HOF

```text
SetupTeardown(f, ...)
{
    Setup()
        f(...)
    Teardown()
}
```

```csharp
public static class ConnectionHelper
{
    public static R Connect<R>(string connString, Func<IDbConnection, R> f)
    {
        using (var conn = new SqlConnection(connString))    // Setup
        {                                                   // Setup
            conn.Open();                                    // Setup
            return f(conn);
        }                                                   // Teardown
    }
}
```

Using Connect:

```csharp
public class DbLogger
{
    string connString;

    public void Log(LogMessage message) =>
        Connect(            // generic type R is int
            connString,
            c => c.Execute(
                "sp_create_log",
                message,
                commandType: CommandType.StoredProcedure));

    public IEnumerable<LogMessage> GetLogs(DateTime since) =>
        Connect(            // generic type R is IEnumerable<LogMessage>
            connString,
            c => c.Query<LogMessage>(
                @"SELECT * FROM [Logs] WHERE [Timestamp] > @since",
                new {since = since}));
}
```

#### 1.5.2 Turning the using statement into a HOF

```csharp
namespace LaYumba.Functional
{
    public static class F
    {
        public static R Using<TDisp, R>(
            TDisp disposable, Func<TDisp, R> f) where TDisp : IDisposable
        {
            using (disposable)
                return f(disposable);
        }
    }
}
```

Using in the previous `Connect` function:

```csharp
using static LaYumba.Functional.F;

public static class ConnectionHelper
{
    public static R Connect<R>(string connStr, Func<IDbConnection, R> f) =>
        Using(
            new SqlConnection(connStr),
            conn => { conn.Open(); return f(conn); });
}
```

#### 1.5.3 Tradeoffs (компромиссы) of HOFs

Comparing the initial and the refactored versions of one of the methods in
`DbLogger`: 

Initial version:

```csharp
public void Log(LogMessage msg)
{
    using (var conn = new SqlConnection(connString))
    {
        int affectedRows = conn.Execute(
            "sp_create_log", msg, commandType: CommandType.StoredProcedure);
    }
}
```

Refactored version (после использования HOF):

```csharp
public void Log(LogMessage msg) =>
    Connect(
        connString,
        c => c.Execute(
            "sp_create_log", msg, commandType: CommandType.StoredProcedure));
```

**Преимущества** использования HOF:

1. Conciseness (выразительность, лаконичность)

2. Avoid duplication - setup/teardown logic is in a single place.

3. Separation of concerns (разделение ответственности) - `ConnectionHelper` управляет соединением,
`DBLogger` содержит только логику логирования.

**Недостатки** использования HOF:

1. You’ve increased stack use. There’s a performance impact, but it’s negligible.

2. Debugging the application will be a bit more complex because of the callbacks.

**Рекомендация** по использованию HOF:

1. Use short lambdas

2. Clear naming

3. Meaningful indentation (содержательные/понятные отступы).

### 1.6 Benefits of functional programming

* *Cleaner code* - more expressive, more readable, and more easily testable code.

* *Better support for concurrency*

* *A multi-paradigm approach*

## Chapter 2

### Why function purity matters

### 2.1 What is function purity?

#### 2.1.1 Purity and side effects

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

#### 2.1.2 Strategies for managing side effects

#### Isolate I/O effects

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

#### Avoid mutating arguments

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

#### Mutating non-local state. Throwing exceptions.

* It's *always* possible to handle errors without relying on exceptions.

* It's *often* possible to avoid state mutation. 

### 2.2 Purity and concurrency

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

#### 2.2.1 Pure functions parallelize well

Pure функция отлично параллелится.

Следующие два вызова показывают одинаковые результаты:

```csharp
list.Select(ToSentenceCase).ToList()
list.AsParallel().Select(ToSentenceCase).ToList()    // Using Parallel LINQ (PLINQ)
```

#### Concurrency (о параллелелизме)

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

#### 2.2.2 Parallelizing impure functions

Impure функция параллелится плохо:

```csharp
list.Select(PrependCounter).ToList()
list.AsParallel().Select(PrependCounter).ToList()    // Будут ошибки
```

Impure функции не параллелятся просто так - их надо дорабатывать
(использовать Interlocked и т.п.).

#### 2.2.3 Avoiding state mutation

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

#### Когда создавать статические методы

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

## Links

### Libraries from Microsoft to programming in a functional style:

* `System.Linq`

* `System.Collections.Immutable` - This is a library of immutable collections.

* `System.Reactive` - This is an implementation of the Reactive Extensions for .NET.

### Unsorted:

* `LanguageExt` https://github.com/louthy/language-ext

  A library written by Paul Louth to improve the C# developer’s experience
  when coding functionally.

* Examples and library `LaYumba` from book https://github.com/la-yumba/functional-csharp-code

* Dapper library: https://github.com/StackExchange/dapper-dot-net