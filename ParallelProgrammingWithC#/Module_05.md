# Module 5. Parallel Loops

*Parallel versions of `for` and `foreach`*.

* Parallel Invoke/For/ForEach
  * Options, stepped for loop
* Stopping, cancellation and exceptions
* Thread Local Storage
* Partitioning

## Lesson 32. Parallel Invoke/For/ForEach

### Введение. Parallel Loops

* `Parallel.Xxx` are blocking calls
  * Wait until all threads completed *or* an exception occured
* Can check the state of the loop as it is executing in
`ParallelLoopState`
* Can check result of execution in `ParallelLoopResult`
* `ParallelLoopOptions` let us customize execution with
  * Max. degree of parallelism
  * Cancellation Token

### Введение. Parallel.For/ForEach

* `Parallel.For`
  * Uses as index [start; finish]
  * Cannot provide a step
    * Create an `IEnumerable<int>` and use `Parallel.ForEach`
  * Partitions data into different tasks
  * Executes provided delegate with counter value argument
    * Might be inefficient
* `Parallel.ForEach`
  * Like `Parallel.For()` but
  * Takes an `IEnumerable<T>` instead

### Введение. Parallel.Invoke

* Runs several provided functions concurrently
* Is equivalent to
  * Creating a task for each lambda
  * Doing a `Task.WaitAll()` on all the tasks

### Введение

Класс `Parallel` имеет три метода:

* `For`
* `ForEach`
* `Invoke`

Здесь не требуется ожидать (использовать `Wait()`) завершения выполнения параллельного запуска
делегатов.

Выполненение производится не по порядку.

### `For`

```csharp
Parallel.For(fromInclusive: 1, toExclusive: 11, i =>    // от 1 до 10
{
    Console.WriteLine($"{i * i}");
});
```

* `fromInclusive` - начальное число (включая его)
* `toExclusive` - конечное число (не включая его)

### `ForEach`

```csharp
var words = new[] { "oh", "what", "a", "night" };
Parallel.ForEach(words, word =>
{
    Console.WriteLine($"\"{word}\" has length {word.Length} (task {Task.CurrentId})");
});
```

Пример выполнения цикла, похожего на `Parallel.For`, но с заданием шага:

```csharp
private static IEnumerable<int> Range(int start, int end, int step)
{
    for (var i = start; i < end; i += step)
    {
        yield return i;
    }
}

public static void Execute()
{
    Parallel.ForEach(
        Range(1, 20, 3),
        num => Console.WriteLine($"{num} from task {Task.CurrentId}"));
}
```

