# Цепочка обязанностей. Код

## Клиент, Main и результат выполнения

```csharp
public class Client
{
    // Обычно клиентский код приспособлен для работы с единственным
    // обработчиком. В большинстве случаев клиенту даже неизвестно, что этот
    // обработчик является частью цепочки.
    public void GiveFood(AbstractHandler<string> handler, string food)
    {
        Console.WriteLine($"Give food: {food}");
        var result = handler.Handle(food);

        if (result != food)
        {
            Console.Write($"    {result}");
        }
        else
        {
            // Конец цепочки обработки.
            Console.WriteLine($"    {food} was left untouched.");
        }
    }
}

static void Main(string[] args)
{
    var client = new Client();

    // Другая часть клиентского кода создает саму цепочку.
    var monkey = new MonkeyHandler();
    var squirrel = new SquirrelHandler();
    var dog = new DogHandler();

    monkey.SetNext(squirrel).SetNext(dog);

    Console.WriteLine("Chain: Monkey > Squirrel > Dog");
    client.GiveFood(monkey, "Nut");
    client.GiveFood(monkey, "Banana");
    client.GiveFood(monkey, "Coffee");
    Console.WriteLine();

    // Клиент должен иметь возможность отправлять запрос любому
    // обработчику, а не только первому в цепочке.
    Console.WriteLine("Chain: Squirrel > Dog");
    client.GiveFood(squirrel, "MeatBall");
    client.GiveFood(squirrel, "Banana");
    client.GiveFood(squirrel, "Coffee");
}
```

Результат выполнения:
```
Chain: Monkey > Squirrel > Dog
Give food: Nut
    Squirrel: I'll eat the Nut.
Give food: Banana
    Monkey: I'll eat the Banana.
Give food: Coffee
    Coffee was left untouched.

Chain: Squirrel > Dog
Give food: MeatBall
    Dog: I'll eat the MeatBall.
Give food: Banana
    Banana was left untouched.
Give food: Coffee
    Coffee was left untouched.
```
