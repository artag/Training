# Фабричный метод. Код

## Клиент, Main и результат выполнения

```csharp
// Клиентский код работает с экземпляром конкретного создателя, хотя и
// через его базовый интерфейс.
class Client1
{
    private readonly Creator _creator;

    public Client1(Creator creator)
    {
        _creator = creator;
    }

    // Используется создатель, заданный через конструктор.
    public void DoWork()
    {
        var result = _creator.SomeOperation();
        Console.WriteLine(result);
    }
}

class Client2
{
    // Используется создатель, передаваемый в метод.
    public void DoWork(Creator creator)
    {
        var result = creator.SomeOperation();
        Console.WriteLine(result);
    }
}

static void Main(string[] args)
{
    Console.WriteLine("Client: Launched with the ConcreteCreator1.");
    Console.WriteLine("Using constructor:");
    var client1 = new Client1(new ConcreteCreator1());
    client1.DoWork();

    Console.WriteLine();

    Console.WriteLine("Client: Launched with the ConcreteCreator2.");
    Console.WriteLine("Using method:");
    var client2 = new Client2();
    client2.DoWork(new ConcreteCreator2());
}
```

Результат выполнения:
```
Client: Launched with the ConcreteCreator1.
Using constructor:
Creator: {Result of ConcreteProduct1}

Client: Launched with the ConcreteCreator2.
Using method:
Creator: {Result of ConcreteProduct2}
```
