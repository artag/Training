# Фасад. Код

## Клиент, Main и результат выполнения

```csharp
// Клиентский код работает со сложными подсистемами через простой
// интерфейс, предоставляемый Фасадом. Когда фасад управляет жизненным
// циклом подсистемы, клиент может даже не знать о существовании
// подсистемы. Такой подход позволяет держать сложность под контролем.
public class Client
{
    public static void Run(Facade facade)
    {
        Console.Write(facade.Operation());
    }
}

// В клиентском коде могут быть уже созданы некоторые объекты
// подсистемы. В этом случае может оказаться целесообразным
// инициализировать Фасад с этими объектами вместо того, чтобы
// позволить Фасаду создавать новые экземпляры.
static void Main(string[] args)
{
    var subsystem1 = new Subsystem1();
    var subsystem2 = new Subsystem2();
    var facade = new Facade(subsystem1, subsystem2);
    Client.Run(facade);
}
```

Результат выполнения:
```
Facade initializes subsystems:
Subsystem1: Ready!
Subsystem2: Get ready!
Facade orders subsystems to perform the action:
Subsystem1: Go!
Subsystem2: Fire!
```
