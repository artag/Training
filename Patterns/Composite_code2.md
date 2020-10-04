# Компоновщик. Код

## Лист, Контейнер

```csharp
// Класс Лист представляет собой конечные объекты структуры. Лист не может
// иметь вложенных компонентов.
//
// Обычно объекты Листьев выполняют фактическую работу, тогда как объекты
// Контейнера лишь делегируют работу своим подкомпонентам.
public class Leaf : Component
{
    public override string Operation()
    {
        return "Leaf";
    }

    public override bool IsComposite()
    {
        return false;
    }
}

    // Класс Контейнер содержит сложные компоненты, которые могут иметь
// вложенные компоненты. Обычно объекты Контейнеры делегируют фактическую
// работу своим детям, а затем «суммируют» результат.
public class Composite : Component
{
    private readonly List<Component> _children = new List<Component>();

    public override void Add(Component component)
    {
        _children.Add(component);
    }

    public override void Remove(Component component)
    {
        _children.Remove(component);
    }

    // Контейнер выполняет свою основную логику особым образом. Он проходит
    // рекурсивно через всех своих детей, собирая и суммируя их результаты.
    // Поскольку потомки контейнера передают эти вызовы своим потомкам и так
    // далее, в результате обходится всё дерево объектов.
    public override string Operation()
    {
        var i = 0;
        var result = "Branch(";

        foreach (var component in _children)
        {
            result += component.Operation();
            if (i != _children.Count - 1)
            {
                result += "+";
            }
            i++;
        }

        return result + ")";
    }
}
```
