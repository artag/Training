# Декоратор. Код

## Компонент, Декоратор

```csharp
// Базовый абстрактный класс Компонента определяет поведение, которое изменяется декораторами.
// Компоненты также могут использовать интерфейс (IComponent).
public abstract class Component
{
    public abstract string Operation();
}

// Конкретные Компоненты предоставляют реализации поведения по умолчанию.
// Может быть несколько вариаций этих классов.
internal class ConcreteComponent : Component
{
    public override string Operation()
    {
        return "ConcreteComponent";
    }
}

// Базовый класс Декоратора следует тому же интерфейсу, что и другие компоненты.
// (Такого класса может и не быть - декораторы могут быть реализациями Component.)
public abstract class Decorator : Component
{
    private readonly Component _component;

    protected Decorator(Component component)
    {
        _component = component;
    }

    // Декоратор делегирует всю работу обёрнутому компоненту.
    public override string Operation()
    {
        return _component != null
            ? _component.Operation()
            : string.Empty;
    }
}

// Конкретные Декораторы вызывают обёрнутый объект и изменяют его результат некоторым образом.
internal class ConcreteDecoratorA : Decorator
{
    public ConcreteDecoratorA(Component comp) : base(comp)
    {
    }

    // Декораторы могут вызывать родительскую реализацию операции, вместо того,
    // чтобы вызвать обёрнутый объект напрямую.
    public override string Operation()
    {
        return $"ConcreteDecoratorA({base.Operation()})";
    }
}

// Декораторы могут выполнять своё поведение до или после вызова обёрнутого объекта.
internal class ConcreteDecoratorB : Decorator
{
    public ConcreteDecoratorB(Component comp) : base(comp)
    {
    }

    public override string Operation()
    {
        return $"ConcreteDecoratorB({base.Operation()})";
    }
}
```