Также для `ForEach` (возможно и для остальных видов loop'ов - в видео не было показано) можно
задать `ParallelOptions`:

```csharp
var po = new ParallelOptions();
po.CancellationToken = ...          // Задание CancellationToken
po.MaxDegreeOfParallelism = ...     // Максимальное количество одновременно запускаемых задач
po.TaskScheduler = ...              // Задание планировщика задач
```

### `Invoke`

```csharp
var a = new Action(() => Console.WriteLine($"First {Task.CurrentId}"));
var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId}"));

Parallel.Invoke(a, b, c);
```

## Lesson 33. Breaking, Cancellations and Exceptions

`ParallelLoopState` - объект, который позволяет досрочно завершить выполнение loop.

```csharp
Parallel.For(0, 20, (int x, ParallelLoopState state) =>
{
    // ...
});
```

Существует несколько способов остановки выполнения loop.

### `Stop`

`Stop()` - останавливает loop как можно скорее (т.к. некоторые задачи все еще продолжают
выполняться после вызова этого метода).

```csharp
Parallel.For(0, 20, (x, state) =>
{
    Console.WriteLine($"{x}. Task {Task.CurrentId}");
    if (x == 10)
        state.Stop();
});
```

### `Break`

`Break()`- сообщает, что цикл Parallel должен прекратить выполнение итераций после текущей
в первый удобный для системы момент. Более долго останавливает выполнение цикла, чем `Stop()`.

`Break()` дополнительно позволяет вывести информацию на какой итерации цикл был остановлен
(если это произошло).

```csharp
var result = Parallel.For(0, 20, (x, state) =>
{
    Console.WriteLine($"{x}. Task {Task.CurrentId}");
    if (x == 10)
        state.Break();
});

Console.WriteLine();
Console.WriteLine($"Was loop completed? {result.IsCompleted}");
if (result.LowestBreakIteration.HasValue)
    Console.WriteLine($"Lowest break iteration is {result.LowestBreakIteration}");
```

Выведет:

```text
Was loop completed? False
Lowest break iteration is 10
```

### Бросить исключение из параллельного цикла

Бросить исключение (любое). Оно пробрасывается вверх по стеку, что может вызвать аварийное завершение
работы программы (если не отловить).

```csharp
private static void ExecuteAndThrow()
{
    Parallel.For(0, 20, (x, state) =>
    {
        Console.WriteLine($"{x}. Task {Task.CurrentId}");
        if (x == 10)
            throw new Exception("Breaking the parallel loop");
    });
}

// Вызов параллельного цикла и обработка исключения.
static void Main(string[] args)
{
    try
    {
        ExecuteAndThrow();
    }
    catch (AggregateException ae)
    {
        ae.Handle(e =>
        {
            Console.WriteLine(e.Message);
            return true;
        });
    }
}
```

### Использование `CancellationTokenSource`

Можно использовать `CancellationTokenSource` путем задания одноименнного свойства у
`ParallelOptions`.

Вызов `cts.Cancel()` бросит `OperationCanceledException` - его необходимо отдельно
обработать (catch `AggregateException` здесь не сработает).

```csharp
private static void ExecuteAndCts()
{
    var cts = new CancellationTokenSource();
    var po = new ParallelOptions();
    po.CancellationToken = cts.Token;

    Parallel.For(0, 20, po, x =>
    {
        Console.WriteLine($"{x}. Task {Task.CurrentId}");
        if (x == 10)
            cts.Cancel();
    });
}

// Вызов параллельного цикла и обработка исключения.
static void Main(string[] args)
{
    try
    {
        ExecuteAndCts();
    }
    catch (OperationCanceledException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

## Lesson 34. Thread Local Storage

* Writing to a shared variable from many tasks is inefficient
* Can store partially evaluated results for each task
* Can specify a function to integrate partial results into final
results

Есть код - необходимо параллельно просуммировать числа. Переменная `sum` является общей:

```csharp
var sum = 0;
Parallel.For(1, 1001, x =>
{
    Interlocked.Add(ref sum, x);
});
```

Такой код довольно тормозной. Так как каждый поток пытается сделать lock для `sum`.

Одним из оптимизаций является использование Thread Local Storage - некоторое состояние (переменная
и т.п.), которое для каждого потока свое и не требует lock. В этой переменной запоминается
partial sum, которые потом суммируются. Это гораздо эффективнее первого подхода.

```csharp
var sum = 0;

Parallel.For(fromInclusive: 1, toExclusive: 1001,
localInit: () => 0,
body: (x, state, tls) =>
{
    tls += x;
    return tls;
},
localFinally: partialSum =>
{
    Interlocked.Add(ref sum, partialSum);
});
```

* `fromInclusive` - начальный индекс, включительно.
* `toExclusive` - конечный индекс, не включительно.
* `localInit` (`Func<TLocal>`) - делегат функции, который возвращает начальное состояние
локальных данных для каждой задачи.
* `body` (`Func<Int32,ParallelLoopState,TLocal,TLocal>` - делегат, который вызывается один раз
за итерацию.
  * `x` - входное значение
  * `state` (`ParallelLoopState`) - позволяет итерациям параллельных циклов взаимодействовать
  с другими итерациями.
  * `tls` - thread local storage (в данном примере - частичная сумма).
* `localFinally` (`Action<TLocal>`) - делегат, который выполняет финальное действие с
локальным состоянием каждой задачи. В данном примере `partialSum` - вычисленная частичная сумма
(локальное состояние каждой из параллельных задач), которые параллельно суммируются через lock
в переменную `sum`.

## Lesson 35. Partitioning

* Data is split into chunks by a partitioner
* Can create your own
* Goal: improve performance
  * E.g., void costly delegate creation calls

Для оценки производительности установим пакет `BenchmarkDotNet`:

```text
dotnet add package BenchmarkDotNet --version 0.13.1
```

Сам код:

```csharp
[Benchmark]
public void SquareEachValue()
{
    const int count = 100000;
    var values = Enumerable.Range(0, count);
    var results = new int[count];
    Parallel.ForEach(values, x => results[x] = (int)Math.Pow(x, 2));
}

static void Main(string[] args)
{
    var summary = BenchmarkRunner.Run<Program>();
    Console.WriteLine(summary);
}
```

Такой код не очень эффективен, т.к. параллельно создается очень много делегатов и каждый
делегат выполняет простую операцию возведения в квадрат.

Запустив программы на выполнение в Release режиме, получаем следующий результат:

```text
|          Method |     Mean |     Error |    StdDev |
|---------------- |---------:|----------:|----------:|
| SquareEachValue | 8.462 ms | 0.0896 ms | 0.0748 ms |
```

Теперь то же самое, только используя поддиапазоны чисел.

```csharp
[Benchmark]
public void SquareEachValueChunked()
{
    const int count = 100000;
    var values = Enumerable.Range(0, count);
    var results = new int[count];

    var part = Partitioner.Create(fromInclusive: 0, toExclusive: count, rangeSize: 10000);
    Parallel.ForEach(source: part, body: range =>
    {
        for (int i = range.Item1; i < range.Item2; i++)
        {
            results[i] = (int)Math.Pow(i, 2);
        }
    });
}
```

`Partitioner` - компонент, который определяет каким образом брать диапазон и как его разделить
на отдельные части.

`Partitioner.Create` - создает модуль разделения.

* `fromInclusive` - нижняя граница диапазона (включительно).
* `toExclusive` - верхняя граница диапазона (не включительно).
* `rangeSize` - размер каждого поддиапазона.

`Parallel.ForEach` - выполняет параллельно итерации.

* `source` (`Partitioner<TSource>`) - разделитель, содержащий исходный источник данных.
* `body` (`Action<TSource,ParallelLoopState>`) - делегат, который вызывается один раз за итерацию.

Запустив программы на выполнение в Release режиме, получаем следующий результат:

```text
|                 Method |     Mean |     Error |    StdDev |
|----------------------- |---------:|----------:|----------:|
|        SquareEachValue | 8.548 ms | 0.0669 ms | 0.0523 ms |
| SquareEachValueChunked | 3.355 ms | 0.0316 ms | 0.0296 ms |
```

(На видео была видна шестикратная разница).

## Summary

### Parallel Loops

* `Parallel.Xxx` are blocking calls
  * Wait until all threads completed *or* an exception occured
* Can check the state of the loop as it is executing in
`ParallelLoopState`
* Can check result of execution in `ParallelLoopResult`
* `ParallelLoopOptions` let us customize execution with
  * Max. degree of parallelism
  * Cancellation Token

### Parallel.Invoke

* Runs several provided functions concurrently
* Is equivalent to
  * Creating a task for each lambda
  * Doing a `Task.WaitAll()` on all the tasks

### Parallel.For/ForEach

* `Parallel.For`
  * Uses as index [start; finish]
  * Cannot provide a step
    * Create an `IEnumerable<int>` and use `Parallel.ForEach`
  * Partitions data into different tasks
  * Executes provided delegate with counter value argument
    * Might be inefficient
* `Parallel.ForEach`
  * Like `Parallel.For()` but
  * Takes an `IEnumerable<T>` instead

### Thread Local Storage

* Writing to a shared variable from many tasks is inefficient
* Can store partially evaluated results for each task
* Can specify a function to integrate partial results into final
results

### Partioning

* Data is split into chunks by a partitioner
* Can create your own
* Goal: improve performance
  * E.g., void costly delegate creation calls
