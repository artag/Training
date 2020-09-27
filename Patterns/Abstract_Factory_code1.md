# Абстрактная фабрика. Код.

## Фабрики

```csharp
// Интерфейс Абстрактной Фабрики объявляет набор методов, которые возвращают
// различные абстрактные продукты.  Эти продукты называются семейством и
// связаны темой или концепцией высокого уровня. Продукты одного семейства
// обычно могут взаимодействовать между собой. Семейство продуктов может
// иметь несколько вариаций,  но продукты одной вариации несовместимы с
// продуктами другой.
public interface IAbstractFactory
{
    IAbstractProductA CreateProductA();

    IAbstractProductB CreateProductB();
}

// Конкретная Фабрика производит семейство продуктов одной вариации. Фабрика
// гарантирует совместимость полученных продуктов.  Обратите внимание, что
// сигнатуры методов Конкретной Фабрики возвращают абстрактный продукт, в то
// время как внутри метода создается экземпляр  конкретного продукта.
internal class ConcreteFactory1 : IAbstractFactory
{
    public IAbstractProductA CreateProductA()
    {
        return new ConcreteProductA1();
    }

    public IAbstractProductB CreateProductB()
    {
        return new ConcreteProductB1();
    }
}

// Каждая Конкретная Фабрика имеет соответствующую вариацию продукта.
internal class ConcreteFactory2 : IAbstractFactory
{
    public IAbstractProductA CreateProductA()
    {
        return new ConcreteProductA2();
    }

    public IAbstractProductB CreateProductB()
    {
        return new ConcreteProductB2();
    }
}
```
