using System;

namespace Command
{
    // Классы Получателей содержат некую важную бизнес-логику. Они умеют
    // выполнять все виды операций, связанных с выполнением запроса. Фактически,
    // любой класс может выступать Получателем.
    internal class Receiver
    {
        public void DoSomething(string a)
        {
            Console.WriteLine($"Receiver. DoSomething on {a}.");
        }

        public void DoSomethingElse(string b)
        {
            Console.WriteLine($"Receiver. DoSomethingElse on {b}.");
        }
    }
}
