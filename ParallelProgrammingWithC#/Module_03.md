# Module 3. Concurrent Collections

* `ConcurrentDictionary`
* Producer-consumer collections
  * `ConcurrentQueue`
  * `ConcurrentStack`
  * `ConcurrentBag`
* Producer-consumer pattern
  * `BlockingCollection`

## Lesson 17. `ConcurrentDictionary`

Коллекция `ConcurrentDictionary`.

Методы:

* `TryAdd` - пытается добавить элемент в словарь. Возвращает `bool` = true, если элемент с таким
ключом уже есть (вместо бросания исключения, как это сделано в обычном словаре).
* `AddOrUpdate` - если элемент есть - обновить, если элемента нет - добавить.
* `GetOrAdd` - получает значение из словаря если оно есть, если его нет, то добавляет его в словарь.
* `TryRemove` - пытается удалить значение из словаря по ключу. Возвращает удаленное значение,
если элемент был удален.

### `TryAdd`

Добавление элементов в коллекцию из разных потоков:

```csharp
private static ConcurrentDictionary<string, string> capitals =
    new ConcurrentDictionary<string, string>();

private static void AddParis()
{
    var success = capitals.TryAdd("France", "Paris");

    // Какой поток вызвал метод. CurrentId = null, если поток Main thread
    var who = Task.CurrentId.HasValue ? ("Task " + Task.CurrentId) : "Main thread";
    Console.WriteLine($"{who} {(success ? "added" : "did not add")} the element.");
}

public static void Main()
{
    var task = Task.Factory.StartNew(AddParis);
    AddParis();
    task.Wait();
}
```

### `AddOrUpdate`

Добавление или обновление элементов в коллекцию из разных потоков:

```csharp
private static ConcurrentDictionary<string, string> capitals =
    new ConcurrentDictionary<string, string>();

private static void AddOrUpdate(string newValue)
{
    capitals.AddOrUpdate(
        "Russia", newValue, (key, oldvalue) => oldvalue + $" --> {newValue}");
}

public static void Main()
{
    var task1 = Task.Factory.StartNew(() => AddOrUpdate("Leningrad"));
    var task2 = Task.Factory.StartNew(() => AddOrUpdate("Moscow"));

    Task.WaitAll(task1, task2);
    Console.WriteLine($"The capital of Russia is {capitals["Russia"]}");
}
```

Напечатает:

```text
The capital of Russia is Leningrad --> Moscow
или
The capital of Russia is Moscow --> Leningrad
```

### `GetOrAdd`

Получение или добавление элементов в коллекцию из разных потоков:

```csharp
private static ConcurrentDictionary<string, string> capitals =
    new ConcurrentDictionary<string, string>();

private static void GetOrAdd(string newValue)
{
    capitals.GetOrAdd("Sweden", newValue);
    Console.WriteLine($"The capital of Sweden is {capitals["Sweden"]}");
}

public static void Main()
{
    var task1 = Task.Factory.StartNew(() => GetOrAdd("Uppsala"));
    var task2 = Task.Factory.StartNew(() => GetOrAdd("Stockholm"));

    Task.WaitAll(task1, task2);
}
```

Напечатает два раза:

Или

```text
The capital of Sweden is Uppsala
```

Или

```text
The capital of Sweden is Stockholm
```

### `TryRemove`

Удаление элементов из коллекции из разных потоков:

```csharp
private static ConcurrentDictionary<string, string> capitals =
    new ConcurrentDictionary<string, string>();

private static void TryRemove(string taskName, string keyToRemove)
{
    string removed;
    var didRemove = capitals.TryRemove(keyToRemove, out removed);
    if (didRemove)
        Console.WriteLine($"Task {taskName}: We just removed {removed}.");
    else
        Console.WriteLine($"Task {taskName}: Failed to remove the capital of {keyToRemove}.");
}

public static void Main()
{
    capitals["Russia"] = "Moscow";      // Обычное добавление значения в словарь.

    var task1 = Task.Factory.StartNew(() => TryRemove("1", "Russia"));
    var task2 = Task.Factory.StartNew(() => TryRemove("2", "Russia"));

    Task.WaitAll(task1, task2);
}
```

### Другие операции

`Count` - затратная операция для конкурентного словаря. Рекомендуется использовать как можно реже.

Использование `foreach`:

```csharp
foreach (var kv in capitals)
{
    Console.WriteLine($" - {kv.Value} is the capital of {kv.Key}");
}
```

## Lesson 18. `ConcurrentQueue`

Работает похоже на обычный `Queue`.

* `Enqueue` - добавление элементов (как обычно)
* `TryDequeue`
* `TryPeek`

Методы `Try`* возвращают `bool`, true - если удалось выполнить операцию.
Возвращают значение, которое было получено в результате операции (если она была успешной).

```csharp
public static void Main(string[] args)
{
    var q = new ConcurrentQueue<int>();
    q.Enqueue(1);
    q.Enqueue(2);

    // 2 1 <- front

    int result;
    if (q.TryDequeue(out result))
        Console.WriteLine($"Removed element {result}");

    if (q.TryPeek(out result))
        Console.WriteLine($"Front element is {result}");
}
```
