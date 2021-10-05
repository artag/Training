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
перепрыгнуть барьер. (и больше никакой информации, кроме того, что это связано с `volatile`).

* `Interlocked.Exchange` - безопасно устанавливает value и возвращает original value. 

* `Interlocked.CompareExchange` - безопасно сравнивает два значения и если они эквивалентны,
замещает первое значение.

## Lesson 12. Spin Locking and Lock Recursion

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

`Mutex` контролирует доступ к определенной области кода. Чем-то похоэ на `lock`.

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
            var haveLock = mutex.WaitOne();            // Захват.
            try
            {
                ba.Deposit(100);
            }
            finally
            {
                if (haveLock) mutex.ReleaseMutex();    // Освобождение.
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
                if (haveLock) mutex.ReleaseMutex();    // Освобождение.
            }
        }
    }));
}
```

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
