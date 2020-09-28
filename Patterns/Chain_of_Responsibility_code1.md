# Цепочка обязанностей. Код

## Обработчики

```csharp
// Интерфейс Обработчика объявляет метод построения цепочки обработчиков.
// Он также объявляет метод для выполнения запроса.
public interface IHandler<T>
{
    IHandler<T> SetNext(IHandler<T> handler);
    T Handle(T request);
}

// Поведение цепочки по умолчанию может быть реализовано внутри базового
// класса обработчика.
public abstract class AbstractHandler<T> : IHandler<T>
{
    private IHandler<T> _nextHandler;

    public IHandler<T> SetNext(IHandler<T> handler)
    {
        _nextHandler = handler;

        // Возврат обработчика отсюда позволит связать обработчики простым способом,
        // вот так: monkey.SetNext(squirrel).SetNext(dog);
        return handler;
    }

    public virtual T Handle(T request)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(request);
        }

        // Обычно тут возвращается null (если конец цепочки), но лучше так не делать.
        return request;
    }
}

public class MonkeyHandler : AbstractHandler<string>
{
    public override string Handle(string request)
    {
        if (request == "Banana")
        {
            return $"Monkey: I'll eat the {request}.\n";
        }

        return base.Handle(request);
    }
}

public class SquirrelHandler : AbstractHandler<string>
{
    public override string Handle(string request)
    {
        if (request == "Nut")
        {
            return $"Squirrel: I'll eat the {request}.\n";
        }

        return base.Handle(request);
    }
}

public class DogHandler : AbstractHandler<string>
{
    public override string Handle(string request)
    {
        if (request == "MeatBall")
        {
            return $"Dog: I'll eat the {request}.\n";
        }

        return base.Handle(request);
    }
}
```
