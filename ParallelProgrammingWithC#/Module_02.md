# Module 2. Data Sharing & Synchronization

* An operation is *atomic* if it cannot be interrupted (прервано).

* `x = 1;` is atomic but `x++` is **not**, as it is made up of 2 operations:
  * `temp <- x + 1; x <- temp`
  * Vulnerable (подвержены) to race condition

* Atomic operations:
  * reference assignment
  * reads and writes to value types <= 32 bits
  * 64-bit reads/writes on a 64-bit system

## Lesson 10. Critical sections

* Uses the `lock` keyword
* Typically locks on an existing object
  * Best to make a new `object` to lock on
* A shorthand for `Monitor.Enter()/Exit()`
* Blocks until a lock is available
  * Unless you use `Monitor.TryEnter()` with a timeout value

Рассматривается пример изменения одного свойства через методы сразу из нескольких потоков.
Возникает race condition.

Все потому что, работа со свойством через методы это неатомарная операция.

Самое простое решение - задать critical section. *Critical section* - часть кода, к которому
единовременно имеет доступ только один поток.

По сути - это использование операции `lock`:

```csharp
public class BankAccount
    {
    private object _padlock = new object();

    public int Balance { get; private set; }

    public void Deposit(int amount)
    {
        lock (_padlock)
        {
            Balance += amount;
        }
    }

    public void Withdraw(int amount)
    {
        lock (_padlock)
        {
            Balance -= amount;
        }
    }
}
```

`lock` - это сокращенный вариант записи `Monitor.Enter` - `Monitor.Exit`.

## Lesson 11. Interlocked operations

* Useful for atomically changing low-level primitives
* `Interlocked.Increment()/Decrement()`
* `Interlocked.Add()`
* `Exchange()/CompareExchange()`

Использование методов из класса `Interlocked`.

`Interlocked` работает с `ref` полями класса, поэтому для свойства необходимо использовать
backing field.

```csharp
public class BankAccount
{
    private int _balance;

    public int Balance
    {
        get => _balance;
        private set => _balance = value;
    }

    public void Deposit(int amount) =>
        Interlocked.Add(ref _balance, amount);

    public void Withdraw(int amount) =>
        Interlocked.Add(ref _balance, -amount);
}
```

* `Interlocked.Increment` и `Interlocked.Decrement` - увеличение и уменьшение величины на 1.

* `Interlocked.MemoryBarrier` - это условное обозначение операции `Thread.MemoryBarrier()`.
Полный барьер памяти гарантирует, что все чтения и записи расположенные до/после барьера будут
выполнены так же до/после барьера, то есть никакая инструкция обращения к памяти не может
перепрыгнуть барьер. (и больше никакой информации, кроме того, что этот механизм связан с
`volatile`).

* `Interlocked.Exchange` - безопасно устанавливает value и возвращает original value.

* `Interlocked.CompareExchange` - безопасно сравнивает два значения и если они эквивалентны,
замещает первое значение.

## Lesson 12. Spin Locking and Lock Recursion

* A spin lock wastes CPU cycles without yielding
  * Useful for brief pauses to prevent rescheduling (без переключения контекста - быстрое освобождение)
* `Enter()` to take, `Exit()` to release (if taken successfully)
* Lock recursion = ability to enter a lock twice on the same thread
* SpinLock does *not* support lock recursion
* Owner tracking helps keep a record of thread that acquired the lock
  * Recursion w/tracking = exception. w/o = deadlock

### Spin Locking

Используется `SpinLock` со стороны задач для ограничения одновременного доступа к shared data.

Потоки, одновременно пытающиеся пополнить и снять с одного счета:

```csharp
var ba = new BankAccount();
var tasks = new List<Task>();

var sl = new SpinLock();                    // Создание SpinLock.

for (int i = 0; i < 10; i++)
{
    tasks.Add(Task.Factory.StartNew(() =>
    {
        for (int j = 0; j < 1000; j++)
        {
            var lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);    // Захват.
                ba.Deposit(100);            // Безопасное изменение свойства через метод
            }
            finally
            {
                if (lockTaken)
                    sl.Exit();              // Освобождение.
            }
        }
    }));

    tasks.Add(Task.Factory.StartNew(() =>
    {
        for (int j = 0; j < 1000; j++)
        {
            var lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);    // Захват.
                ba.Withdraw(100);           // Безопасное изменение свойства через метод
            }
            finally
            {
                if (lockTaken)
                    sl.Exit();              // Освобождение.
            }
        }
    }));
}
```

