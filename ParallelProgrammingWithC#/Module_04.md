# Module 4. Task Coordination

Getting multiple tasks to execute in a particular order.

* Continuations - вслед за task начинает свое выполнение другой task
* Child Tasks - task создает дочерние tasks
* More synchronization primitives
  * `Barrier` - используются для моделирования multi-stage concurrent algorithms
  * `CountdownEvent`
  * `ManualResetEventSlim`/`AutoResetEvent`
  * `SemaphoreSlim`

## Lesson 24. Continuations

### `ContinueWith`

Используется конструкция `task.ContinueWith(Action<Task> continuationAction)`.

Запускает вторую задачу после завершения первой.

```csharp
var task = Task.Factory.StartNew(() =>
{
    Console.WriteLine("Boiling water");
});

var task2 = task.ContinueWith(t =>
{
    Console.WriteLine($"Completed task {t.Id}, pour water into cup.");
});

task2.Wait();
```

Выведет:

```text
Boiling water
Completed task 1, pour water into cup.
```

### `ContinueWhenAll`

Используется конструкция `task.ContinueWhenAll(Task[] tasks, Action<Task> continuationAction)`.

Запускает следующую задачу после завершения всех указанных предыдущих задач.

```csharp
var task1 = Task.Factory.StartNew(() => "Task 1");
var task2 = Task.Factory.StartNew(() => "Task 2");

var task3 = Task.Factory.ContinueWhenAll(
    new [] { task1, task2 },
    tasks =>
    {
        Console.WriteLine("Tasks completed:");
        foreach (var t in tasks)
            Console.WriteLine(" - " + t.Result);
        Console.WriteLine("All tasks done");
    });

task3.Wait();
```

### `ContinueWhenAny`

Запускает следующую задачу после завершения одной из указанных предыдущих задач.

```csharp
var task1 = Task.Factory.StartNew(() => "Task 1");
var task2 = Task.Factory.StartNew(() => "Task 2");

var task3 = Task.Factory.ContinueWhenAny(
    new [] { task1, task2 },
    previousTask =>
    {
        Console.WriteLine("Tasks completed:");
        Console.WriteLine(" - " + previousTask.Result);
        Console.WriteLine("All tasks done");
    });

task3.Wait();
```

## Lesson 25. Child Tasks

Параметр `TaskCreationOptions.AttachedToParent` позволяет привязать child задачу к задаче parent.
Т.о., ожидание завершения задачи parent (`parent.Wait()`), будет также ожидать заверешение
дочерней задачи.

```csharp
var parent = new Task(() =>
{
    // Дочерняя задача.
    var child = new Task(() =>
    {
        Console.WriteLine("Child task starting.");
        Thread.Sleep(3000);
        Console.WriteLine("Child task finishing.");
    }, TaskCreationOptions.AttachedToParent);

    child.Start();
});

    parent.Start();

try
{
    parent.Wait();
}
catch (AggregateException ae)
{
    ae.Handle(e => true);
}
```

С доченими задачами возможно использовать `ContinueWith`. В следующем примере показано, как
запускать один обработчик `ContinueWith` при успешном окончании работы дочерней задачи, и другой,
если дочерняя задача завершилась с ошибкой.

```csharp
var parent = new Task(() =>
{
    // Дочерняя задача.
    var child = new Task(() =>
    {
        Console.WriteLine("Child task starting.");
        Thread.Sleep(3000);
        Console.WriteLine("Child task finishing.");
        // throw new Exception();      // To test failure handler
    }, TaskCreationOptions.AttachedToParent);

    // Сработает если предыдущая дочерняя задача успешно завершится.
    var completionHandler = child.ContinueWith(t =>
    {
        Console.WriteLine($"Hooray, task {t.Id}'s state is {t.Status}");
    },
    TaskContinuationOptions.AttachedToParent |
    TaskContinuationOptions.OnlyOnRanToCompletion);

    // Сработает если предыдущая дочерняя задача завершится с ошибкой.
    var failHandler = child.ContinueWith(t =>
    {
        Console.WriteLine($"Oops, task {t.Id}'s state is {t.Status}");
    },
    TaskContinuationOptions.AttachedToParent |
    TaskContinuationOptions.OnlyOnFaulted);

    child.Start();
});

    parent.Start();

try
{
    parent.Wait();
}
catch (AggregateException ae)
{
    ae.Handle(e => true);
}
```

