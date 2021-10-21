# Module 5. Parallel Loops

*Parallel versions of `for` and `foreach`*.

* Parallel Invoke/For/ForEach
  * Options, stepped for loop
* Stopping, cancellation and exceptions
* Thread Local Storage
* Partitioning

## Lesson 32. Parallel Invoke/For/ForEach

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
