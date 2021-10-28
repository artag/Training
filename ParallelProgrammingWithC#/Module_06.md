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
