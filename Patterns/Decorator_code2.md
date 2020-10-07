# Декоратор. Код

## Клиент, Main и результат выполнения

```csharp
internal class Client
{
    // Клиентский код работает со всеми объектами, используя интерфейс
    // Компонента. Таким образом, он остаётся независимым от конкретных
    // классов компонентов, с которыми работает.
    public void Call(Component component)
    {
        Console.WriteLine("RESULT: " + component.Operation());
    }
}

static void Main(string[] args)
{
    var client = new Client();

    // Клиент использует простой компонент
    var simple = new ConcreteComponent();
    Console.WriteLine("Client. ConcreteComponent:");
    client.Call(simple);
    Console.WriteLine();

    // Декораторы могут обёртывать не только
    // простые компоненты, но и другие декораторы.
    var decorator1 = new ConcreteDecoratorA(simple);
    Console.WriteLine("Client. ConcreteComponent in DecoratorA:");
    client.Call(decorator1);
    Console.WriteLine();

    var decorator2 = new ConcreteDecoratorB(decorator1);
    Console.WriteLine("Client. ConcreteComponent in Decorators A and B:");
    client.Call(decorator2);
    Console.WriteLine();
}
```

Результат выполнения:
```
Client. ConcreteComponent:
RESULT: ConcreteComponent

Client. ConcreteComponent in DecoratorA:
RESULT: ConcreteDecoratorA(ConcreteComponent)

Client. ConcreteComponent in Decorators A and B:
RESULT: ConcreteDecoratorB(ConcreteDecoratorA(ConcreteComponent))
```