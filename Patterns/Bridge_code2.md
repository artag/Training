# Мост. Код

## Клиент, Main и результат выполнения

```csharp
// Интерфейс клиента.
public interface IClient
{
    void DoWork();
}

// За исключением этапа инициализации, когда объект Абстракции
// связывается с определённым объектом Реализации, клиентский код должен
// зависеть только от класса Абстракции. Таким образом, клиентский код
// может поддерживать любую комбинацию абстракции и реализации.
internal class Client : IClient
{
    private readonly IAbstraction _abstraction;

    public Client(IAbstraction abstraction)
    {
        _abstraction = abstraction;
    }

    public void DoWork()
    {
        var result = _abstraction.Operation();
        Console.WriteLine($"Result: {result}");
    }
}

static void Main(string[] args)
{
    var abstraction1 = new Abstraction(new ConcreteImplementationA());
    var client1 = new Client(abstraction1);
    client1.DoWork();

    Console.WriteLine();

    var abstraction2 = new ExtendedAbstraction(new ConcreteImplementationB());
    var client2 = new Client(abstraction2);
    client2.DoWork();
}
```

Результат выполнения:
```
Result: Abstraction: Base operation with:
The result in ConcreteImplementationA.


Result: ExtendedAbstraction: Extended operation with:
The result in ConcreteImplementationB.
```