## Lesson 26. `Barrier`

Класс `Barrier` - позволяет нескольким задачам параллельно работать с алгоритмом, используя
несколько фаз.

Аргументы, задаваемые при создании `Barrier`:

* `participantCount` - число worker threads, работающих с каким-либо кодом (алгоритмом).
* `postPhaseAction` - handler, запускаемый после заверешения работы фазы.

У объекта `Barrier` есть:

* Свойство `ParticipantsRemaining` - Gets the number of participants in the barrier that haven't
yet signaled in the current phase.

* Методы `AddParticipant()` и `AddParticipants(int participantCount)` - notifies the `Barrier`
that there will be additional participant(s).

* Метод `SignalAndWait()`, плюс перегруженные методы - signals that a participant has reached
the barrier and waits for all other participants to reach the barrier as well.

Пример. Алгоритм по приготовлению чая (3 фазы выполнения программы):

```csharp
static Barrier barrier = new Barrier(participantCount: 2, postPhaseAction: b =>
{
    Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished");
});

public static void Water()
{
    Console.WriteLine("Phase 0. Putting the kettle on (takes a bit longer)");
    Thread.Sleep(2000);
    barrier.SignalAndWait();
    Console.WriteLine("Phase 1. Pouring water into cup");
    barrier.SignalAndWait();
    Console.WriteLine("Phase 2. Putting the kettle away");
}

public static void Cup()
{
    Console.WriteLine("Phase 0. Finding the nicest cup of tea (fast)");
    barrier.SignalAndWait();
    Console.WriteLine("Phase 1. Adding tea");
    barrier.SignalAndWait();
    Console.WriteLine("Phase 2. Adding sugar");
}

static void Main(string[] args)
{
    var water = Task.Factory.StartNew(Water);
    var cup = Task.Factory.StartNew(Cup);

    var tea = Task.Factory.ContinueWhenAll(new [] { water, cup }, tasks =>
    {
        Console.WriteLine("Enjoy your cup of tea.");
    });

    tea.Wait();
}
```

Выполнение программы:

```text
Phase 0. Finding the nicest cup of tea (fast)
Phase 0. Putting the kettle on (takes a bit longer)
Phase 0 is finished
Phase 1. Pouring water into cup
Phase 1. Adding tea
Phase 1 is finished
Phase 2. Adding sugar
Phase 2. Putting the kettle away
Enjoy your cup of tea.
```

## Lesson 27. `CountdownEvent`

`CountdownEvent` по функционалу схож с `Barrier`. Только сигнал `Signal()` и ожидание `Wait()`
разделены друг от друга.

Кажая из задач сигнализирует о своем завершении и уменьшает внутренний счетчик у `CountdownEvent`
на единицу. Когда внутренний счетчик обнуляется, то завершается ожидание `cte.Wait()`.

Пример. Запуск 5 задач. Ожидание окончания их работы (всех задач) в finalTask.

```csharp
private static int taskCount = 5;   // Количество запускаемых задач.
private static Random random = new Random();
static CountdownEvent cte = new CountdownEvent(taskCount);

static void Main(string[] args)
{
    for (int i = 0; i < taskCount; i++)
    {
        Task.Factory.StartNew(() =>
        {
            Console.WriteLine($"Entering task {Task.CurrentId}");
            Thread.Sleep(random.Next(3000));
            // Регистрирует сигнал и уменьшает счетчик в CountdownEvent.
            cte.Signal();
            Console.WriteLine($"Exiting task {Task.CurrentId}");
        });
    }

    var finalTask = Task.Factory.StartNew(() => 
    {
        Console.WriteLine($"Waiting for other tasks to complete in {Task.CurrentId}");
        // Ожидает завершения всех задач. По окончании ожидания продолжает работу дальше.
        cte.Wait();
        Console.WriteLine("All tasks completed");
    });

    finalTask.Wait();
}
```
