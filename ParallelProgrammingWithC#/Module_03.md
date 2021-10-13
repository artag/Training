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

Методы вида `Try`* возвращают `bool`, true - если удалось выполнить операцию.
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

## Lesson 19. `ConcurrentStack`

Основные методы:

* `Push` - добавление элементов (как обычно)
* `TryPeek`
* `TryPop`
* `TryPopRange` - пытается извлечь несколько элементов за раз.

Методы `TryPeek` и `TryPop` возвращают `bool`, true - если удалось выполнить операцию.
Возвращают значение, которое было получено в результате операции (если она была успешной).

Метод `TryPopRange` возвращает `int` - количество элементов, которое удалось извлечьиз стека.
Возвращается несколько элементов в виде массива - указывается смещение в массиве и сколько элементов
необходимо получить из стека. Все остальные элементы в массиве остаются прежними.

```csharp
static void Main(string[] args)
{
    var stack = new ConcurrentStack<int>();

    // Добавление элементов в стек.
    stack.Push(1);
    stack.Push(2);
    stack.Push(3);
    stack.Push(4);

    int result;
    if (stack.TryPeek(out result))
        Console.WriteLine($"{result} is on top");
        // Вывод в консоль: 4 is on top

    if (stack.TryPop(out result))
        Console.WriteLine($"Popped {result}");
        // Вывод в консоль: Popped 4

    var items = new int[5];
    items[3] = 77;
    items[4] = 666;     // 77 и 666 останутся, как будет видно ниже

    // startIndex - с какого индекса будет заполнен массив items.
    // count - сколько элементов попытаться извлечь из стека.
    if (stack.TryPopRange(items, startIndex: 0, count: 5) > 0)    // В стеке осталось 3 элемента.
    {
        var text = string.Join(", ", items.Select(i => i.ToString()));
        Console.WriteLine($"Popped these items: {text}");
        // Вывод в консоль: Popped these items: 3, 2, 1, 77, 666
    }
}
```
