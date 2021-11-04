# Module 6. Parallel LINQ

* LINQ is an awesome technology for querying data.
* Uses a number of operators
  * `Select()`, `Sum()`, ...
* By default, execution is sequential

PLINQ is TPL's counterpart for parallel LINQ

In this section:

* `AsParallel()`
* Cancellation and exceptions
* Merge Options
* Custom Aggregation

## Lesson 38. AsParallel and ParallelQuery

`AsParallel()` - метод расширения, который возвращает `ParallelQuery<TSource>`.
`ParallelQuery` лениво вычисляется, его значение будет вычислено после, когда к нему будет
произведено обращение.

### AsParallel

Пример использования `AsParallel()`. Возведение чисел в куб:

```csharp
const int count = 50;

var items = Enumerable.Range(1, count).ToArray();
var results = new int[count];

items.AsParallel().ForAll(x =>
{
    var newValue = x * x * x;
    results[x - 1] = newValue;
});
```

Вычисление чисел производится параллельно и не по порядку, но в `results` будет сохранен
упорядоченный набор чисел.

### AsOrdered

Можно сохранить порядок выполнения операций при их параллельном вычислении - использовать
дополнительный метод `AsOrdered()`:

```csharp
const int count = 50;
var items = Enumerable.Range(1, count).ToArray();

var cubes = items.AsParallel().AsOrdered().Select(x => x * x * x);
```

Это ленивое вычисление - `cubes` будет вычислен после, когда к нему будет произведено обращение,
например:

```csharp
foreach (var result in cubes)
    Console.Write($"{result}\t");
```

## Lesson 39. Cancellation and Exceptions

### Обработка исключений

```csharp
var items = ParallelEnumerable.Range(1, 20);
var results = items.Select(i =>
{
    var result = Math.Log10(i);
    // Для демонстрации обработки исключения.
    if (result > 1) throw new InvalidOperationException();
    return result;
});

try
{
    foreach (var r in results)
        Console.WriteLine($"result = {r}");
}
catch (AggregateException ae)
{
    ae.Handle(e =>
    {
        Console.WriteLine($"{e.GetType().Name}: {e.Message}");
        return true;
    });
}
```

`Select` для вычисления `results` выполняется параллельно, т.к. `items` имеет
тип `ParallelQuery<int>`.

`results` имеет тип `ParallelQuery<int>`, поэтому его вычисление происходит на стадии обращения к
нему - в `foreach`.

Внутреннее исключение выбрасывается в виде `AggregateException`.

### Cancellation token

Пример прерывания параллельных вычислений с использованием `CancellationTokenSource`.

```csharp
var items = ParallelEnumerable.Range(1, 20);

var cts = new CancellationTokenSource();
var results = items.WithCancellation(cts.Token).Select(i =>
{
    var result = Math.Log10(i);
    return result;
});

try
{
    foreach (var r in results)
    {
        if (r > 1)
            cts.Cancel();

        Console.WriteLine($"result = {r}");
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("Canceled");
}
```

* Создается `CancellationTokenSource`.
* Используется метод `WithCancellation`.
* Прерывание через `cts.Cancel()`.
* При прерывании выбрасывается исключение `OperationCanceledException`, которое необходимо поймать
и обработать.

## Lesson 40. Merge Options

Есть такой код:

```csharp
var numbers = Enumerable.Range(1, 20).ToArray();

// Producer
var results = numbers.AsParallel()
    .Select(x =>
    {
        var result = Math.Log10(x);
        Console.WriteLine($"Produced {result}");
        return result;
    });

// Consumer
foreach (var result in results)
{
    Console.WriteLine($"Consumed {result}");
}
```

При работе вначале срабатывает в основном код Producer'а (и только очень редко из Consumer'а).
После отработки всего код Producer'а, начинает выполняться код Consumer'а.

Опция `WithMergeOptions()` поволяет контролировать как быстро нужно предоставить результаты
вычислений потребителю. `WithMergeOptions()` имеет следуюзие основные агументы:

* `ParallelMergeOptions.AutoBuffered` (по умолч.) - аккумулирует вычисляемые параллельно результаты
в буфер перед выдачей потребителю. Размер буфера автоматически выбирается системой.
* `ParallelMergeOptions.FullyBuffered` - вначале вычисляет параллельно все результаты и только
потом предоставляет их потребителю.
* `ParallelMergeOptions.NotBuffered` - выдает результаты потребителю сразу после их вычисления.

```csharp
var numbers = Enumerable.Range(1, 20).ToArray();

// Producer
var results = numbers
    .AsParallel()
    .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
    .Select(x =>
    {
        var result = Math.Log10(x);
        Console.WriteLine($"Produced {result}");        // <-- Добавлено
        return result;
    });

// Consumer
foreach (var result in results)
{
    Console.WriteLine($"Consumed {result}");
}
```

Здесь добавление `WithMergeOptions(ParallelMergeOptions.FullyBuffered)` заставит вначале полностью
выполнить код в Producer, и только потом начать выполнение в части Consumer.

## Lesson 41. Custom Aggregation

### Последовательная операция Aggregation

`Sum` и `Average` пример частных случаев операции `Aggregate`

```csharp
var sum = Enumerable.Range(1, 1000).Sum();              // Суммирование
var average = Enumerable.Range(1, 1000).Average();      // Нахождение среднего значения
```

Реализация суммирования чисел с использованием `Aggregate`

```csharp
var sum = Enumerable.Range(1, 1000)
    .Aggregate(seed: 0, (i, acc) => i + acc);
```

* `seed` - начальное значение в аккумуляторе
* `i` - текущее число для суммирования
* `acc` - аккумулятор

Это все последовательно выполняемые операции.

### Параллельная операция Aggregation

```csharp
var sum2 = ParallelEnumerable.Range(1, 1000)
    .Aggregate(
        seed: 0,
        (partialSum, i) => partialSum += i,
        (total, subtotal) => total += subtotal,
        i => i
    );
```

* `seed` - начальное значение в аккумуляторе
* updateAccumulatorFunc
* combineAccumulatorFunc
* resultSelector

## Summary

* Turn a LINQ query parallel by
  * Calling `AsParallel()` on an `IEnumerable`
  * Use a `ParallelEnumerable`
* Use `WithCancellation()` to provide a cancellation token
* Catch
  * `AggragateException`
  * `OperationCanceledException` if expecting to cancel
* `WithMergeOptions(ParallelMergeOptions.Xxx)` determine how soon
produces results can be consumed
* Parallel version of `Aggregate()` provides a syntax for custom
per-task aggregation options
