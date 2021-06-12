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

## 2.3 Purity and testability

### 2.3.1 In practice: a validation scenario

Пример. Этому классу требуется Validation:

```csharp
public abstract class Command { }

public sealed class MakeTransfer : Command
{
    public string Bic { get; set; }       // Should be valid (some format)
    public DateTime Date { get; set; }    // Should not be past
    ...
}
```

Simple interface for all these validator classes:

```csharp
public interface IValidator<T>
{
    bool IsValid(T t);
}
```

Basic implementation:

```csharp
using System.Text.RegularExpressions;

// Pure - no side effects and the result of IsValid is deterministic.
public sealed class BicFormatValidator : IValidator<MakeTransfer>
{
    static readonly Regex regex = new Regex("^[A-Z]{6}[A-Z1-9]{5}$");

    public bool IsValid(MakeTransfer cmd) =>
        regex.IsMatch(cmd.Bic);
}

// Impure - the result of IsValid will depend on the current date.
// I/O side effect: DateTime.UtcNow queries the system clock.
public class DateNotPastValidator : IValidator<MakeTransfer>
{
    public bool IsValid(MakeTransfer cmd) =>
        (DateTime.UtcNow.Date <= cmd.Date.Date);
}
```

Functions that perform I/O are difficult to test.

### 2.3.2 Bringing impure functions under test

*Interface-based approach* - abstract I/O operations in an interface, and to use a deterministic implementation in the tests.

For our example: abstract access to the system clock:

```csharp
// Encapsulates the impure behavior in an interface
public interface IDateTimeService
{
    DateTime UtcNow { get; }
}

// Provides a default implementation
public class DefaultDateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
```

Refactor the date validator to consume this interface:

```csharp
public class DateNotPastValidator : IValidator<MakeTransfer>
{
    private readonly IDateTimeService clock;

    // Interface is injected in the constructor.
    public DateNotPastValidator(IDateTimeService clock)
    {
        this.clock = clock;
    }

    // Validation now depends on the interface.
    public bool IsValid(MakeTransfer request) =>
        clock.UtcNow.Date <= request.Date.Date;
}
```

* When running normally, you'll compose your objects so that you get the "real"
impure implementation that checks the system clock.

* When running unit tests, you'll inject a "fake" pure implementation that does
something predictable, such as always returning the same DateTime, enabling
you to write tests that are repeatable.

#### Типичные шаги при использовании dependency injection и mocks.

1) Определение интерфейса (`IDateTimeService`), который будет абстрацией для impure операции,
которая выполнется в тестируемом классе.

2) Поместить impure логику (`DateTime.UtcNow`) в класс-имплементацию этого интерфейса
(`DefaultDateTimeService`).

3) В тестируемом классе, запросить интерфейс в конструкторе, сохранить его в поле и использовать
когда это будет необходимо.

4) Добавить bootstrapping логику для правильной инициализации класса.

5) Создать и inject fake имплементацию интерфейса для unit-тестирования.

Tests can be written in this form:

```csharp
public class DateNotPastValidatorTest
{
    static DateTime presentDate = new DateTime(2016, 12, 12);

    // Provides a pure, fake implementation
    private class FakeDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => presentDate;
    }

    [Test]
    public void WhenTransferDateIsPast_ThenValidationFails()
    {
        // Injects the fake
        var sut = new DateNotPastValidator(new FakeDateTimeService());
        var cmd = new MakeTransfer { Date = presentDate.AddDays(-1) };
        Assert.AreEqual(false, sut.IsValid(cmd));
    }
}
```

Unit tests need to be:

* *Isolated* (no I/O) 
* *Repeatable* (always get the same result, given the same inputs).

These properties are guaranteed when you use pure functions.

### 2.3.3 Why testing impure functions is hard

Testing a *pure* function is easy. Ее выходы зависят только от входов:

```text
Arrange:                   Act:                       Assert:
set up input value(s)      evaluate the function      verify output is as expected
INPUTS                ---> UNIT UNDER TEST       ---> OUTPUTS
```

С другой стороны, когда тестируется *impure* функция, ее поведение возможно дополнительно
будет зависеть от:

* от состояния программы
* от состояния "снаружи" программы

Плюс, side effects функции могут привести к новому состоянию программы.

Примеры:

* The date validator depends on the state of the world, specifically the current time.

* A `void` - returning method that sends an email has no explicit output to assert
against, but it results in a new state of the world.

* A method that sets a non-local variable results in a new state of the program.

Unit testing from a functional perspective for *impure* function:

