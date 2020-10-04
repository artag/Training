# Команда. Код

## Получатель, Вызывающий класс

```csharp
// Классы Получателей содержат некую важную бизнес-логику. Они умеют
// выполнять все виды операций, связанных с выполнением запроса. Фактически,
// любой класс может выступать Получателем.
internal class Receiver
{
    public void DoSomething(string a)
    {
        Console.WriteLine($"Receiver. DoSomething on {a}.");
    }

    public void DoSomethingElse(string b)
    {
        Console.WriteLine($"Receiver. DoSomethingElse on {b}.");
    }
}

// Интерфейс Вызывающего класса.
public interface IInvoker
{
    void Invoke();
}

// Объект Вызывающего класса может быть связан с одной или несколькими командами.
// Он отправляет запрос команде/командам.
internal class Invoker : IInvoker
{
    private readonly ICommand _startCommand;
    private readonly ICommand _finishCommand;

    public Invoker(ICommand startCommand, ICommand finishCommand)
    {
        _startCommand = startCommand;
        _finishCommand = finishCommand;
    }

    // Вызывающий объект не зависит от классов конкретных команд и получателей.
    // Вызывающий объект передаёт запрос получателю косвенно, выполняя команду.
    public void Invoke()
    {
        Console.WriteLine("Invoker. Execute start command");
        _startCommand.Execute();

        Console.WriteLine("Invoker: Execute finish command");
        _finishCommand.Execute();
    }
}
```