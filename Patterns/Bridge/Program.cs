using System;

namespace Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            var abstraction1 = new Abstraction(new ConcreteImplementationA());
            var client1 = new Client(abstraction1);
            client1.DoWork();

            Console.WriteLine();

            var abstraction2 = new ExtendedAbstraction(new ConcreteImplementationB());
            var client2 = new Client(abstraction2);
            client2.DoWork();
        }
    }
}
