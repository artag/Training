# Module 1. Task Programming

## Lesson 3. Creating and Starting Tasks

* Task is a unit of work that takes a function:
  * `new Task(function), t.Start()`
  * `Task.Factory.StartNew(function)`

* Tasks can be passed an object

* Tasks can return values:
  * `new Task<T>, task.Result`

* Tasks can report their state
  * `task.IsCompleted, task.IsFaulted`, etc.

Два способа создания task:

```csharp
static void Main(string[] args)
{
    // Способ 1. Создает и запускает task.
    Task.Factory.StartNew(() => Write('.'));

    // Способ 2. Отдельно создает и запускает task.
    var t = new Task(() => Write('?'));
    t.Start();
}

public static void Write(char c) { //... }
```

Создание task с передачей object:

```csharp
public static Main(string[] args)
{
    var t = new Task(Write, "hello");
    t.Start();

    Task.Factory.StartNew(Write, 123);
}

    public static void Write(object o) { //... }
```

Возврат значения из task, ожидание завершения работы:

```csharp
public static Main(string[] args)
{
    var text1 = "testing";
    var text2 = "this";

    var task1 = new Task<int>(TextLength, text1);
    task1.Start();

    var task2 = Task.Factory.StartNew<int>(TextLength, text2);

    // Ожидание выполнения task.
    Console.WriteLine($"Length of '{text1}' is {task1.Result}");
    Console.WriteLine($"Length of '{text2}' is {task2.Result}");
}

public static int TextLength(object o)
{
    Console.WriteLine($"\nTask with id {Task.CurrentId} processing object '{o}'...");
    return o.ToString().Length;
}
```

## Lesson 4. Cancelling Tasks

* Cancellation of tasks is supported via
  * `CancellationTokenSource`, which returns a
  * `CancellationToken token = cts.Token`

* The token is passed into the function
  * E.g., `Task.Factory.StartNew(..., token)`

* To cancel, we call `cts.Cancel()`

* Cancellation is cooperative
  * Task can check `token.IsCancellationRequested` and "soft fail" or
  * Throw an exception via `token.ThrowIfCancellationRequested()`

### Прерывание задачи

*1 Способ*. Прерывание task. Тип выхода "soft exit":

```csharp
static void Main(string[] args)
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    var t = new Task(() =>
    {
        var i = 0;
        while (true)
        {
            // Остановка. soft exit.
            if (token.IsCancellationRequested)
                break;

            Console.WriteLine($"{i++}");
        }
    }, token);
    t.Start();

    // Прерывание работы task
    cts.Cancel();
}
```

*2 Способ*. Более canonical way. Кидание `OperationCanceledException`:

```csharp
static void Main(string[] args)
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    var t = new Task(() =>
    {
        var i = 0;
        while (true)
        {
            // Остановка. More canonical way.
            if (token.IsCancellationRequested)
                    throw new OperationCanceledException();

            Console.WriteLine($"{i++}");
        }
    }, token);
    t.Start();

    // Прерывание работы task
    cts.Cancel();
}
```

*3 Способ*. Ну и наиболее recommended вариант. Объединение инструкции `if` и выброса exception:

```csharp
public static void Main(string[] args)
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    var t = new Task(() =>
    {
        var i = 0;
        while (true)
        {
            // Recommended way of stopping the task.
            token.ThrowIfCancellationRequested();
            Console.WriteLine($"{i++}");
        }
    }, token);
    t.Start();

    // Прерывание работы task
    cts.Cancel();
}
```

### Выполнить действие после отмены задачи

*1 Способ*. Подписка на событие `Register`:

```csharp
public static void Main(string[] args)
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    token.Register(() =>
    {
        Console.WriteLine("Cancelation has been requested.");
    });

    var t = new Task(() =>
    {
        var i = 0;
        while (true)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine($"{i++}");
        }
    }, token);
    t.Start();

    Console.ReadKey();
    cts.Cancel();
}
```

*2 Способ*. Более сложный. Ожидание в отдельном потоке срабатывания
token'а. При срабатывании ожидающий поток продолжает свою работу
и делает что надо сделать.

```csharp
public static void Main(string[] args)
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;

    var t = new Task(() =>
    {
        var i = 0;
        while (true)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine($"{i++}");
        }
    }, token);
    t.Start();

    // Ожидание срабатывания token.WaitHandle:
    Task.Factory.StartNew(() =>
    {
        token.WaitHandle.WaitOne();
        Console.WriteLine("Wait handle release, cancelation was requested");
    });

    Console.ReadKey();
    cts.Cancel();
}
```

### Composite cancellation token

```csharp
public static void Main(string[] args)
{
    var planned = new CancellationTokenSource();
    var preventative = new CancellationTokenSource();
    var emergency = new CancellationTokenSource();

    // Объединение token source.
    // Работа задачи будет прервана, если сработает один из token'ов.
    var paranoid = CancellationTokenSource.CreateLinkedTokenSource(
        planned.Token, preventative.Token, emergency.Token);

    Task.Factory.StartNew(() =>
    {
        var i = 0;
        while (true)
        {
            paranoid.Token.ThrowIfCancellationRequested();
            Console.WriteLine($"{i++}");
            Thread.Sleep(1000);
        }
    }, paranoid.Token);

    Console.ReadKey();
    // Вызовет прерывание работы задачи.
    // Этот прием также сработает на planned или preventative.
    emergency.Cancel();
}
```

## Lesson 5. Waiting for Time to Pass

* `Thread.Sleep(msec)`

* `token.WaitHandle.WaitOne(msec)`
  * Returns a bool indicating whether cancelation was requested in the time period specified

