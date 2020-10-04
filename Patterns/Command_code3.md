# Команда. Код

## Main (клиент) и результат выполнения

```csharp
static void Main(string[] args)
{
    var simpleCommand = new SimpleCommand("Say Hi!");

    var receiver = new Receiver();
    var complexCommand = new ComplexCommand(receiver, "Send email", "Save report");

    // Клиентский код может параметризовать объект
    // вызывающего класса любыми командами.
    var invoker = new Invoker(simpleCommand, complexCommand);
    invoker.Invoke();
}
```

Результат выполнения:
```
Invoker. Execute start command
SimpleCommand. Execute. Begin.
Printing: Say Hi!
SimpleCommand. Execute. End.

Invoker: Execute finish command
ComplexCommand. Execute. Begin
Receiver. DoSomething on Send email.
Receiver. DoSomethingElse on Save report.
ComplexCommand. Execute. End
```
