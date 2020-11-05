using System;

namespace FactoryMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client: Launched with the ConcreteCreator1.");
            Console.WriteLine("Using constructor:");
            var client1 = new Client1(new ConcreteCreator1());
            client1.DoWork();

            Console.WriteLine();

            Console.WriteLine("Client: Launched with the ConcreteCreator2.");
            Console.WriteLine("Using method:");
            var client2 = new Client2();
            client2.DoWork(new ConcreteCreator2());
        }
    }
}
