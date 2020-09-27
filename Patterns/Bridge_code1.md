# Мост. Код

## Абстракция и реализация

```csharp
public interface IAbstraction
{
    string Operation();
}

// Абстракция устанавливает интерфейс для «управляющей» части двух иерархий
// классов. Она содержит ссылку на объект из иерархии Реализации и
// делегирует ему всю настоящую работу.
internal class Abstraction : IAbstraction
{
    public Abstraction(IImplementation implementation)
    {
        Implementation = implementation;
    }

    protected IImplementation Implementation { get; }

    public virtual string Operation()
    {
        return "Abstraction: Base operation with:\n" +
               Implementation.OperationImplementation();
    }
}

// Можно расширить Абстракцию без изменения классов Реализации.
internal class ExtendedAbstraction : Abstraction
{
    public ExtendedAbstraction(IImplementation implementation)
        : base(implementation)
    {
    }

    public override string Operation()
    {
        return "ExtendedAbstraction: Extended operation with:\n" +
               base.Implementation.OperationImplementation();
    }
}

// Реализация устанавливает интерфейс для всех классов реализации. Он не
// должен соответствовать интерфейсу Абстракции. На практике оба интерфейса
// могут быть совершенно разными. Как правило, интерфейс Реализации
// предоставляет только примитивные операции, в то время как Абстракция
// определяет операции более высокого уровня, основанные на этих примитивах.
public interface IImplementation
{
    string OperationImplementation();
}

// Каждая Конкретная Реализация соответствует определённой платформе и
// реализует интерфейс Реализации с использованием API этой платформы.
internal class ConcreteImplementationA : IImplementation
{
    public string OperationImplementation()
    {
        return "The result in ConcreteImplementationA.\n";
    }
}

internal class ConcreteImplementationB : IImplementation
{
    public string OperationImplementation()
    {
        return "The result in ConcreteImplementationB.\n";
    }
}
```
