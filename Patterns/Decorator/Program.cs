using System;

namespace Decorator
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();

            // Клиент использует простой компонент
            var simple = new ConcreteComponent();
            Console.WriteLine("Client. ConcreteComponent:");
            client.Call(simple);
            Console.WriteLine();

            // Декораторы могут обёртывать не только
            // простые компоненты, но и другие декораторы.
            var decorator1 = new ConcreteDecoratorA(simple);
            Console.WriteLine("Client. ConcreteComponent in DecoratorA:");
            client.Call(decorator1);
            Console.WriteLine();

            var decorator2 = new ConcreteDecoratorB(decorator1);
            Console.WriteLine("Client. ConcreteComponent in Decorators A and B:");
            client.Call(decorator2);
            Console.WriteLine();
        }
    }
}