### Lock Recursion

При попытке повторного захвата через `SpinLock` бросается исключение `LockRecursionException`.
Пример:

```csharp
static SpinLock sl = new SpinLock();

public static void LockRecursion(int x)
{
    bool lockTaken = false;
    try
    {
        sl.Enter(ref lockTaken);         // Захват.
    }
    catch (LockRecursionException ex)    // Кидается при попытке повторного захвата.
    {
        Console.WriteLine("Exception: " + ex);
    }
    finally
    {
        if (lockTaken)
        {
            Console.WriteLine($"Took a lock, x = {x}");
            LockRecursion(x - 1);   // Рекурсивный вызов (попытка повторного захвата).
            sl.Exit();              // Освобождение.
        }
        else
        {
            Console.WriteLine($"Failed to take a lock, x = {x}");
        }
    }
}
```

Строка `LockRecursion(x - 1);` пытается повторно рекурсивно сделать захват, до освобождения,
что может привести к deadlock. При повторной попытке захвата без освобождения предыдущего
выбрасывается исключение `LockRecursionException`.

Рекурсию в этом и похожих случаях опасно использовать.

## Lesson 13. Mutex

* A `WaitHandle`-derived synchronization primitive
* `WaitOne()` to acquire
  * Possibly with a timeout (можно задать timeout ожидания захвата)
* `ReleaseMutex()` to release
* `Mutex.WaitAll()` to acquire several (захват нескольких mutex)
* Global/named mutexes are shared between processes
  * `Mutex.OpenExisting()` to acquire
  * `mutex = new Mutex(false, <name>)`

### Использование одного mutex

`Mutex` контролирует доступ к определенной области кода. Чем-то похож на `lock`.

Идея - с помощью mutex пытаемся сделать lock. Когда его делаем, то выполняем нужную операцию.
После операции освобождаем mutex. Делается со стороны потоков.

```csharp
var ba = new BankAccount();
var tasks = new List<Task>();

var mutex = new Mutex();            // Создание mutex.

for (int i = 0; i < 10; i++)
{
    tasks.Add(Task.Factory.StartNew(() =>
    {
        for (int j = 0; j < 1000; j++)
        {
            // Приостанавливает выполнение потока до тех пор,
            // пока не будет получен мьютекс.
            var haveLock = mutex.WaitOne();            // Захват.
            try
            {
                ba.Deposit(100);
            }
            finally
            {
                if (haveLock) mutex.ReleaseMutex();    // Освобождение мьютекса.
            }
        }
    }));

    tasks.Add(Task.Factory.StartNew(() =>
    {
        for (int j = 0; j < 1000; j++)
        {
            var haveLock = mutex.WaitOne();            // Захват.
            try
            {
                ba.Withdraw(100);
            }
            finally
            {
                if (haveLock) mutex.ReleaseMutex();    // Освобождение мьютекса.
            }
        }
    }));
}
```

### Использование нескольких mutex

Mutex'ы могут взаимодействовать друг с другом. Пример - перевод с одного счета на другой в
многопоточной среде с использованием двух mutex'ов:

```csharp
var ba1 = new BankAccount();
var ba2 = new BankAccount();

var mutex1 = new Mutex();
var mutex2 = new Mutex();

// Захватывает сразу два mutex и работает с ними.
var haveLock = WaitHandle.WaitAll(new[]{ mutex1, mutex2 });
try
{
    // Одновременно безопасно меняются данные на первом банковском счете и на втором.
    ba1.Transfer(ba2, 1);
}
finally
{
    // Освобождаются оба mutex'а.
    if (haveLock)
    {
        mutex1.ReleaseMutex();
        mutex2.ReleaseMutex();
    }
}
```

### Использование одного mutex в нескольких процессах

Один mutex можно использовать между несколькими процессами. Например, одно приложение можно
запустить в единственном экземпляре, для всех остальных выдавать ошибку:

```csharp
static void Main(string[] args)
{
    const string appName = "MyApp";
    Mutex mutex;

            try
            {
                // Пытается открыть именованный мьютекс, если он уже существует.
                mutex = Mutex.OpenExisting(appName);
                // Покажет это сообщение, если работает другой процесс этого приложения.
                Console.WriteLine($"Sorry, {appName} is already running");
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                // Первый экземпляр программы.
                // Процесс не смог открыть мьютекс (т.к. его еще никто не создал).
                Console.WriteLine("We can run the program just fine");
                mutex = new Mutex(false, appName);      // Создание нового mutex
            }

            mutex.ReleaseMutex();       // Освобождение mutex
        }
```

