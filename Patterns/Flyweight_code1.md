# Легковес. Код.

## Фабрика Легковесов, Клиент

```csharp
// Фабрика классов-легковесов (с общими полями).
// Решает, когда нужно создать новый легковес, а когда можно обойтись существующим.
public class TreeTypeFactory 
{
    private readonly Dictionary<string, TreeType> _treeTypes =
        new Dictionary<string, TreeType>();

    public TreeType GetTreeType(string name, string color)
    {
        var treeType = new TreeType(name, color);
        var key = treeType.ToString();
        if (!_treeTypes.ContainsKey(key))
        {
            Console.WriteLine($"Create new tree type {key}");
            _treeTypes[key] = treeType;
        }
        else
        {
            Console.WriteLine($"Get existing tree type {key}");
        }

        return _treeTypes[key];
    }
}

// Клиент
public class Forest
{
    private readonly List<Tree> _trees = new List<Tree>();

    private readonly TreeTypeFactory _treeTypeFactory;
    private readonly string _surface;

    public Forest(TreeTypeFactory treeTypeFactory, string surface)
    {
        _treeTypeFactory = treeTypeFactory;
        _surface = surface;
    }

    public void PlantTree(int x, int y, string name, string color)
    {
        var treeType = _treeTypeFactory.GetTreeType(name, color);
        var tree = new Tree(x, y, treeType);
        _trees.Add(tree);
    }

    public void Draw()
    {
        _trees.ForEach(tree => tree.Draw(_surface));
    }
}
```
