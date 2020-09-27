# Адаптер. Код

```csharp
// Адаптируемый интерфейс.
public interface ISource
{
    string GetRequestSource();
}

// Адаптируемый класс содержит некоторое полезное поведение, но его
// интерфейс несовместим  с существующим клиентским кодом. Адаптируемый
// класс нуждается в некоторой доработке,  прежде чем клиентский код сможет
// его использовать.
internal class Source : ISource
{
    public string GetRequestSource()
    {
        return "Source request.";
    }
}

// Целевой класс объявляет интерфейс, с которым может работать клиентский код.
public interface ITarget
{
    string GetRequest();
}

// Адаптер делает интерфейс Адаптируемого класса совместимым с целевым
// интерфейсом.
internal class Adapter : ITarget
{
    private readonly ISource _source;

    public Adapter(ISource source)
    {
        _source = source;
    }

    public string GetRequest()
    {
        var resultSource = _source.GetRequestSource();
        return $"This is '{resultSource}'";
    }
}

class Program
{
    static void Main(string[] args)
    {
        var source = new Source();
        var target = new Adapter(source);

        var result = target.GetRequest();
        Console.WriteLine($"Result: {result}");
    }
}
```

Результат выполнения:
```
Result: This is 'Source request.'
```
