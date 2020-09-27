# Строитель. Код

## Директор и Продукт

```csharp
// Директор отвечает только за выполнение шагов построения в определённой
// последовательности. Это полезно при производстве продуктов в определённом
// порядке или особой конфигурации. Строго говоря, класс Директор
// необязателен, так как клиент может напрямую управлять строителями.
//
// Вариант Директора с установкой Строителя через конструктор.
public class DirectorA
{
    private IBuilder _builder;

    public DirectorA(IBuilder builder)
    {
        _builder = builder;
    }

    // Директор может строить несколько вариаций продукта, используя
    // одинаковые шаги построения.
    public void BuildMinimalProduct()
    {
        _builder.BuildPartA();
    }

    public void BuildFullProduct()
    {
        _builder.BuildPartA();
        _builder.BuildPartB();
        _builder.BuildPartC();
    }
}

// Вариант Директора с установкой Строителя через метод.
public class DirectorB
{
    public void BuildMinimalProduct(IBuilder builder)
    {
        builder.BuildPartA();
    }

    public void BuildFullProduct(IBuilder builder)
    {
        builder.BuildPartA();
        builder.BuildPartB();
        builder.BuildPartC();
    }
}

// Имеет смысл использовать паттерн Строитель только тогда, когда ваши
// продукты достаточно сложны и требуют обширной конфигурации.
//
// В отличие от других порождающих паттернов, различные конкретные строители
// могут производить несвязанные продукты. Другими словами, результаты
// различных строителей  могут не всегда следовать одному и тому же
// интерфейсу.
public class Product
{
    private readonly List<string> _parts = new List<string>();

    public void Add(string part)
    {
        _parts.Add(part);
    }

    public void Show()
    {
        Console.WriteLine("*** Product Parts ***");
        foreach (var part in _parts)
            Console.WriteLine(part);
    }
}
```