* Spin waiting does not give up the thread's turn
  * `Thread.SpinWait()`
  * `SpinWait.SpinUntil(function)`

Способы задержки работы task.

*1 Способ*. `Thread.Sleep`:

```csharp
static void Main(string[] args)
{
    var t = new Task(() =>
    {
        Thread.Sleep(1000);
    });
}
```

`Thread.Sleep` приостанавливает работу задачи на требуемое время и сообщает scheduler, что можно
на это время переключиться на выполнение других потоков.

*2 Способ*. `Thread.SpinWait()` или использовать `SpinWait.SpinUntil()`.

Тоже приостанавливают работу задачи, но не дают scheduler разрешения на переключение на выполнение
других потоков. С одной стороны это впустую тратит ресурсы ЦП, с другой стороны - не переключает
контекст выполнения.

*3 Способ*. Waiting on cancellation token.

```csharp
static void Main(string[] args)
{
    var cts = new CancellationTokenSource();
    var token = cts.Token;
    var t = new Task(() =>
    {
        Console.WriteLine("Press any key to disarm; you have 5 seconds");
        bool cancelled = token.WaitHandle.WaitOne(5000);
        Console.WriteLine(cancelled ? "Bomb disarmed." : "BOOM!!!");
    }, token);
    t.Start();

    Console.ReadKey();
    cts.Cancel();
}
```

## Lesson 6. Waiting for Tasks

* Waiting for single task
  * `task.Wait(optional timeout)`

* Waiting for several tasks
  * `Task.WaitAll(t1, t2)`
  * `Task.WaitAny(t1, t2)`

* `WaitAny`/`WaitAll` will throw on cancellation

Ожидание выполнения задачи.

1. `Task.Wait()` и `Task.Wait(CancellationToken)`:

```csharp
var cts = new CancellationTokenSource();
var token = cts.Token;
var t = new Task(() => { ... }, token);
t.Start();

// Ожидание завершения работы задачи.
t.Wait();
// или
t.Wait(token)
```

Ожидание завершения работы задачи досрочно завершается если сработает cancellation token.

2 `Task.WaitAll(params Task[] tasks)`:

```csharp
Task.WaitAll(t1, t2);
```

Ждет завершения работы всех задач.

3 `Task.WaitAny(params Task[] tasks)`:

```csharp
Task.WaitAny(t1, t2);
```

Ждет завершения работы любой из задач, до первой завершенной.

4 Для всех видов задач можно установить *timeout* ожидания:

```csharp
Task.WaitAny(new [] { t1, t2 }, 4000);
Task.WaitAny(new [] { t1, t2 }, 4000, token);    // и еще добавить token
```

5 При прерывании задачи, на ожидании завешения ее работы (`Task.WaitAll` и т.п.) вылетает
необработанное исключение `System.OperationCanceledException`. (Обработка исключений будет
рассмотрена в lesson 7).

6 Tasks can report their state: `Task.IsCompleted, Task.IsFaulted`, etc.

У каждой `Task` есть свойство `Status`, которое представляет собой enum `TaskStatus` - текущее
состояние выполнения task.

## Lesson 7. Exception Handling

* An unobserved task exception will not get handled

* `task.Wait()` or `Task.WaitAny()`/`WaitAll()` will catch an...
  * `AggregateException`
  * Use `ae.InnerExceptions` to iterate all exceptions caught
  * Use `ae.Handle(e => {...})` to selectively handle exceptions
    * Return `true` if handled, `false` otherwise

* Note: there are ways of handling unobserved exceptions
  * They are tricky and unreliable

```csharp
static void Main(string[] args)
{
    var t = Task.Factory.StartNew(() =>
    {
        Console.WriteLine("Running task...");
        throw new InvalidOperationException();
    });

    Console.WriteLine("Main program done.");
    Console.ReadKey();
}
```

В таком виде исключение, выброшенное внутри task, будет проигнорировано.

Но, если ожидать выполнение задач:

```csharp
static void Main(string[] args)
{
    var t1 = Task.Factory.StartNew(() => { /* выбрасывается какое-то исключение */ });
    var t2 = Task.Factory.StartNew(() => { /* выбрасывается какое-то исключение */ });

    Task.WaitAll(t1, t2);
}
```

то вылетит `AggregateException`. Идея, в том, что все исключения, которые были выброшены внутри
task, собираются в одно исключение - в `AggregateException`.

Это исключение ловится примерно так:

```csharp
static void Main(string[] args)
{
    var t1 = Task.Factory.StartNew(() => { /* выбрасывается какое-то исключение */ });
    var t2 = Task.Factory.StartNew(() => { /* выбрасывается какое-то исключение */ });

    try
    {
        Task.WaitAll(t1, t2);
    }
    catch (AggregateException ae)
    {
        foreach (var e in ae.InnerExceptions)
            Console.WriteLine($"Exception {e.GetType()} from {e.Source}");
    }
}
```

Выборочная обработка исключения внутри `AggregateException`:

```csharp
static void Main(string[] args)
{
    var t1 = Task.Factory.StartNew(() => { /* выбрасывается какое-то исключение */ });
    var t2 = Task.Factory.StartNew(() => { /* выбрасывается какое-то исключение */ });

    try
    {
        Task.WaitAll(t1, t2);
    }
    catch (AggregateException ae)
    {
        ae.Handle(e =>
        {
            if (e is InvalidOperationException)
            {
                Console.WriteLine("Invalid op!");
                return true; // true - исключение было обработано.
            }

            return false;   // false - исключение не было обработано.
        });
    }
}
```

Необработанные исключения из `AggregateException` пробрасываются дальше, в этом же "контейнере"
`AggregateException`.
