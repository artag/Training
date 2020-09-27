# Абстрактная фабрика. Код

## Main и результат выполнения

```csharp
class Program
{
    static void Main(string[] args)
    {
        var client1 = new Client(new ConcreteFactory1());
        var client2 = new Client(new ConcreteFactory2());

        client1.DoWork();
        Console.WriteLine("---");
        client2.DoWork();
    }
}
```

Результат выполнения:
```
The result of the product A1. Made by ConcreteFactory1.
The result of the product B1. Made by ConcreteFactory1.
The result of the B1 collaborating with the (The result of the product A1. Made by ConcreteFactory1.)
---
The result of the product A2. Made by ConcreteFactory2.
The result of the product B2. Made by ConcreteFactory2.
The result of the B2 collaborating with the (The result of the product A2. Made by ConcreteFactory2.)
```
