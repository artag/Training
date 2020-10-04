# Команда. Код

## Команды

```csharp
// Интерфейс Команды объявляет метод для выполнения команд.
public interface ICommand
{
    void Execute();
}

// Некоторые команды способны выполнять простые операции самостоятельно.
internal class SimpleCommand : ICommand
{
    private readonly string _payload;

    public SimpleCommand(string payload)
    {
        _payload = payload;
    }

    public void Execute()
    {
        Console.WriteLine("SimpleCommand. Execute. Begin.");
        Console.WriteLine($"Printing: {_payload}");
        Console.WriteLine("SimpleCommand. Execute. End.");
        Console.WriteLine();
    }
}

// Есть команды, которые делегируют более сложные операции другим
// объектам, называемым "получателями".
class ComplexCommand : ICommand
{
    private readonly Receiver _receiver;

    // Данные о контексте, необходимые для запуска методов получателя.
    private readonly string _a;
    private readonly string _b;

    // Сложные команды могут принимать один или несколько объектов-
    // получателей вместе с любыми данными о контексте через конструктор.
    public ComplexCommand(Receiver receiver, string a, string b)
    {
        _receiver = receiver;
        _a = a;
        _b = b;
    }

    // Команды могут делегировать выполнение любым методам получателя.
    public void Execute()
    {
        Console.WriteLine("ComplexCommand. Execute. Begin");

        _receiver.DoSomething(_a);
        _receiver.DoSomethingElse(_b);

        Console.WriteLine("ComplexCommand. Execute. End");
        Console.WriteLine();
    }
}
```
