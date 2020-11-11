# Легковес. Код.

## Main и результат выполнения

```csharp
static void Main()
{
    var treeTypeFactory = new TreeTypeFactory();
    var forest = new Forest(treeTypeFactory, "grass");
    forest.PlantTree(1, 1, "Green Big", "green");
    forest.PlantTree(2, 2, "Blue Small", "blue");
    forest.PlantTree(3, 3, "Green Big", "green");
    forest.PlantTree(4, 4, "Blue Small", "blue");
    Console.WriteLine();

    forest.Draw();
}
```

Результат выполнения:
```
Create new tree type Name: Green Big; Color: green;
Create new tree type Name: Blue Small; Color: blue;
Get existing tree type Name: Green Big; Color: green;
Get existing tree type Name: Blue Small; Color: blue;

Tree at (1; 1). Draw Green Big tree with green color on grass.
Tree at (2; 2). Draw Blue Small tree with blue color on grass.
Tree at (3; 3). Draw Green Big tree with green color on grass.
Tree at (4; 4). Draw Blue Small tree with blue color on grass.
```