AAA pattern | Functional thinking                                                   |
------------|-----------------------------------------------------------------------|
Arrange     | Set up the (explicit and implicit) inputs to the function under test  |
Act         | Evaluate the function under test                                      |
Assert      | Verify the correctness of the (explicit and implicit) outputs         |

Два подхода к тестированию функций с side effects:

* The state of the world is managed by using mocks to create an artificial world in
which the test runs. To test I/O operations.

* Setting the state of the program and checking that it's updated correctly
doesn't require mocks, but it makes for brittle (хрупкие) tests and breaks encapsulation.

### 2.3.4 Parameterized unit tests

Параметризированные тесты более функциональны, т.к. заставляют думать о них в терминах
входных и выходных значений.

Преимущества: тестирование нескольких сценариев, используя один тестовый метод.

Пример:

```csharp
public class DateNotPastValidatorTest
{
    static DateTime presentDate = new DateTime(2016, 12, 12);

    private class FakeDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => presentDate;
    }

    [TestCase(+1, ExpectedResult = true)]       // NUnit
    [TestCase( 0, ExpectedResult = true)]
    [TestCase(-1, ExpectedResult = false)]
    public bool WhenTransferDateIsPast_ThenValidatorFails(int offset)
    {
        var sut = new DateNotPastValidator(new FakeDateTimeService());
        var cmd = new MakeTransfer { Date = presentDate.AddDays(offset) };
        return sut.IsValid(cmd);
    }
}
```

### 2.3.5 Avoiding header interfaces

Современные приложения используют множество интерфейсов для каждой операции
(у которых по одной реализации), которые задействует I/O.

Такие интерфейсы называются "header interfaces" - они отличаются от первоначальной идеи для
чего задумывались интерфейсы (несколько реализаций для одного контракта).

Результат - множество файлов, в которых сложно ориентироваться.

### Решение 1. Pushing the pure boundary outwards

Весь код сделать pure невозможно, но можно раздвинуть границы pure кода.

Injecting a specific value, rather than an interface, makes IsValid pure.
Переписанный date validator:

```csharp
public class DateNotPastValidator : IValidator<MakeTransfer>
{
    private readonly DateTime today;

    public DateNotPastValidator(DateTime today)
    {
        this.today = today;
    }

    public bool IsValid(MakeTransfer cmd)       // Pure
        => (today <= cmd.Date.Date);
}
```

Теперь:

* Code instantiates `DateNotPastValidator` must know how to get the current time.

* `DateNotPastValidator` must be short-lived.

### Решение 2. Injecting functions as dependencies

Пример, когда предыдущий вариант не подходит:

```csharp
public sealed class BicExistsValidator : IValidator<MakeTransfer>
{
    readonly IEnumerable<string> validCodes;    // Конструктор с установкой поля пропущен.

    public bool IsValid(MakeTransfer cmd) =>
        validCodes.Contains(cmd.Bic);
}
```

Здесь проверка идентификатора банка путем его сравнения с заранее известными идентификаторами
банков.

Но:

1) Список банков надо обновлять, а это impure операция.

2) Клиенский код зависит от валидатора, но не отвечает за его работу и актуальность
идентификаторов.

3) Инициализирующий код не знает, будет ли валидатор использоваться и когда.

Решение. Вместо интерфейса использовать делегат:

```csharp
public sealed class BicExistsValidator : IValidator<MakeTransfer>
{
    readonly Func<IEnumerable<string>> getValidCodes;

    public BicExistsValidator(Func<IEnumerable<string>> getValidCodes)
    {
        this.getValidCodes = getValidCodes;
    }

    public bool IsValid(MakeTransfer cmd) =>
        getValidCodes().Contains(cmd.Bic);
}
```

Unit тесты в таком случае будут выглядеть таким образом:

```csharp
public class BicExistsValidatorTest
{
    static string[] validCodes = { "ABCDEFGJ123" };

    [TestCase("ABCDEFGJ123", ExpectedResult = true)]
    [TestCase("XXXXXXXXXXX", ExpectedResult = false)]
    public bool WhenBicNotFound_ThenValidationFails(string bic) =>
        new BicExistsValidator(() => validCodes)
            .IsValid(new MakeTransfer { Bic = bic });
}
```

## 2.4 Purity and the evolution of computing

Тенденции:

* Увеличение значения и использования I/O (сетевые сервисы).

* Increased requirements for asynchronous I/O.

* Parallelization is becoming the main road to computing speed (развитие CPU идет в сторону
параллелизации).
