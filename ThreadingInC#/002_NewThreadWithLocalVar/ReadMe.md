```csharp
new Thread(Go);
Go();

static void Go()
{
    for (int cycles = 0; cycles < 5; cycles++)
        Console.Write("?");
}
```

Вывод:

```text
?????
```