Мои примечания:

1. В Linux этот прием срабатывает только в одном терминале.
2. На `mutex.ReleaseMutex()` кидает исключение.

## Lesson 14. Reader-Writer Locks

* A reader-writer lock can lock for reading or writing
  * (`Enter/Exit`)(`Read/Write`)`Lock()`
* Supports lock recursion in ctor parameter
  * Not recommended (трудно дебажить, легко сделать ошибку)
* Supports upgradeability (чтение + запись внутри одной блокировки)
  * `Enter/ExitUpgradeableReadLock()`

Используется `ReaderWriterLockSlim`. Он может быть использован в рекурсии (параметр
`recursionPolicy` (`LockRecursionPolicy.SupportsRecursion`) в конструкторе).

Тем не менее, также не рекомендуется использовать read-write locks в рекурсии.

### ReadLock и WriteLock

Для операций read используется один вид lock'ов (`EnterReadLock` и `ExitReadLock`),
для операций write - другой (`EnterWriteLock` и `ExitWriteLock`):

```csharp
static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();

static void Main(string[] args)
{
    int x = 0;

    var tasks = new List<Task>();
    for (var i = 0; i < 10; i++)
    {
        tasks.Add(Task.Factory.StartNew(() =>
        {
            padLock.EnterReadLock();
            Console.WriteLine($"Entered read lock, x = {x}");

            Thread.Sleep(5000);

            padLock.ExitReadLock();
            Console.WriteLine($"Exited read lock, x = {x}");
        }));
    }
    // ..
    while (true)
    {
        //
        padLock.EnterWriteLock();
        Console.WriteLine("Write lock acquired");

        // Установка нового значения.
        var newValue = random.Next(10);
        x = newValue;
        Console.WriteLine($"Set x = {x}");

        //
        padLock.ExitWriteLock();
        Console.WriteLine("Write lock released");
    }
}
```

### UpgradeableReadLock

Иногда требуется на этапе чтения значения его поменять. Для этого применяют такую конструкцию
(пример):

```csharp
var taskNum = i;
tasks.Add(Task.Factory.StartNew(() =>
{
    padLock.EnterUpgradeableReadLock();
    // Чтение
    Console.WriteLine($"Entered read lock, x = {x}");

    // Запись в нечетных задачах.
    if (taskNum % 2 == 1)
    {
        padLock.EnterWriteLock();
        x = random.Next(10);
        Console.WriteLine($"Set {x}");
        padLock.ExitWriteLock();
    }

    Thread.Sleep(5000);

    Console.WriteLine($"Exited read lock, x = {x}");
    padLock.ExitUpgradeableReadLock();
}));
```

## Summary

### Critical Sections

* Uses the `lock` keyword
* Typically locks on an existing object
  * Best to make a new `object` to lock on
* A shorthand for `Monitor.Enter()`/`Exit()`
* Blocks until a lock is available
  * Unless you use `Monitor.TryEnter()` with a timeout value

### Interlocked Operations

* Useful for atomically changing low-level primitives
* `Interlocked.Increment()`/`Decrement()`
* `Interlocked.Add()`
* `Exchange()`/`CompareExchange()`

### Spin Locking and Lock Recursion

* A spin lock wastes CPU cycles without yielding
  * Useful for brief pauses to prevent rescheduling (без переключения контекста - быстрое освобождение)
* `Enter()` to take, `Exit()` to release (if taken successfully)
* Lock recursion = ability to enter a lock twice on the same thread
* SpinLock does *not* support lock recursion
* Owner tracking helps keep a record of thread that acquired the lock
  * Recursion w/tracking = exception. w/o = deadlock

### Mutex

* A `WaitHandle` - derived synchronization primitive
* `WaitOne()` to acquire
  * Possibly with a timeout (можно задать timeout ожидания захвата)
* `ReleaseMutex()` to release
* `Mutex.WaitAll()` to acquire several (захват нескольких mutex)
* Global/named mutexes are shared between processes
  * `Mutex.OpenExisting()` to acquire
  * `mutex = new Mutex(false, <name>)`

### Reader-Writer Locks

* A reader-writer lock can lock for reading or writing
  * (`Enter`/`Exit`)(`Read`/`Write`)`Lock()`
* Supports lock recursion in ctor parameter
  * Not recommended (трудно дебажить, легко сделать ошибку)
* Supports upgradeability (чтение + запись внутри одной блокировки)
  * `Enter`/`ExitUpgradeableReadLock()`
