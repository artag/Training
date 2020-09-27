using System;

namespace AbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            var client1 = new Client(new ConcreteFactory1());
            var client2 = new Client(new ConcreteFactory2());

            client1.DoWork();
            Console.WriteLine("---");
            client2.DoWork();
        }
    }
}
