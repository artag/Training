# Строитель. Код

## Main (клиентский код) и результат выполнения

```csharp
// Клиентский код создаёт объект-строитель, передаёт его директору,
// а затем инициирует  процесс построения. Конечный результат
// извлекается из объекта-строителя.
static void Main(string[] args)
{
    var builder = new ConcreteBuilder();

    var directorA = new DirectorA(builder);

    Console.WriteLine("Use DirectorA. Minimal product:");
    directorA.BuildMinimalProduct();
    var minProduct = builder.GetResult();
    minProduct.Show();

    Console.WriteLine("---");
    Console.WriteLine("Use DirectorA. Full product:");
    directorA.BuildFullProduct();
    var fullProduct = builder.GetResult();
    fullProduct.Show();

    Console.WriteLine("---");
    Console.WriteLine("Use DirectorB. Minimal product:");
    var directorB = new DirectorB();
    directorB.BuildMinimalProduct(builder);
    builder.GetResult().Show();

    // Паттерн Строитель можно использовать без класса Директор.
    Console.WriteLine("---");
    Console.WriteLine("Without Director. Custom product:");
    builder.BuildPartA();
    builder.BuildPartC();
    builder.GetResult().Show();
}
```

Результат выполнения:
```
Use DirectorA. Minimal product:
*** Product Parts ***
PartA1
---
Use DirectorA. Full product:
*** Product Parts ***
PartA1
PartB1
PartC1
---
Use DirectorB. Minimal product:
*** Product Parts ***
PartA1
---
Without Director. Custom product:
*** Product Parts ***
PartA1
PartC1
```
