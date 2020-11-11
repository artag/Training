# Легковес. Код.

## Легковес с общими полями, Легковес со всеми полями

```csharp
// Общий интерфейс классов-легковесов.
public interface IDrawableTree
{
    void Draw(string surface);
}

// Класс-легковес. Содержит общие поля.
// Ссылаемся на него из множества отдельных деревьев.
public class TreeType : IDrawableTree
{
    private readonly string _name;
    private readonly string _color;

    public TreeType(string name, string color)
    {
        _name = name;
        _color = color;
    }

    public void Draw(string surface)
    {
        Console.WriteLine($"Draw {_name} tree with {_color} color on {surface}.");
    }

    public override string ToString()
    {
        var tree = new StringBuilder();
        tree.Append($"Name: {_name}; ");
        tree.Append($"Color: {_color};");
        return tree.ToString();
    }
}

// Класс-легковес с уникальными полями.
// Также содержит другой класс-легковес с общими полями.
public class Tree : IDrawableTree
{
    private readonly int _x;
    private readonly int _y;
    private readonly TreeType _treeType;

    public Tree(int x, int y, TreeType treeType)
    {
        _x = x;
        _y = y;
        _treeType = treeType;
    }

    public void Draw(string surface)
    {
        Console.Write($"Tree at ({_x}; {_y}). ");
        _treeType.Draw(surface);
    }
}
```
