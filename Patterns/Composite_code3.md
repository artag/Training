# Компоновщик. Код

## Клиент, Main и результат выполнения

```csharp
// Работа с компонентами:
// 1. Через приватные поля (инициализация через конструктор)
// 2. Через параметры методов (как сделано здесь)
public class Client
{
    // Клиентский код работает со всеми компонентами через базовый
    // интерфейс.
    public void ClientCode(Component component)
    {
        var result = component.Operation();
        Console.WriteLine($"RESULT: {result}");
        Console.WriteLine();
    }

    // Благодаря тому, что операции управления потомками объявлены в базовом
    // классе Компонента, клиентский код может работать как с простыми, так
    // и со сложными компонентами, вне зависимости от их конкретных классов.
    public void ClientCode2(Component component1, Component component2)
    {
        if (component1.IsComposite())
        {
            component1.Add(component2);
        }

        var result = component1.Operation();
        Console.WriteLine($"RESULT: {result}");
        Console.WriteLine();
    }
}

static void Main(string[] args)
{
    var client = new Client();

    var leaf = new Leaf();

    var branch1 = new Composite();
    branch1.Add(new Leaf());
    branch1.Add(new Leaf());
    var branch2 = new Composite();
    branch2.Add(new Leaf());
    var tree = new Composite();
    tree.Add(branch1);
    tree.Add(branch2);

    Console.WriteLine("Client. Get Leaf:");
    client.ClientCode(leaf);

    Console.WriteLine("Client. Get Composite (tree):");
    client.ClientCode(tree);

    Console.WriteLine("Client. Get Composite (tree) and Leaf:");
    client.ClientCode2(tree, leaf);
}
```

Результат выполнения:
```
Client. Get Leaf:
RESULT: Leaf

Client. Get Composite (tree):
RESULT: Branch(Branch(Leaf+Leaf)+Branch(Leaf))

Client. Get Composite (tree) and Leaf:
RESULT: Branch(Branch(Leaf+Leaf)+Branch(Leaf)+Leaf)
```
