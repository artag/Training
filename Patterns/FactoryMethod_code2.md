# Фабричный метод. Код

## Продукт

```csharp
// Интерфейс Продукта объявляет операции, которые должны выполнять все
// конкретные продукты.
interface IProduct
{
    string Operation();
}

// Конкретные Продукты предоставляют различные реализации интерфейса
// Продукта.
class ConcreteProduct1 : IProduct
{
    public string Operation()
    {
        return "{Result of ConcreteProduct1}";
    }
}

class ConcreteProduct2 : IProduct
{
    public string Operation()
    {
        return "{Result of ConcreteProduct2}";
    }
}
```