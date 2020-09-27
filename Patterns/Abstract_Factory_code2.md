# Абстрактная фабрика. Код

## Продукты

```csharp
// Каждый отдельный продукт семейства продуктов должен иметь базовый
// интерфейс. Все вариации продукта должны реализовывать этот интерфейс.
public interface IAbstractProductA
{
    string MethodA();
}

public interface IAbstractProductB
{
    // Продукт B способен работать самостоятельно...
    string MethodB();

    // ...а также взаимодействовать с Продуктами А той же вариации.
    //
    // Абстрактная Фабрика гарантирует, что все продукты, которые она
    // создает, имеют одинаковую вариацию и, следовательно, совместимы.
    string AnotherMethodB(IAbstractProductA collaborator);
}

// Конкретные продукты создаются соответствующими Конкретными Фабриками.
internal class ConcreteProductA1 : IAbstractProductA
{
    public string MethodA()
    {
        return "The result of the product A1. Made by ConcreteFactory1.";
    }
}

internal class ConcreteProductA2 : IAbstractProductA
{
    public string MethodA()
    {
        return "The result of the product A2. Made by ConcreteFactory2.";
    }
}

// Конкретные Продукты создаются соответствующими Конкретными Фабриками.
internal class ConcreteProductB1 : IAbstractProductB
{
    public string MethodB()
    {
        return "The result of the product B1. Made by ConcreteFactory1.";
    }

    // Продукт B1 может корректно работать только с Продуктом A1. Тем не
    // менее, он принимает любой экземпляр Абстрактного Продукта А в
    // качестве аргумента.
    public string AnotherMethodB(IAbstractProductA collaborator)
    {
        var result = collaborator.MethodA();
        return $"The result of the B1 collaborating with the ({result})";
    }
}

internal class ConcreteProductB2 : IAbstractProductB
{
    public string MethodB()
    {
        return "The result of the product B2. Made by ConcreteFactory2.";
    }

    public string AnotherMethodB(IAbstractProductA collaborator)
    {
        var result = collaborator.MethodA();
        return $"The result of the B2 collaborating with the ({result})";
    }
}
```
